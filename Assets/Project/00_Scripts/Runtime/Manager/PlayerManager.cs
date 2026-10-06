using System;
using System.Collections.Generic;
using Event_Bus;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [field: SerializeField] private List<Ball> m_balls =  new();
    
    private EventBinding<OnBallSpawnEvent> m_onBallSpawnBinding;
    private EventBinding<OnBallDespawnEvent> m_onBallDespawnBinding;
    
    void RegisterBall(OnBallSpawnEvent e) => m_balls.Add(e.ball);
    void UnregisterBall(OnBallDespawnEvent e) => m_balls.Remove(e.ball);
    
    [field: SerializeField] private Ball m_currentBall;
    
    private int m_currentBallIndex = -1;

    private void Awake()
    {
        m_onBallSpawnBinding = new EventBinding<OnBallSpawnEvent>(RegisterBall);
        m_onBallDespawnBinding = new EventBinding<OnBallDespawnEvent>(UnregisterBall);
        
        EventBus<OnBallSpawnEvent>.Register(m_onBallSpawnBinding);
        EventBus<OnBallDespawnEvent>.Register(m_onBallDespawnBinding);
    }

    private void Update()
    {
        HandleInput();
    }

    private void OnDestroy()
    {
        EventBus<OnBallSpawnEvent>.Unregister(m_onBallSpawnBinding);
        EventBus<OnBallDespawnEvent>.Unregister(m_onBallDespawnBinding);
    }

    private void HandleInput()
    {
        HandleSelectionBall();

        HandleRotationBall();

        HandleShooting();
    }

    private void HandleShooting()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            m_currentBall?.Shoot();
        }
    }

    private void HandleRotationBall()
    {
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            m_currentBall?.Rotate(false);
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            m_currentBall?.Rotate(true);
        }
    }

    private void HandleSelectionBall()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) return;
        
        if (m_balls.Count <= 0) return;
            
        m_currentBallIndex++;

        m_currentBallIndex %= m_balls.Count;

        m_currentBall = m_balls[m_currentBallIndex];
    }
}
