using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public bool active;
	public bool dead;
	public bool sleeping;
	public int health;
	public int maxHealth = 10;
	public int minHealth = -10;
	[SerializeField] private int wakeUpHurt = 5;
	[SerializeField] private int healAmount = 3;
	private Grid grid;

	public void Heal(int healAmount)
	{
		if (active && !dead && health < maxHealth)
		{
			health += healAmount;
			if (health > maxHealth)
				health = maxHealth;
			if (sleeping)
				sleeping = false;
			grid.UpdateGrid();
		}
	}

	public void Hurt(int hurtAmount)
	{
		health -= hurtAmount;
		if (health < minHealth)
		{
			health = minHealth;
			Kill();
		}
	}

	public void Activate()
	{
		active = true;
		sleeping = true;
	}
	
	public void Kill()
	{
		dead = true;
	}
	public void Wake()
	{
		sleeping = false;
		Hurt(wakeUpHurt);
	}

	/// <summary>
	/// OnMouseDown is called when the user has pressed the mouse button while
	/// over the GUIElement or Collider.
	/// </summary>
	void OnMouseDown()
	{
		Heal(healAmount);
	}

	public void SetGridReference(Grid grid)
	{
		this.grid = grid;
	}
}
