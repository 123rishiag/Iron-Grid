using System.Collections;
using UnityEngine;

public class Crossbow_Visuals : MonoBehaviour
{
    [SerializeField] private LineRenderer attackVisuals;
    [SerializeField] private float attackVisualDuration = 0.1f;

    [Header("Glowing Visuals")]
    [SerializeField] private MeshRenderer meshRenderer;

    [Space]
    [SerializeField] private float maxIntensity = 150f;
    [Space]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    private Tower_Crossbow myTower;
    private Material material;
    private float currentIntensity;

    private void Awake()
    {
        myTower = GetComponent<Tower_Crossbow>();

        material = new Material(meshRenderer.material);

        meshRenderer.material = material;

        StartCoroutine(ChangeEmission(1));
    }

    private void Update()
    {
        UpdateEmissionColor();
    }

    private void UpdateEmissionColor()
    {
        Color emissionColor = Color.Lerp(startColor, endColor, currentIntensity / maxIntensity);

        emissionColor = emissionColor * Mathf.LinearToGammaSpace(currentIntensity);

        material.SetColor("_EmissionColor", emissionColor);
    }

    public void PlayReloadVFX(float duration)
    {
        StartCoroutine(ChangeEmission(duration / 2));
    }

    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(VFXCoroutine(startPoint, endPoint));
    }

    private IEnumerator VFXCoroutine(Vector3 startPoint, Vector3 endPoint)
    {
        myTower.EnableRotation(false);

        attackVisuals.enabled = true;
        attackVisuals.SetPosition(0, startPoint);
        attackVisuals.SetPosition(1, endPoint);

        yield return new WaitForSeconds(attackVisualDuration);

        attackVisuals.enabled = false;

        myTower.EnableRotation(true);
    }

    private IEnumerator ChangeEmission(float duration)
    {
        float startTime = Time.time;
        float startIntensity = 0f;

        // Do something repeatedly until the duration has passed
        while(Time.time - startTime < duration)
        {
            // Calculates the proportion of the duration that has elapsed since the start of the coroutine.
            float tValue = (Time.time - startTime) / duration;
            currentIntensity = Mathf.Lerp(startIntensity, maxIntensity, tValue);
            yield return null;
        }

        currentIntensity = maxIntensity;
    }
}
