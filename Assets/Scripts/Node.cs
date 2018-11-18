using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Node : MonoBehaviour {

	public bool active;
	public bool dead;
	public bool sleeping;
	public int health;
	public int maxHealth;
	public int minHealth;
	[SerializeField] private int wakeUpHurt;
	public int healAmount;
	private Grid grid;
	[SerializeField] private AudioSource deathSound;
	[SerializeField] private AudioSource clickSound;

	public int posX;
	public int posY;

	public bool finished;

	public void Heal(int healAmount, bool update)
	{
		if (active && !dead && health < maxHealth)
		{
			health += healAmount;
			
			if (health > maxHealth)
				health = maxHealth;
			
			if (sleeping)
				sleeping = false;
			
			Debug.Log("Healed");

			if(update)			
				grid.UpdateGrid();
		}
	}

	public void Hurt(int hurtAmount)
	{
		health -= hurtAmount;
		if (health <= minHealth)
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
		deathSound.pitch = Random.Range(0.5f, 1f);
		deathSound.Play();
	}
	public void Wake()
	{
		sleeping = false;
		Hurt(wakeUpHurt);
	}

	public void SetGridReference(Grid grid)
	{
		this.grid = grid;
	}

	/// <summary>
	/// OnMouseDown is called when the user has pressed the mouse button while
	/// over the GUIElement or Collider.
	/// </summary>
	void OnMouseDown()
	{
		if(!finished && !dead && health != maxHealth)
		{
			Heal(healAmount, true);
			grid.HealNeighbours(posX, posY);
			
			clickSound.pitch = Random.Range(0.5f, 1f);
			clickSound.Play();
		}
	}
}
