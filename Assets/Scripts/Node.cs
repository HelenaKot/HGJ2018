﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	private int health;
	[SerializeField] private int maxHealth = 10;
	[SerializeField] private int minHealth = 10;

	public void Heal(int healAmount)
	{
		health += healAmount;
		if (health > maxHealth)
			health = maxHealth;
	}

	public void Hurt(int hurtAmount)
	{
		health -= hurtAmount;
		if (health < minHealth)
			health = minHealth;
	}
}
