using UnityEngine;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    public float dayLengthInSeconds = 120f;
    [Range(0f, 1f)] public float timeOfDay = 0f;
    public float startTime = 0.25f;

    [Header("Sun Settings")]
    public Light sun;
    public Gradient sunColor;

    [Header("UI Settings")]
    public TMP_Text timeDisplay;

    private float timeMultiplier;

    void Start()
    {
        if (sun == null)
        {
            Debug.LogError("No Sun assigned.");
            enabled = false;
            return;
        }

        timeOfDay = startTime;
        timeMultiplier = 1f / dayLengthInSeconds;
    }

    void Update()
    {
        timeOfDay += Time.deltaTime * timeMultiplier;
        if (timeOfDay >= 1f) timeOfDay -= 1f;

        float sunRotation = timeOfDay * 360f - 90f;
        sun.transform.rotation = Quaternion.Euler(sunRotation, 170f, 0f);

        if (sunColor != null)
            sun.color = sunColor.Evaluate(timeOfDay);

        if (timeDisplay != null)
        {
            int hours = Mathf.FloorToInt(timeOfDay * 24f);
            int minutes = Mathf.FloorToInt((timeOfDay * 1440f) % 60f);
            timeDisplay.text = $"{hours:00}:{minutes:00}";
        }
    }
}