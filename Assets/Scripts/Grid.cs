using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour, IObservable<NodeGraphics>
{
	[SerializeField] private int gridLength = 7;
	[SerializeField] private int gridHeight = 6;

	private Node[,] gridFieldNodes;
	[SerializeField] private GameObject nodePrefab;

	[SerializeField] private int hurtAmount = 1;
	[SerializeField] private int activatedAmount = 2;

	[SerializeField] private Rack rack;

	private bool lastRound;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		CreateGrid();
	}

	private void CreateGrid()
	{
		gridFieldNodes = new Node[gridLength, gridHeight];		
		for (int i = 0; i < gridLength; i++)
		{
			for (int j = 0; j < gridHeight; j++)
			{
				GameObject node = Instantiate(nodePrefab);
				Node nodeScript = node.GetComponent<Node>();
				nodeScript.SetGridReference(this);
				gridFieldNodes[i,j] = nodeScript;
				NodeGraphics ng = node.GetComponent<NodeGraphics>();
				ng.SetRackReference(rack);
				ng.Initialize(i,j);
				Subscribe(ng);
			}
		}

		foreach(Node node in GetStartNodes())
		{
			node.active = true;
			node.health = Random.Range(-3,4);
		}

		UpdateObservers();
	}
	
	public void UpdateGrid()
	{		
		List<Node> inactiveNodes = new List<Node>();
		for (int i = 0; i < gridLength; i++)
		{
			for (int j = 0; j < gridHeight; j++)
			{
				Node currentNode = gridFieldNodes[i,j];
				
				if(currentNode.active)
				{
					foreach (Node node in GetNeighbours(i,j))
					{
						if(!node.active)
						{
							inactiveNodes.Add(node);
						}
					}
					if (currentNode.health <0 && !currentNode.dead)
					{
						//currentNode.Hurt(hurtAmount);					
						foreach (Node node in GetNeighbours(i,j))
						{
							if(node.active && !node.sleeping)
								node.Hurt(hurtAmount);
						}
					}
					if (currentNode.sleeping)
					{
						currentNode.Wake();
					}
				}				
			}
		}

		if(!lastRound)
		{
			if(inactiveNodes.Count > activatedAmount)
			{
				for (int i = 0; i<activatedAmount; i++)
				{
					Node currentNode = inactiveNodes[Random.Range(0, inactiveNodes.Count)];
					currentNode.Activate();
					inactiveNodes.Remove(currentNode);
				}

				if (inactiveNodes.Count == 0)
					lastRound = true;

				UpdateObservers();
			}			
			else if (inactiveNodes.Count > 0)
			{
				for (int i = 0; i< inactiveNodes.Count; i++)
				{
					Node currentNode = inactiveNodes[Random.Range(0, inactiveNodes.Count)];
					currentNode.Activate();
					inactiveNodes.Remove(currentNode);
				}

				lastRound = true;

				UpdateObservers();
			}
		}
		else
		{			
			FinishGame();
			UpdateObservers();
		}
	}

	private List<Node> GetStartNodes()
	{
		List<Node> startNodes = new List<Node>();
		if (gridLength % 2 == 0 && gridHeight % 2 == 0)
		{
			startNodes.Add(gridFieldNodes[gridLength/2,gridHeight/2]);
			startNodes.Add(gridFieldNodes[gridLength/2+1,gridHeight/2]);
			startNodes.Add(gridFieldNodes[gridLength/2,gridHeight/2+1]);
			startNodes.Add(gridFieldNodes[gridLength/2+1,gridHeight/2+1]);
		}
		else if (gridLength % 2 != 0 && gridHeight % 2 == 0)
		{
			int middleX = Mathf.FloorToInt(gridLength/2);

			startNodes.Add(gridFieldNodes[middleX,gridHeight/2]);
			startNodes.Add(gridFieldNodes[middleX+1,gridHeight/2]);
			startNodes.Add(gridFieldNodes[middleX-1,gridHeight/2]);
			startNodes.Add(gridFieldNodes[middleX,gridHeight/2+1]);
			startNodes.Add(gridFieldNodes[middleX+1,gridHeight/2+1]);
			startNodes.Add(gridFieldNodes[middleX-1,gridHeight/2+1]);
		}
		else if (gridLength % 2 == 0 && gridHeight % 2 != 0)
		{
			int middleY = Mathf.FloorToInt(gridHeight/2);

			startNodes.Add(gridFieldNodes[gridLength/2,middleY]);
			startNodes.Add(gridFieldNodes[gridLength/2,middleY+1]);
			startNodes.Add(gridFieldNodes[gridLength/2,middleY-1]);
			startNodes.Add(gridFieldNodes[gridLength/2+1,middleY]);
			startNodes.Add(gridFieldNodes[gridLength/2+1,middleY+1]);
			startNodes.Add(gridFieldNodes[gridLength/2+1,middleY-1]);
		}
		else
		{
			int middleX = Mathf.FloorToInt(gridLength/2);
			int middleY = Mathf.FloorToInt(gridHeight/2);

			startNodes.Add(gridFieldNodes[middleX,middleY]);
			startNodes.Add(gridFieldNodes[middleX+1, middleY]);
			startNodes.Add(gridFieldNodes[middleX-1,middleY]);
			startNodes.Add(gridFieldNodes[middleX, middleY+1]);
			startNodes.Add(gridFieldNodes[middleX, middleY-1]);
		}

		return startNodes;
	}


	private void FinishGame()
	{
		int finalScore = 0;
		foreach (Node node in gridFieldNodes)
		{
			finalScore += node.health;
		}
		Debug.Log(finalScore);
		if (finalScore >= 0)
			Win();
		else
			Lose();
	}

	private void Win()
	{
		Debug.Log("Win");
	}

	private void Lose()
	{
		Debug.Log("Lose");
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
		if (posX != gridLength-1)
		{
			neighbours.Add(gridFieldNodes[posX+1, posY]);
		}
		if (posY != gridHeight-1)
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
