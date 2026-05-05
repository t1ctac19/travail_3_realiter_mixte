using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private Volume SkyVolume; // assigne ton Volume ici
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    private PhysicallyBasedSky sky;
    private Fog fog;

    private void Start()
    {
        SkyVolume.profile.TryGet(out sky);
        SkyVolume.profile.TryGet(out fog);
    }

    private void Update()
    {
        TimeOfDay += Time.deltaTime;
        TimeOfDay %= 24;
        UpdateLighting(TimeOfDay / 24f);
    }

    private void UpdateLighting(float t)
    {
        if (DirectionalLight != null)
        {
            DirectionalLight.transform.localRotation =
                Quaternion.Euler((t * 360f) - 90f, 170f, 0);

            // Couleur du soleil selon l'heure
            DirectionalLight.color = Color.Lerp(
                new Color(1f, 0.4f, 0.1f),  // lever/coucher (orange)
                new Color(1f, 0.98f, 0.9f), // midi (blanc-jaune)
                Mathf.Sin(t * Mathf.PI)     // arc naturel
            );
        }
    }
}