using Event_Bus;
using UnityEngine;

public readonly struct OnContactBall : IEvent
{
    public readonly Vector3 hitPosition;
    public readonly Ball ball;

    public OnContactBall(Ball ball,Vector3 hitPosition)
    {
        this.ball = ball;
        this.hitPosition = hitPosition;
    }
    
}