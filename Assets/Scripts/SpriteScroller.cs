using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScroller : MonoBehaviour {
    public float speed = 0.05F;
    private Material myMaterial;
    private float offset = 0;

    void Start () {
        myMaterial = GetComponentInChildren<SpriteRenderer>().material;
    }
	
	void Update () {
        offset += speed;
        myMaterial.mainTextureOffset = new Vector2(0, offset);
    }
}
