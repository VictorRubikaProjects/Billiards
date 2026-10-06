using Event_Bus;
using UnityEngine;

public readonly struct OnContactBall : IEvent
{
    public readonly Ball ball;
    public readonly Vector3 position;
    public readonly Vector3 normal;

    public OnContactBall(Ball ball, Vector3 position, Vector3 normal)
    {
        this.ball = ball;
        this.position = position;
        this.normal = normal;
    }
}