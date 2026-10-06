using System;
using Event_Bus;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private EventBinding<OnContactBall> m_onContactBinding;
    private float m_currentSpeed = 0;
    
    private void Start()
    {
        EventBus<OnBallSpawnEvent>.Raise(new OnBallSpawnEvent(
            transform,
            this));

        m_onContactBinding = new EventBinding<OnContactBall>(OnContactWall);
        
        EventBus<OnContactBall>.Register(m_onContactBinding);
    }

    private void Update()
    {
        HandleMoving();
    }

    private void OnDestroy()
    {
        EventBus<OnBallDespawnEvent>.Raise(new OnBallDespawnEvent(
            transform,
            this));
        
        EventBus<OnContactBall>.Unregister(m_onContactBinding);
    }

    public void Rotate(bool right)
    {
        float direction = right ? 1 : -1;
        transform.Rotate(Vector3.up, direction * Time.deltaTime * 60f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 0.2f);
    }

    private void OnContactWall(OnContactBall e)
    {
        Vector3 direction = e.hitPosition - e.ball.transform.position;
        
        
    }

    public void Shoot()
    {
        Vector3 direction = transform.forward;
        m_currentSpeed = 2; 
    }

    private void HandleMoving()
    {
        Vector3 nextPosition = transform.position + transform.forward * (m_currentSpeed * Time.deltaTime);
        
        transform.position = nextPosition;

        if (m_currentSpeed <= 0.05f) m_currentSpeed = 0;

        if (m_currentSpeed > 0) m_currentSpeed -= Time.deltaTime * m_currentSpeed;
    }
}
