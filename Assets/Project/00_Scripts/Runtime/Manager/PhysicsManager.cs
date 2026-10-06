using System;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    [field: SerializeField] private BoundsData m_boundsData;
    
    private Bounds m_gameplayBounds;

    private void Awake()
    {
        m_gameplayBounds = new Bounds(m_boundsData.center, m_boundsData.size);
    }

    private void Update() 
    {
        m_gameplayBounds.Update();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
        Gizmos.DrawWireCube(m_boundsData.center, m_boundsData.size);
    }
}
