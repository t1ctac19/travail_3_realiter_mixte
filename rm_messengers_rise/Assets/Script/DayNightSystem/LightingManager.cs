using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using TMPro;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private Volume SkyVolume;

    [Header("Time")]
    [SerializeField, Range(0, 24)] private float TimeOfDay = 6f;
    [SerializeField] private float DayDurationSeconds = 120f;
    [SerializeField] private float StartTime = 6f;

    [Header("Sun Intensity")]
    [SerializeField] private float MaxSunIntensity = 5f;

    [Header("UI")]
    [SerializeField] private TMP_Text TimeDisplay;

    private GradientSky gradientSky;
    private Fog fog;
    private HDAdditionalLightData hdLight;

    private void Start()
    {
        TimeOfDay = StartTime;
        InitComponents();
    }

    private void OnEnable()
    {
        InitComponents();
    }

    private void InitComponents()
    {
        if (SkyVolume != null && SkyVolume.profile != null)
        {
            SkyVolume.profile.TryGet(out gradientSky);
            SkyVolume.profile.TryGet(out fog);
        }

        if (DirectionalLight != null)
            hdLight = DirectionalLight.GetComponent<HDAdditionalLightData>();
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            TimeOfDay += (Time.deltaTime / DayDurationSeconds) * 24f;
            TimeOfDay %= 24;
        }

        UpdateLighting(TimeOfDay / 24f);
        UpdateTimeDisplay();
    }

    private void UpdateLighting(float t)
    {
        float ambientMin = 0.08f;
        float sunArc = Mathf.Max(ambientMin,
            Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((t - 0.21f) / 0.12f))
            * Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((0.80f - t) / 0.12f)));

        // ── Rotation & couleur de la lumière ──
        if (DirectionalLight != null)
        {
            DirectionalLight.transform.localRotation =
                Quaternion.Euler((t * 360f) - 90f, 170f, 0f);

            DirectionalLight.color = Color.Lerp(
                new Color(1f, 0.4f, 0.1f),
                new Color(1f, 0.98f, 0.9f),
                Mathf.Clamp01(sunArc)
            );

            if (hdLight != null)
                hdLight.intensity = Mathf.Lerp(0f, MaxSunIntensity, Mathf.Clamp01(sunArc));
        }

        // ── Gradient Sky ──
        if (gradientSky != null)
        {
            gradientSky.top.value = Color.Lerp(
                new Color(0.05f, 0.05f, 0.20f),
                new Color(0.2f, 0.5f, 0.8f),
                Mathf.Clamp01(sunArc)
            );

            gradientSky.middle.value = Color.Lerp(
                new Color(0.04f, 0.04f, 0.15f),
                new Color(0.53f, 0.81f, 0.92f),
                Mathf.Clamp01(sunArc)
            );

            gradientSky.bottom.value = Color.Lerp(
                new Color(0.03f, 0.03f, 0.12f),
                new Color(0.83f, 0.66f, 0.42f),
                Mathf.Clamp01(sunArc)
            );
        }

        // ── Fog ──
        if (fog != null)
            fog.meanFreePath.value = Mathf.Lerp(50f, 400f, Mathf.Clamp01(sunArc));
    }

    private void UpdateTimeDisplay()
    {
        if (TimeDisplay == null) return;

        int hours = Mathf.FloorToInt(TimeOfDay);
        int minutes = Mathf.FloorToInt((TimeOfDay * 60f) % 60f);
        TimeDisplay.text = $"{hours:00}:{minutes:00}";
    }

    private void OnValidate()
    {
        InitComponents();

        if (DirectionalLight != null) return;

        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}