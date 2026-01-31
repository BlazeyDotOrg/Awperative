namespace Awperative;

public sealed record BehaviorDestroyEvent
{
    public readonly Behavior behavior;
    public readonly Scene scene;

    internal BehaviorDestroyEvent() {}

    internal BehaviorDestroyEvent(Behavior __behavior, Scene __scene)
    {
        behavior = __behavior;
        scene = __scene;
    }
}