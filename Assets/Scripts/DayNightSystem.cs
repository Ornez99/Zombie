using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightSystem : MonoBehaviour {

    public static DayNightSystem Instance;

    [SerializeField]
    private int dayStartMinute = 420;
    [SerializeField]
    private int nightStartMinute = 1140;
    [SerializeField]
    private float gameMinutesPerSecond = 60;
    [SerializeField]
    private float currentMinute = 450;
    [SerializeField]
    private Light sun = null;
    [SerializeField]
    private Light moon = null;
    private bool lastIsDayCheck;
    [SerializeField]
    private Image timeImage = null;
    private RectTransform timeRectTransform;
    private Color32 dayColor = new Color32(0xE4, 0xD2, 0x67, 0xFF);
    private Color32 nightColor = new Color32(0x18, 0x3A, 0x58, 0xFF);


    public bool IsDay => (currentMinute > dayStartMinute && currentMinute < nightStartMinute);
    public int GetHour => Mathf.FloorToInt(currentMinute / 60);

    public void Initialize() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There can be only one instance of this script!");
            Destroy(this);
        }
        timeRectTransform = timeImage.GetComponent<RectTransform>();
        lastIsDayCheck = !IsDay;
        UpdateTimeSlider();
        lastIsDayCheck = IsDay;
        UpdateSun();
        sun.gameObject.SetActive(IsDay);
        moon.gameObject.SetActive(!IsDay);
        Instance = this;
    }

    private void Update() {
        currentMinute += Time.deltaTime * gameMinutesPerSecond;
        if (currentMinute >= 1440)
            currentMinute -= 1440;

        UpdateTimeSlider();
        UpdateSun();
        lastIsDayCheck = IsDay;
    }


    private void UpdateTimeSlider() {
        if (lastIsDayCheck != IsDay) {
            timeImage.color = (IsDay) ? dayColor : nightColor;
            
        }
        float timeImageWidth;

        if (IsDay)
            timeImageWidth = (currentMinute - dayStartMinute) * 2.666F;
        else {
            if (currentMinute < dayStartMinute)
                timeImageWidth = (currentMinute + (1440 - nightStartMinute)) * 2.666F;
            else
                timeImageWidth = (currentMinute - nightStartMinute) * 2.666F;
        }
        timeRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, timeImageWidth);
    }

    private void UpdateSun() {
        if (lastIsDayCheck != IsDay) {
            sun.gameObject.SetActive(IsDay);
            moon.gameObject.SetActive(!IsDay);
        }

        /*if (IsDay) {
            sun.gameObject.SetActive(true);
            moon.gameObject.SetActive(false);
        } 
        else {
            sun.gameObject.SetActive(false);
            moon.gameObject.SetActive(true);
        }*/
    }


}
