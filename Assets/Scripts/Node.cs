using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Node : MonoBehaviour {

	public bool active;
	public bool dead;
	public bool sleeping;
	public int health;
	public int maxHealth = 10;
	public int minHealth = -10;
	[SerializeField] private int wakeUpHurt = 3;
	public int healAmount = 5;
	private Grid grid;

	private TextMeshPro tmpro;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		tmpro = GetComponentInChildren<TextMeshPro>();
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		tmpro.text = health.ToString();
	}

	public void Heal(int healAmount)
	{
		if (active && !dead && health < maxHealth)
		{
			health += healAmount;
			if (health > maxHealth)
				health = maxHealth;
			if (sleeping)
				sleeping = false;
			Debug.Log("Healed");
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
		Heal(healAmount);
	}
}
