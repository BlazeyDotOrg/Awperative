using System;


namespace Awperative;


public sealed partial class Scene : DockerEntity
{
    
    public event EventHandler<BodyCreateEvent> BodyCreatedEvent;
    public event EventHandler<BodyDestroyEvent> BodyDestroyedEvent;
}