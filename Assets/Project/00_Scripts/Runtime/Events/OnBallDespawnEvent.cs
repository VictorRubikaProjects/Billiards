using Event_Bus;
using UnityEngine;

public readonly struct OnBallDespawnEvent : IEvent
{
    public readonly Transform target;
    public readonly Ball ball;

    public OnBallDespawnEvent(Transform target,Ball ball)
    {
        this.target = target;
        this.ball = ball;
    }
}