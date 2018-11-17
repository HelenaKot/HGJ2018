using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGraphics : MonoBehaviour, IObserver<NodeGraphics> {
    public Gradient HealthColor = new Gradient();
    Node node;
    private int healthPoints;
    private Material myMaterial;

    void Awake()
    {
        node = GetComponent<Node>();
        healthPoints = node.maxHealth + Mathf.Abs(node.minHealth);
        myMaterial = GetComponentInChildren<Renderer>().materials[1];
        myMaterial.EnableKeyword("_EMISSION");
        node.health = 4;
        updateColor();
    }

    public void Initialize(int posX, int posY)
    {
       
    }

    void IObserver<NodeGraphics>.Update()
    {
        updateColor();
    }

    private void updateColor()
    {
        Color glowColor = HealthColor.Evaluate((node.health + Mathf.Abs(node.minHealth)) / (float)healthPoints);
        myMaterial.SetColor("_EmissionColor", glowColor);
        myMaterial.SetColor("_Color", glowColor);
    }
    
}
