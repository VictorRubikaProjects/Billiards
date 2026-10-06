using System;
using System.Collections;
using Event_Bus;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private AnimationCurve _curveShake;
    [SerializeField] private ParticleSystem _particleSystemGood;
    private Coroutine _coroutineRotation;
    private Coroutine _coroutineShake;
    private Quaternion _cameraTargetRotation;
    private EventBinding<OnCameraShake>  m_onCameraShakeBinding;

    private void Awake()
    {
        _cameraTargetRotation = _camera.transform.rotation;

        m_onCameraShakeBinding = new EventBinding<OnCameraShake>(Shake);
        EventBus<OnCameraShake>.Register(m_onCameraShakeBinding);
    }

    private void OnDestroy()
    {
        EventBus<OnCameraShake>.Unregister(m_onCameraShakeBinding);
    }

    private void Update()
    {
        HandleInputRotation();   
    }

    private void HandleInputRotation()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;
        
        if (_coroutineRotation != null) StopCoroutine(_coroutineRotation);

        _cameraTargetRotation = Quaternion.Euler(0, 90f, 0) * _cameraTargetRotation;
        _coroutineRotation = StartCoroutine(RotateCamera(_camera.transform.rotation,_cameraTargetRotation));
    }
    
    
    IEnumerator RotateCamera(Quaternion currentRotation, Quaternion targetRotation)
    {
        float elapsedTime = 0;
        float duration = 0.5f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, elapsedTime / duration);
            yield return null;
        }
        
        transform.rotation = targetRotation;
    }

    private void Shake()
    {
        if (_coroutineShake != null) StopCoroutine(_coroutineShake);
        _coroutineShake = StartCoroutine(Shaking());
    }
    
    IEnumerator Shaking()
    {
        _particleSystemGood.Stop();
        _particleSystemGood.Clear();
        _particleSystemGood.Play();
        Vector3 startPosition = transform.position;
        
        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime +=  Time.deltaTime;
            float strenght = _curveShake.Evaluate(elapsedTime/duration);
            transform.position = startPosition + Random.insideUnitSphere * strenght;
            yield return null;
        }
        transform.position = startPosition;
    }
}
