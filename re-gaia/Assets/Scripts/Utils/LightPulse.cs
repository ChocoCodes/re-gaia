using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightPulse : MonoBehaviour
{
    public Light2D spotLight;
    [SerializeField] private float minRadius = 1.95f;
    [SerializeField] private float maxRadius = 2.23f;
    [SerializeField] private float speed = 1.25f;

    private void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f; // Goes from 0 to 1
        spotLight.pointLightOuterRadius = Mathf.Lerp(minRadius, maxRadius, t);
    }
}