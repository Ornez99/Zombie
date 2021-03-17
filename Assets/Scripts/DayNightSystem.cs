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
    private Slider timeSlider = null;
    [SerializeField]
    private Image sliderImage = null;

    [SerializeField]
    private Light sun = null;

    [SerializeField]
    private Light moon = null;

    public bool IsDay => (currentMinute > dayStartMinute && currentMinute < nightStartMinute);
    public int GetHour => Mathf.FloorToInt(currentMinute / 60);

    public void Initialize() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There can be only one instance of this script!");
            Destroy(this);
        }

        Instance = this;
    }

    private void Update() {
        currentMinute += Time.deltaTime * gameMinutesPerSecond;
        if (currentMinute >= 1440)
            currentMinute -= 1440;
        UpdateTimeSlider();
        UpdateSun();
    }


    private void UpdateTimeSlider() {
        if (IsDay)
            timeSlider.value = currentMinute - dayStartMinute;
        else {
            if (currentMinute < dayStartMinute)
                timeSlider.value = currentMinute + (1440 - nightStartMinute);
            else
                timeSlider.value = currentMinute - nightStartMinute;
        }

        if (IsDay)
            sliderImage.color = Color.yellow;
        else
            sliderImage.color = Color.blue;
    }

    private void UpdateSun() {
        if (IsDay) {
            sun.gameObject.SetActive(true);
            moon.gameObject.SetActive(false);
        } 
        else {
            sun.gameObject.SetActive(false);
            moon.gameObject.SetActive(true);
        }
    }


}
