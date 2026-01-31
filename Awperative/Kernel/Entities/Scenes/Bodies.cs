using System;
using System.Collections.Generic;


namespace Awperative;


public sealed partial class Scene
{
    public List<Body> bodies { get; private set; } = [];
    
    public Body AddBody(Transform __transform) {
        Body body = new Body(this, __transform);
        bodies.Add(body);
        body.Create();
        
        BodyCreatedEvent?.Invoke(this, new BodyCreateEvent(body, this));
        
        return body;
    }

    public Body AddBody() {
        Body body = new Body(this, new Transform());
        bodies.Add(body);
        body.Create();
        
        BodyCreatedEvent?.Invoke(this, new BodyCreateEvent(body, this));

        return body;
    }
    
    public Body[] GetBodies(string tag) {
        List<Body> _bodies = new List<Body>();
        
        foreach (Body body in bodies) 
            if (body.tags.Contains(tag))
                _bodies.Add(body);
        
        
        if(_bodies.Count == 0)
            throw new Exception("No Bodies found with the tag " + tag);
        
        return _bodies.ToArray();
    }

    public Body GetBody(string tag) {
        foreach (Body body in bodies)
            if (body.tags.Contains(tag))
                return body;
        
        throw new Exception("No Body found with the tag " + tag);
    }
    
    public void DestroyBody(Body __body) {
        __body.Destroy();
        BodyDestroyedEvent?.Invoke(this, new BodyDestroyEvent(__body, this));
        if (!bodies.Remove(__body))
            throw new Exception("Removal Failed! Does the Body Exist?");
    }
    
    //todo: add destroying and creating bodies with tags
    
    
    //TAG SYSTEM IN V4
    
}