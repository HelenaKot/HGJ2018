using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public bool active;
	public bool dead;
	public bool sleeping;
	public int health;
	[SerializeField] public int maxHealth = 10;
	[SerializeField] public int minHealth = -10;

	public void Heal(int healAmount)
	{
		if (active && !dead)
		{
			health += healAmount;
			if (health > maxHealth)
				health = maxHealth;
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
	}
}
