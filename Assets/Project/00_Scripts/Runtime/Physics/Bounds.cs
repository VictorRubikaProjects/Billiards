using System;
using System.Collections.Generic;
using Event_Bus;
using UnityEngine;

public class Bounds : IDisposable
{
    private List<Transform> boundariesTargets = new();
    
    private BoundsData boundsData;
    
    private EventBinding<OnBallSpawnEvent> m_onBallSpawnBinding;
    private EventBinding<OnBallDespawnEvent> m_onBallDespawnBinding;

    public Bounds(Vector3 center, Vector3 size)
    {
        boundsData = new BoundsData
        {
            center = center,
            size = size
        };

        m_onBallSpawnBinding = new EventBinding<OnBallSpawnEvent>(RegisterTarget);
        m_onBallDespawnBinding = new EventBinding<OnBallDespawnEvent>(UnregisterTarget);
        
        EventBus<OnBallSpawnEvent>.Register(m_onBallSpawnBinding);
        EventBus<OnBallDespawnEvent>.Register(m_onBallDespawnBinding);
    }

    public void Dispose()
    {
        EventBus<OnBallSpawnEvent>.Unregister(m_onBallSpawnBinding);
        EventBus<OnBallDespawnEvent>.Unregister(m_onBallDespawnBinding);
        
    }

    public void Update() => ApplyBoundaries();
    
    private void RegisterTarget(OnBallSpawnEvent e) => boundariesTargets.Add(e.target);
    private void UnregisterTarget(OnBallDespawnEvent e) => boundariesTargets.Remove(e.target);

    private void ApplyBoundaries()
    {
        foreach (Transform target in boundariesTargets)
        {
            float x = target.position.x;
            float y = target.position.y;
            float z = target.position.z;
            
            Vector3 oldPosition = target.position;
            
            x = CalculateBoundaries(x, boundsData.center.x, boundsData.size.x);
            y = CalculateBoundaries(y, boundsData.center.y, boundsData.size.y);
            z = CalculateBoundaries(z, boundsData.center.z, boundsData.size.z);
            
            Vector3 finalPosition = new Vector3(x, y, z);

            if (oldPosition != finalPosition && target.TryGetComponent(out Ball ball)) 
            {
                EventBus<OnContactBall>.Raise(new OnContactBall(ball,finalPosition));
            }
            
            target.position = finalPosition;
        }
    }

    private float CalculateBoundaries(float posAxes,float centerAxes,float sizeAxes) => 
        Mathf.Clamp(posAxes, centerAxes - sizeAxes/2, centerAxes + sizeAxes/2);
    
    //private Vector3 GetNormals()
    
}