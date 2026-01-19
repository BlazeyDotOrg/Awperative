namespace Gravity.Kernel;

public sealed record BehaviorCreateEvent
{
    public readonly Behavior behavior;
    public readonly Scene scene;

    internal BehaviorCreateEvent() {}

    internal BehaviorCreateEvent(Behavior __behavior, Scene __scene)
    {
        behavior = __behavior;
        scene = __scene;
    }
}