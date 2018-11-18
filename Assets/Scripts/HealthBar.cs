using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public int barWidth;
	private int pointWidth;

	[SerializeField] private RectTransform positiveBar;
	[SerializeField] private RectTransform negativeBar;
	public void Setup(int maxHealth, int minHealth, int numberOfNodes)
	{
		int totalPoints = (maxHealth + Mathf.Abs(minHealth)) * numberOfNodes;
		pointWidth = pointWidth/totalPoints;
	}

	public void UpdateHealthBar(int negativePoints, int positivePoints)
	{
		positiveBar.sizeDelta = new Vector2(positivePoints*pointWidth, positiveBar.sizeDelta.y);
		negativeBar.sizeDelta = new Vector2(negativePoints*pointWidth, negativeBar.sizeDelta.y);
	}
}
