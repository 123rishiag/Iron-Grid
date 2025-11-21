using System.Collections;
using Unity.VisualScripting;
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

    [Header("Rotor Visuals")]
    [SerializeField] private Transform rotor;
    [SerializeField] private Transform rotorUnloaded;
    [SerializeField] private Transform rotorLoaded;

    [Header("Front Glow String")]
    [SerializeField] private LineRenderer frontString_L;
    [SerializeField] private LineRenderer frontString_R;

    [Space]
    [SerializeField] private Transform frontStartPoint_L;
    [SerializeField] private Transform frontStartPoint_R;
    [SerializeField] private Transform frontEndPoint_L;
    [SerializeField] private Transform frontEndPoint_R;

    [Header("Back Glow String")]
    [SerializeField] private LineRenderer backString_L;
    [SerializeField] private LineRenderer backString_R;

    [Space]
    [SerializeField] private Transform backStartPoint_L;
    [SerializeField] private Transform backStartPoint_R;
    [SerializeField] private Transform backEndPoint_L;
    [SerializeField] private Transform backEndPoint_R;

    [SerializeField] private LineRenderer[] lineRenderers;

    private Enemy myEnemy;
    private Material material;
    private float currentIntensity;

    private void Awake()
    {
        material = new Material(meshRenderer.material);
        meshRenderer.material = material;

        UpdateMaterialsOnlineRenderers();
        StartCoroutine(ChangeEmission(1));
    }

    private void UpdateMaterialsOnlineRenderers()
    {
        foreach (var lr in lineRenderers)
        {
            lr.material = material;
        }
    }

    private void Update()
    {
        UpdateEmissionColor();
        UpdateStrings();
        UpdateAttackVisualsIfNeeded();
    }

    private void UpdateAttackVisualsIfNeeded()
    {
        if (attackVisuals.enabled && myEnemy != null)
        {
            attackVisuals.SetPosition(1, myEnemy.CenterPoint());
        }
    }

    private void UpdateStrings()
    {
        UpdateStringVisual(frontString_L, frontStartPoint_L, frontEndPoint_L);
        UpdateStringVisual(frontString_R, frontStartPoint_R, frontEndPoint_R);
        UpdateStringVisual(backString_L, backStartPoint_L, backEndPoint_L);
        UpdateStringVisual(backString_R, backStartPoint_R, backEndPoint_R);
    }

    private void UpdateEmissionColor()
    {
        Color emissionColor = Color.Lerp(startColor, endColor, currentIntensity / maxIntensity);

        emissionColor = emissionColor * Mathf.LinearToGammaSpace(currentIntensity);

        material.SetColor("_EmissionColor", emissionColor);
    }

    public void PlayReloadVFX(float duration)
    {
        // To Visually reload the gun, before its reloaded
        float newDuration = duration / 2;

        StartCoroutine(ChangeEmission(newDuration));
        StartCoroutine(UpdateRotorPosition(newDuration));
    }

    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint, Enemy newEnemy)
    {
        StartCoroutine(VFXCoroutine(startPoint, endPoint, newEnemy));
    }

    private IEnumerator VFXCoroutine(Vector3 startPoint, Vector3 endPoint, Enemy newEnemy)
    {
        myEnemy = newEnemy;

        attackVisuals.enabled = true;
        attackVisuals.SetPosition(0, startPoint);
        attackVisuals.SetPosition(1, endPoint);

        yield return new WaitForSeconds(attackVisualDuration);

        attackVisuals.enabled = false;
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

    private IEnumerator UpdateRotorPosition(float duration)
    {
        float startTime = Time.time;

        // Do something repeatedly until the duration has passed
        while (Time.time - startTime < duration)
        {
            // Calculates the proportion of the duration that has elapsed since the start of the coroutine.
            float tValue = (Time.time - startTime) / duration;
            rotor.position = Vector3.Lerp(rotorUnloaded.position, rotorLoaded.position, tValue);
            yield return null;
        }

        rotor.position = rotorLoaded.position;
    }

    private void UpdateStringVisual(LineRenderer lineRenderer, Transform startPoint, Transform endPoint)
    {
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }
}
