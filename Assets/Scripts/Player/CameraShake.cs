
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _swaySpeed = 1.2f;
    [SerializeField] private float _swayAmount = 0.15f;

    [Header("Noise")]
    [SerializeField] private float _noiseSpeed = 1.5f;
    [SerializeField] private float _noiseAmount = 0.08f;

    [Header("Rotation")]
    [SerializeField] private float _rotationAmount = 8f;
    [SerializeField] private float _rotationSpeed = 0.8f;

    private float _elapsed;
    private float _duration;
    private float _magnitude;
    private float _noiseOffset;

    private bool _shaking;

    private Vector3 _initialLocalPos;
    private Quaternion _initialLocalRot;

    private void Awake()
    {
        _initialLocalPos = transform.localPosition;
        _initialLocalRot = transform.localRotation;
        _noiseOffset = Random.Range(0f, 1000f);
    }

    public void Shake(float duration, float magnitude)
    {
        _elapsed = 0f;
        _duration = duration;
        _magnitude = magnitude;
        _shaking = true;
    }

    private void LateUpdate()
    {
        if (!_shaking)
            return;

        _elapsed += Time.deltaTime;

        if (_elapsed >= _duration)
        {
            _shaking = false;
            transform.localPosition = _initialLocalPos;
            transform.localRotation = _initialLocalRot;
            return;
        }

        float t = _elapsed;

        // Smooth fade in and out
        float fadeIn = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t / 1f));
        float fadeOut = Mathf.SmoothStep(
            0f, 1f, Mathf.Clamp01((_duration - t) / 1.5f)
        );

        float intensity = fadeIn * fadeOut * _magnitude;

        // Drunken sway
        float swayX = Mathf.Sin(t * _swaySpeed) * _swayAmount;
        float swayY = Mathf.Sin(t * _swaySpeed * 0.7f + 2f) * _swayAmount;

        // Smooth noise
        float noiseX = Mathf.PerlinNoise(
            _noiseOffset, t * _noiseSpeed
        ) * 2f - 1f;

        float noiseY = Mathf.PerlinNoise(
            _noiseOffset + 100f, t * _noiseSpeed
        ) * 2f - 1f;

        Vector3 offset = new Vector3(
            swayX + noiseX * _noiseAmount,
            swayY + noiseY * _noiseAmount,
            0f
        );

        // Rotation
        float roll = Mathf.Sin(t * _rotationSpeed) * _rotationAmount;

        float rotationNoise = Mathf.PerlinNoise(
            _noiseOffset + 200f,
            t * _noiseSpeed * 0.5f
        ) * 2f - 1f;

        float finalRoll = (roll + rotationNoise * 3f) * intensity;

        transform.localPosition = _initialLocalPos + offset * intensity;

        transform.localRotation = _initialLocalRot *
            Quaternion.Euler(0f, 0f, finalRoll);
    }
}
