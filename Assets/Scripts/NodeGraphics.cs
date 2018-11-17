using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGraphics : MonoBehaviour, IObserver<NodeGraphics> {
   public Gradient HealthColor = new Gradient();

    void IObserver<NodeGraphics>.Update()
    {
        throw new System.NotImplementedException();
    }

	public void Initialize(int posX, int posY)
	{

	}
    
}
