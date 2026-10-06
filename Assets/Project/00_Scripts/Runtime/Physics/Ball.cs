using Event_Bus;
using UnityEngine;

public abstract class Ball : MonoBehaviour
{
    public Vector3 Velocity
    {
        get => transform.forward * m_currentSpeed;
        set
        {
            m_currentSpeed = value.magnitude;
            if (m_currentSpeed > 0.0001f)
                transform.rotation = Quaternion.LookRotation(value / m_currentSpeed, Vector3.up);
        }
    }

    private EventBinding<OnContactBall> m_onContactBinding;
    private EventBinding<OnTriggerZoneEnter> m_onTriggerBinding;

    private float m_currentSpeed;

    protected virtual void Start()
    {
        EventBus<OnBallSpawnEvent>.Raise(new OnBallSpawnEvent(transform, this));

        m_onContactBinding = new EventBinding<OnContactBall>(OnContactWall);
        m_onTriggerBinding = new EventBinding<OnTriggerZoneEnter>(OnZoneEnter);

        EventBus<OnContactBall>.Register(m_onContactBinding);
        EventBus<OnTriggerZoneEnter>.Register(m_onTriggerBinding);
    }

    private void Update() => HandleMoving();

    protected virtual void OnDestroy()
    {
        EventBus<OnBallDespawnEvent>.Raise(new OnBallDespawnEvent(transform, this));

        EventBus<OnContactBall>.Unregister(m_onContactBinding);
        EventBus<OnTriggerZoneEnter>.Unregister(m_onTriggerBinding);
    }

    protected abstract void HandleZoneEntered();

    private void OnZoneEnter(OnTriggerZoneEnter e)
    {
        if (e.ball != this) return;
        HandleZoneEntered();
    }

    private void OnContactWall(OnContactBall e)
    {
        if (e.ball != this) return;

        Vector3 reflected = Vector3.Reflect(transform.forward, e.normal);
        transform.rotation = Quaternion.LookRotation(reflected, Vector3.up);
        m_currentSpeed *= 0.8f;
    }

    private void HandleMoving()
    {
        transform.position += transform.forward * (m_currentSpeed * Time.deltaTime);

        if (m_currentSpeed <= 0.05f) m_currentSpeed = 0;

        if (m_currentSpeed > 0) m_currentSpeed -= Time.deltaTime * m_currentSpeed;
    }
}