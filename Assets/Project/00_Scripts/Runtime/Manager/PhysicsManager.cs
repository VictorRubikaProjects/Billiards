using System.Collections.Generic;
using Event_Bus;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    [field: SerializeField] private BoundsData m_boundsData;
    [SerializeField] private float m_restitution = 0.8f;
    [SerializeField] private List<TriggerZone> m_triggerZones = new();
    private const float BALL_RADIUS = 0.05f;

    private Bounds m_gameplayBounds;
    private readonly List<Ball> m_balls = new();

    private EventBinding<OnBallSpawnEvent> m_onBallSpawnBinding;
    private EventBinding<OnBallDespawnEvent> m_onBallDespawnBinding;

    private void Awake()
    {
        m_gameplayBounds = new Bounds(m_boundsData.center, m_boundsData.size);

        m_onBallSpawnBinding = new EventBinding<OnBallSpawnEvent>(e => m_balls.Add(e.ball));
        m_onBallDespawnBinding = new EventBinding<OnBallDespawnEvent>(e => m_balls.Remove(e.ball));

        EventBus<OnBallSpawnEvent>.Register(m_onBallSpawnBinding);
        EventBus<OnBallDespawnEvent>.Register(m_onBallDespawnBinding);
    }

    private void OnDestroy()
    {
        EventBus<OnBallSpawnEvent>.Unregister(m_onBallSpawnBinding);
        EventBus<OnBallDespawnEvent>.Unregister(m_onBallDespawnBinding);
        m_gameplayBounds.Dispose();
    }

    private void Update()
    {
        m_gameplayBounds.Update();
        ResolveBallCollisions();

        foreach (TriggerZone zone in m_triggerZones)
            zone.Update(m_balls, BALL_RADIUS);
    }

    private void ResolveBallCollisions()
    {
        float minDistance = BALL_RADIUS * 2f;
        float minSqrDistance = minDistance * minDistance;

        for (int i = 0; i < m_balls.Count; i++)
        for (int j = i + 1; j < m_balls.Count; j++)
        {
            Ball a = m_balls[i];
            Ball b = m_balls[j];

            Vector3 delta = b.transform.position - a.transform.position;
            float sqrDistance = delta.sqrMagnitude;
            if (sqrDistance >= minSqrDistance) continue;

            float distance = Mathf.Sqrt(sqrDistance);
            Vector3 normal = distance > Mathf.Epsilon ? delta / distance : Vector3.right;

            float overlapDistance = (minDistance - distance)/2;
            
            a.transform.position -= overlapDistance * normal;
            b.transform.position += overlapDistance * normal;
            
            float dotA = Vector3.Dot(a.Velocity, normal);
            float dotB = Vector3.Dot(b.Velocity, -normal);
            
            if (dotA <= 0 && dotB <= 0) continue;

            Vector3 vn;

             if (dotA >= dotB)
             {
                 vn = dotA * normal;
                 ApplyImpact(a,b,vn);
             }
             else
             {
                 vn = dotB * -normal;
                 ApplyImpact(b,a,vn);
             }
        }
        
    }

    private void ApplyImpact(Ball impactor, Ball impacted, Vector3 vn)
    {
        Vector3 vt = impactor.Velocity - vn;
        impactor.Velocity = vt;
        impacted.Velocity += vn * m_restitution;
    }
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
        Gizmos.DrawWireCube(m_boundsData.center, m_boundsData.size);

        foreach (TriggerZone zone in m_triggerZones)
            zone.DrawGizmos();
    }
}