using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGraphics : MonoBehaviour, IObserver<NodeGraphics> {
    [SerializeField] private Gradient HealthColor = new Gradient();
    [SerializeField] private Transform[] shelfAnchors;
    [SerializeField] private Transform[] columnAnchors;

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
        if (posX >= 0 
            && posX < columnAnchors.Length
            && posY >= 0
            && posY < shelfAnchors.Length)
        {
            transform.position = new Vector3(
                columnAnchors[posX].position.x, 
                shelfAnchors[posY].position.y, 
                shelfAnchors[posY].position.z);
        }
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
