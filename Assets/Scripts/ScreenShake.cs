using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public Transform cameraTransform;
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.1f;

    private Vector3 originalCameraPosition;
    private bool isShaking = false;

    private void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    public void Shake(float customShakeAmount)
    {
        if (!isShaking)
        {
            StartCoroutine(DoShake(customShakeAmount));
        }
    }

    private IEnumerator DoShake(float customShakeAmount)
    {
        isShaking = true;
        originalCameraPosition = cameraTransform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            Vector3 randomPoint = originalCameraPosition + (Random.insideUnitSphere * shakeMagnitude * customShakeAmount);

            cameraTransform.localPosition = randomPoint;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalCameraPosition;
        isShaking = false;
    }
}
