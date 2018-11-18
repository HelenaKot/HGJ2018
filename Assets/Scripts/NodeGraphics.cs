using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGraphics : MonoBehaviour, IObserver<NodeGraphics> {
    [SerializeField] private Gradient HealthColor = new Gradient();
    [SerializeField] private Gradient FaceColor = new Gradient();
    [SerializeField] private Sprite[] faces;
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
    private SpriteMask mySpriteMask;

    private bool interpolatingColor;

    private Color startColor;
    private Color targetColor;

    private float emission;
    private float lerpStage = 0;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (interpolatingColor)
        {
            lerpStage += 0.6f*Time.deltaTime;
            
            myMaterial.SetColor("_EmissionColor", Color.Lerp(startColor, targetColor, lerpStage) * Mathf.LinearToGammaSpace(minEmission + emission));
            myMaterial.SetColor("_Color", Color.Lerp(startColor, targetColor, lerpStage));
            
            if (lerpStage >= 1)
            {
                interpolatingColor = false;
                lerpStage = 0;
            }
        }
    }

    void Awake()
    {
        node = GetComponent<Node>();
        healthPoints = node.maxHealth + Mathf.Abs(node.minHealth);
        myMaterial = GetComponentInChildren<Renderer>().materials[1];
        mySprite = GetComponentInChildren<SpriteRenderer>();
        mySpriteMask = GetComponentInChildren<SpriteMask>();
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
            mySpriteMask.sprite = faces[0];
            mySprite.color = faceColorSleep;
        }
        else
        {
            float healthPercent = (node.health + Mathf.Abs(node.minHealth)) / (float)healthPoints;
            if (node.dead)
            {
                mySpriteMask.sprite = faces[faces.Length - 1];
                mySprite.color = faceColorSleep;
            } else
            {
                updateFace(healthPercent);
                mySprite.color = FaceColor.Evaluate(healthPercent);
            }
            Color glowColor = HealthColor.Evaluate(healthPercent);
            UpdateColor(glowColor, intensity * healthPercent);
        }
    }

    private void UpdateColor(Color color, float emission)
    {
        startColor = myMaterial.GetColor("_Color");
        targetColor = color;
        this.emission = emission;
        interpolatingColor = true;
        lerpStage = 0;

        //myMaterial.SetColor("_EmissionColor", color * Mathf.LinearToGammaSpace(minEmission + emission));
        //myMaterial.SetColor("_Color", color);
    }

    private void updateFace(float percent)
    {
        if (faces.Length > 0)
        {
            if (node.sleeping)
            {
                mySpriteMask.sprite = faces[0];
            }
            else if (percent.Equals(1F))
            {
                mySpriteMask.sprite = faces[1];
            }
            else
            {
                // int 1 : faces.Length
                int validFaces = faces.Length - 2;
                int face =  (int) (validFaces + 1 - ((validFaces - 1) * percent));
                mySpriteMask.sprite = faces[face];
            }
        }
    }

    
}
