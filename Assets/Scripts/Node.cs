using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public bool active;
	public bool dead;
	public bool sleeping;
	public int health;
	[SerializeField] private int maxHealth = 10;
	[SerializeField] private int minHealth = -10;
	[SerializeField] private int wakeUpHurt = 5;

	public void Heal(int healAmount)
	{
		if (active && !dead)
		{
			health += healAmount;
			if (health > maxHealth)
				health = maxHealth;
			if (sleeping)
				sleeping = false;
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
}
