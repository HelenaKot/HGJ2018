using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour, IObservable<NodeGraphics>
{
	[SerializeField] private int gridLength = 10;
	[SerializeField] private int gridHeight = 8;

	private Node[,] gridFieldNodes;
	[SerializeField] private GameObject nodePrefab;

	[SerializeField] private int hurtAmount;

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

	public void UpdateGrid()
	{		
		List<Node> inactiveNodes = new List<Node>();
		for (int i = 0; i < gridLength; i++)
		{
			for (int j = 0; i < gridHeight; i++)
			{
				Node currentNode = gridFieldNodes[i,j];
				if(!currentNode.active)
				{
					inactiveNodes.Add(currentNode);
				}
				else if (currentNode.sleeping)
				{
					currentNode.Wake();
				}
				else if (currentNode.health <0 && !currentNode.dead)
				{
					currentNode.Hurt(hurtAmount);					
					foreach (Node node in GetNeighbours(i,j))
					{
						if(node.active && !node.sleeping)
							node.Hurt(hurtAmount);
					}
				}
			}
		}
		inactiveNodes[Random.Range(0, inactiveNodes.Count)].Activate();
		UpdateObservers();
	}

	private Node[] GetNeighbours(int posX, int posY)
	{
		List<Node> neighbours = new List<Node>();
		if (posX != 0)
		{
			neighbours.Add(gridFieldNodes[posX-1,posY]);
		}
		if (posY != 0)
		{
			neighbours.Add(gridFieldNodes[posX, posY-1]);
		}
		if (posX != gridLength)
		{
			neighbours.Add(gridFieldNodes[posX+1, posY]);
		}
		if (posY != gridHeight)
		{
			neighbours.Add(gridFieldNodes[posX, posY+1]);
		}

		return neighbours.ToArray();
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
