using Event_Bus;
using UnityEngine;

public readonly struct OnBallSpawnEvent : IEvent
{
    public readonly Transform target;
    public readonly Ball ball;

    public OnBallSpawnEvent(Transform target, Ball ball)
    {
        this.target = target;
        this.ball = ball;
    }
}