using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGraphics : MonoBehaviour, IObserver<NodeGraphics> {
   public Gradient HealthColor = new Gradient();

    Node node;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        node = GetComponent<Node>();
    }

    void IObserver<NodeGraphics>.Update()
    {
        throw new System.NotImplementedException();
    }

	public void Initialize(int posX, int posY)
	{

	}
    
}
