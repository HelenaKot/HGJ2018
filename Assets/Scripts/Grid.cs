using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour, IObservable<NodeGraphics>
{
	[SerializeField] private int gridLength = 10;
	[SerializeField] private int gridHeight = 8;

	private Node[,] gridFieldNodes;
	[SerializeField] private GameObject nodePrefab;

	private void CreateGrid()
	{
		gridFieldNodes = new Node[gridLength, gridHeight];
		for (int i = 0; i < gridLength; i++)
		{
			for (int j = 0; i < gridHeight; i++)
			{
				GameObject node = Instantiate(nodePrefab);
				gridFieldNodes[i,j] = node.GetComponent<Node>();
				NodeGraphics ng = node.GetComponent<NodeGraphics>();
				ng.Initialize(i,j);
				Subscribe(ng);
			}
		}
	}

	#region observer code
	private List<IObserver<NodeGraphics>> nodeGraphicsObserverList = new List<IObserver<NodeGraphics>>();

    public void Subscribe(IObserver<NodeGraphics> observer)
    {
        nodeGraphicsObserverList.Add(observer);
    }

    public void Unsubscribe(IObserver<NodeGraphics> observer)
    {
        nodeGraphicsObserverList.Remove(observer);
    }

    public void UpdateObservers()
    {
        foreach (IObserver<NodeGraphics> observer in nodeGraphicsObserverList)
		{
			observer.Update();
		}
    }
	#endregion
}
