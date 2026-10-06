using System;
using System.Collections.Generic;
using Event_Bus;
using UnityEngine;

[Serializable]
public class TriggerZone
{
    [SerializeField] private Vector3 m_center;
    [SerializeField] private Vector3 m_size = Vector3.one;

    private readonly HashSet<Ball> m_inside = new();

    public void Update(List<Ball> balls, float radius)
    {
        foreach (Ball ball in balls)
        {
            if (Overlaps(ball.transform.position, radius))
            {
                if (m_inside.Add(ball))
                    EventBus<OnTriggerZoneEnter>.Raise(new OnTriggerZoneEnter(ball, this));
            }
            else
            {
                m_inside.Remove(ball);
            }
        }
    }

    private bool Overlaps(Vector3 position, float radius)
    {
        Vector3 half = m_size * 0.5f;

        Vector3 closest = new Vector3(
            Mathf.Clamp(position.x, m_center.x - half.x, m_center.x + half.x),
            Mathf.Clamp(position.y, m_center.y - half.y, m_center.y + half.y),
            Mathf.Clamp(position.z, m_center.z - half.z, m_center.z + half.z));

        return (position - closest).sqrMagnitude <= radius * radius;
    }

    public void DrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.4f);
        Gizmos.DrawWireCube(m_center, m_size);
    }
}

