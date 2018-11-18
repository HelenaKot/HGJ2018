using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public float barWidth;
	private float pointWidth;

	[SerializeField] private RectTransform positiveBar;
	[SerializeField] private RectTransform negativeBar;
	public void Setup(float maxHealth, float minHealth, int numberOfNodes)
	{
		float totalPoints = (maxHealth + Mathf.Abs(minHealth)) * numberOfNodes;
		pointWidth = barWidth/totalPoints;
		pointWidth *= 2;
	}

	public void UpdateHealthBar(float negativePoints, float positivePoints)
	{
		positiveBar.sizeDelta = new Vector2(positivePoints*pointWidth, positiveBar.sizeDelta.y);
		negativeBar.sizeDelta = new Vector2(negativePoints*pointWidth, negativeBar.sizeDelta.y);
	}
}
