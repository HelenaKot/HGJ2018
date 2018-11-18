using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public float barWidth;

	[SerializeField] private RectTransform positiveBar;
	[SerializeField] private RectTransform negativeBar;

	public void UpdateHealthBar(float negativePoints, float positivePoints)
	{
		float totalPoints = negativePoints + positivePoints;

		float pointWidth = barWidth/totalPoints;

		positiveBar.sizeDelta = new Vector2(positivePoints*pointWidth, positiveBar.sizeDelta.y);
		negativeBar.sizeDelta = new Vector2(negativePoints*pointWidth, negativeBar.sizeDelta.y);
	}
}
