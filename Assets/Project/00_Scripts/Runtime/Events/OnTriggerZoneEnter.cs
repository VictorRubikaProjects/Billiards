using Event_Bus;

public readonly struct OnTriggerZoneEnter : IEvent
{
    public readonly Ball ball;
    public readonly TriggerZone zone;

    public OnTriggerZoneEnter(Ball ball, TriggerZone zone)
    {
        this.ball = ball;
        this.zone = zone;
    }
}