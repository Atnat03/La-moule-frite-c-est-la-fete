using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingTimerBar : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private Slider ballSlider;
    [SerializeField] private Image ballSliderImage;
    [SerializeField] private RectTransform slider, badZoneMin, greatZone, badZoneMax;
    [SerializeField] private Color32 inactiveZone, activeZone;
    private float timeShootMin;
    private float timeShootMax;
    public bool goodTime;
    
    public void InitializeTimerBar(float timeMin, float timeMax, float totalTime)
    {
        float scaleFactor = ballSlider.transform.GetComponent<RectTransform>().sizeDelta.x;
        badZoneMin.sizeDelta = new(timeMin * scaleFactor / totalTime, badZoneMin.sizeDelta.y);
        greatZone.sizeDelta = new(((timeMax - timeMin) * scaleFactor) / totalTime, greatZone.sizeDelta.y);
        badZoneMax.sizeDelta = new(((totalTime - timeMax) * scaleFactor) / totalTime, badZoneMax.sizeDelta.y);
        
        ballSlider.minValue = 0;
        ballSlider.maxValue = totalTime;
    }
    public void HandleTimer(float time, float timeMin, float timeMax)
    {
        ballSlider.value = time;
        if(time >= timeMin && time <= timeMax)
        {
            ballSliderImage.color = activeZone;
            goodTime = true;
        }
        else
        {
            ballSliderImage.color = inactiveZone;
            goodTime = false;
        }
    }
}
