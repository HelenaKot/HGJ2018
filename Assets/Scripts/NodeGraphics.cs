using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGraphics : MonoBehaviour, IObserver<NodeGraphics> {
    [SerializeField] private Gradient HealthColor = new Gradient();
    [SerializeField] private Gradient FaceColor = new Gradient();
    [SerializeField] private Color offColor = Color.black;
    [SerializeField] private Color sleepColor = Color.gray;
    [SerializeField] private Color faceColorSleep = Color.white;
    [SerializeField] float intensity = 1.4F;
    [SerializeField] float minEmission = 0.8F;

    Rack rack; //todo!
    Node node;
    private int healthPoints;
    private Material myMaterial;
    private SpriteRenderer mySprite;

    void Awake()
    {
        node = GetComponent<Node>();
        healthPoints = node.maxHealth + Mathf.Abs(node.minHealth);
        myMaterial = GetComponentInChildren<Renderer>().materials[1];
        mySprite = GetComponentInChildren<SpriteRenderer>();
        myMaterial.EnableKeyword("_EMISSION");
        refreshView();
    }

    public void SetRackReference(Rack rack)
    {
        this.rack = rack;
    }

    public void Initialize(int posX, int posY)
    {
        transform.position = rack.getTransform(posX, posY);
    }

    void IObserver<NodeGraphics>.Update()
    {
        refreshView();
    }

    private void refreshView()
    {
        mySprite.enabled = node.active;
        if (!node.active)
        {
            UpdateColor(offColor, 0);
        }
        else if (node.sleeping)
        {
            UpdateColor(sleepColor, 0);
            mySprite.color = faceColorSleep;
        }
        else
        {
            float healthPercent = (node.health + Mathf.Abs(node.minHealth)) / (float)healthPoints;
            mySprite.color = FaceColor.Evaluate(healthPercent);
            Color glowColor = HealthColor.Evaluate(healthPercent);
            UpdateColor(glowColor, intensity * healthPercent);
        }
    }

    private void UpdateColor(Color color, float emission)
    {
        myMaterial.SetColor("_EmissionColor", color * Mathf.LinearToGammaSpace(minEmission + emission));
        myMaterial.SetColor("_Color", color);
    }

    
}
