using UnityEngine;

public class CueBall : Ball
{
    [SerializeField] private float m_minShootSpeed = 0.5f;
    [SerializeField] private float m_maxShootSpeed = 4f;
    [SerializeField] private float m_chargeDuration = 1.5f;
    [SerializeField] private float m_rotationSpeed = 60f;

    private Vector3 m_respawnPosition;
    private float m_chargeTime;

    public float ChargeRatio => Mathf.Clamp01(m_chargeTime / m_chargeDuration);

    public void SetRespawnPosition(Vector3 position) => m_respawnPosition = position;

    public void Rotate(bool right)
    {
        float direction = right ? 1f : -1f;
        transform.Rotate(Vector3.up, direction * m_rotationSpeed * Time.deltaTime);
    }

    public void Charge(float deltaTime) => m_chargeTime += deltaTime;

    public void Release()
    {
        Velocity = transform.forward * Mathf.Lerp(m_minShootSpeed, m_maxShootSpeed, ChargeRatio);
        m_chargeTime = 0f;
    }

    protected override void HandleZoneEntered()
    {
        transform.position = m_respawnPosition;
        Velocity = Vector3.zero;
        m_chargeTime = 0f;
    }
    
    public void CancelCharge() => m_chargeTime = 0f;
}