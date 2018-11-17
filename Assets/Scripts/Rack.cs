using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rack : MonoBehaviour {
    [SerializeField] private Transform[] shelfAnchors;
    [SerializeField] private Transform[] columnAnchors;
    
    public Vector3 getTransform(int posX, int posY)
    {
        if (posX >= 0
           && posX < columnAnchors.Length
           && posY >= 0
           && posY < shelfAnchors.Length)
        {
            return new Vector3(
                columnAnchors[posX].position.x,
                shelfAnchors[posY].position.y,
                shelfAnchors[posY].position.z);
        }   
        return new Vector3(0, 0, 0);
    }
}
