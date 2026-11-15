using UnityEngine;
using UnityEngine.UI;

public class AlcoolBar : MonoBehaviour
{
	[SerializeField] private Slider alcoolBar;
	[SerializeField] private RectTransform maxAlcoolBar;
	
	
	public void AddAlcool(float value)
	{
		if (alcoolBar.value + value >= alcoolBar.maxValue)
			alcoolBar.value = alcoolBar.maxValue;
		else
			alcoolBar.value = value;
	}

	public void InitAlcoolBar(float maxAlcoolBarValue)
	{
		maxAlcoolBar.offsetMin = new Vector2(150f * maxAlcoolBarValue, maxAlcoolBar.offsetMin.y);
	}
}
