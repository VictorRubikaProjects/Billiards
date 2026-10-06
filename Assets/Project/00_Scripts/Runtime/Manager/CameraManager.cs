using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private Coroutine _coroutineRotation;
    private Quaternion _cameraTargetRotation;

    private void Awake() => _cameraTargetRotation = _camera.transform.rotation;

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
}
