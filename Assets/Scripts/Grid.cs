﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Grid : MonoBehaviour, IObservable<NodeGraphics>
{
	[SerializeField] private int gridLength;
	[SerializeField] private int gridHeight;

	private Node[,] gridFieldNodes;
	[SerializeField] private GameObject nodePrefab;

	[SerializeField] private int hurtAmount;
	[SerializeField] private int activatedAmount;
	[SerializeField] private int neighbourHealAmount;

	[SerializeField] private Rack rack;
	[SerializeField] private HealthBar healthBar;

	private bool lastRound;

	[SerializeField] private GameObject winLose;
	[SerializeField] private GameObject startScreen;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		CreateGrid();
	}

	public void CreateGrid()
	{
		gridFieldNodes = new Node[gridLength, gridHeight];		
		for (int i = 0; i < gridLength; i++)
		{
			for (int j = 0; j < gridHeight; j++)
			{
				GameObject node = Instantiate(nodePrefab);
				Node nodeScript = node.GetComponent<Node>();
				nodeScript.SetGridReference(this);
				nodeScript.posX = i;
				nodeScript.posY = j;
				gridFieldNodes[i,j] = nodeScript;
				NodeGraphics ng = node.GetComponent<NodeGraphics>();
				ng.SetRackReference(rack);
				ng.Initialize(i,j);
				Subscribe(ng);
			}
		}
	}
		
		public void StartGame()
		{			
		startScreen.SetActive(false);
		healthBar.gameObject.SetActive(true);

		int negativePoints = 0;
		int positivePoints = 0;

		foreach(Node node in GetStartNodes())
		{
			node.active = true;
			node.health = Random.Range(-3,4);
			if (node.health>0)
			{
				positivePoints += node.health;
			}
			else if (node.health < 0)
			{
				negativePoints += Mathf.Abs(node.health);
			}
		}

		UpdateObservers();
		healthBar.UpdateHealthBar(negativePoints, positivePoints);
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

		UpdateHealth();

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
			else
			{
				FinishGame();
				UpdateObservers();
			}
		}
		else
		{			
			FinishGame();
			UpdateObservers();
		}
	}

	public void HealNeighbours(int posX, int posY)
	{
		Node[] neighbours = GetNeighbours(posX, posY);
		foreach(Node node in neighbours)
		{
			if(node.active && !node.dead && !node.sleeping)
			{
				node.Heal(neighbourHealAmount, false);
			}
		}
	}

	public void Restart()
	{
		foreach(Node node in gridFieldNodes)
		{
			Unsubscribe(node.GetComponent<NodeGraphics>());
			Destroy(node.gameObject);
		}
		winLose.SetActive(false);
		lastRound = false;
		CreateGrid();
		StartGame();
	}

	private void UpdateHealth()
	{
		int positivePoints = 0;
		int negativePoints = 0;
		foreach (Node node in gridFieldNodes)
		{
			if (node.health>0)
			{
				positivePoints += node.health;
			}
			else if (node.health < 0)
			{
				negativePoints += Mathf.Abs(node.health);
			}
		}
		healthBar.UpdateHealthBar(negativePoints, positivePoints);
	}
	private List<Node> GetStartNodes()
	{
		List<Node> startNodes = new List<Node>();
		if (gridLength % 2 == 0 && gridHeight % 2 == 0)
		{
			startNodes.Add(gridFieldNodes[gridLength/2-1,gridHeight/2-1]);
			startNodes.Add(gridFieldNodes[gridLength/2,gridHeight/2-1]);
			startNodes.Add(gridFieldNodes[gridLength/2-1,gridHeight/2]);
			startNodes.Add(gridFieldNodes[gridLength/2,gridHeight/2]);
		}
		else if (gridLength % 2 != 0 && gridHeight % 2 == 0)
		{
			int middleX = Mathf.FloorToInt(gridLength/2);

			startNodes.Add(gridFieldNodes[middleX,gridHeight/2-1]);
			startNodes.Add(gridFieldNodes[middleX+1,gridHeight/2-1]);
			startNodes.Add(gridFieldNodes[middleX-1,gridHeight/2-1]);
			startNodes.Add(gridFieldNodes[middleX,gridHeight/2]);
			startNodes.Add(gridFieldNodes[middleX+1,gridHeight/2]);
			startNodes.Add(gridFieldNodes[middleX-1,gridHeight/2]);
		}
		else if (gridLength % 2 == 0 && gridHeight % 2 != 0)
		{
			int middleY = Mathf.FloorToInt(gridHeight/2);

			startNodes.Add(gridFieldNodes[gridLength/2-1,middleY]);
			startNodes.Add(gridFieldNodes[gridLength/2-1,middleY+1]);
			startNodes.Add(gridFieldNodes[gridLength/2-1,middleY-1]);
			startNodes.Add(gridFieldNodes[gridLength/2,middleY]);
			startNodes.Add(gridFieldNodes[gridLength/2,middleY+1]);
			startNodes.Add(gridFieldNodes[gridLength/2,middleY-1]);
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
		foreach(Node node in gridFieldNodes)
		{
			node.finished = true;
		}
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
		winLose.SetActive(true);
		winLose.GetComponentInChildren<TextMeshProUGUI>().text = "YOU WIN";
	}

	private void Lose()
	{
		Debug.Log("Lose");
		winLose.SetActive(true);
		winLose.GetComponentInChildren<TextMeshProUGUI>().text = "YOU LOSE";
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
