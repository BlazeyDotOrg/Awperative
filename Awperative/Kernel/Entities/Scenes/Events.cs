using System;


namespace Gravity.Kernel;


public sealed partial class Scene
{
    public event EventHandler<BehaviorCreateEvent> BehaviorCreatedEvent;
    public event EventHandler<BehaviorDestroyEvent> BehaviorDestroyedEvent;
    
    
    public event EventHandler<BodyCreateEvent> BodyCreatedEvent;
    public event EventHandler<BodyDestroyEvent> BodyDestroyedEvent;
}