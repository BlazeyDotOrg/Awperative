using System;
using System.Collections.Generic;


namespace Gravity.Kernel;


public sealed partial class Scene
{
    
    public List<Behavior> behaviors { get; private set; } = [];

    //todo: use extern keyword to make transform ambiguous to support potential 3D games

    
    public Generic AddBehavior<Generic>(object[] args) where Generic : Behavior {

        if (SingletonExists<Generic>())
            throw new Exception("Cannot add behavior when singleton exists!");
        
        Generic behavior = (Generic) Activator.CreateInstance(typeof(Generic), args);
        
        if(behavior == null)
            throw new Exception("Failed to create behavior!");
        
        behaviors.Add(behavior);
        behavior.Initiate(this);
        
        BehaviorCreatedEvent?.Invoke(this, new BehaviorCreateEvent(behavior, this));
        
        return behavior;
    }
    
    public Generic AddBehavior<Generic>() where Generic : Behavior {
        
        if (SingletonExists<Generic>())
            throw new Exception("Cannot add behavior when singleton exists!");
        
        Generic behavior = (Generic) Activator.CreateInstance(typeof(Generic));
        
        if(behavior == null)
            throw new Exception("Failed to create behavior!");
        
        behaviors.Add(behavior);
        behavior.Initiate(this);
        
        BehaviorCreatedEvent?.Invoke(this, new BehaviorCreateEvent(behavior, this));
        
        return behavior;
    }
    
    public Generic[] GetBehaviors<Generic>() where Generic : Behavior {
        
        List<Behavior> foundBehaviors = behaviors.FindAll(x => x.GetType() == typeof(Generic));
        
        if(foundBehaviors.Count == 0)
            throw new Exception("Scene has no behaviors of that type!");
        
        return foundBehaviors.ToArray() as Generic[];
    }
    
    public Generic GetBehavior<Generic>() where Generic : Behavior {
        
        Behavior foundBehavior = behaviors.Find(x => x.GetType() == typeof(Generic));
        
        if(foundBehavior == null)
            throw new Exception("Scene has no behaviors of that type!");
        
        return foundBehavior as Generic;
    }

    public void RemoveBehaviors<Generic>() where Generic : Behavior {
        
        Behavior[] foundBehaviors = GetBehaviors<Generic>();
        
        if(foundBehaviors.Length == 0)
            throw new Exception("Scene has no behaviors of that type!");

        foreach (Behavior behavior in foundBehaviors) {
            behavior.End();
            behaviors.Remove(behavior);
            BehaviorDestroyedEvent?.Invoke(this, new BehaviorDestroyEvent(behavior, this));
        }
    }
    
    public void RemoveBehavior<Generic>() where Generic : Behavior {
        
        Behavior foundBehavior = GetBehavior<Generic>();
        
        if(foundBehavior == null)
            throw new Exception("Scene has no behaviors of that type!");
        
        foundBehavior.End();
        behaviors.Remove(foundBehavior);
        BehaviorDestroyedEvent?.Invoke(this ,new BehaviorDestroyEvent(foundBehavior, this));
    }

    public void RemoveBehavior(Behavior __behavior) {
        __behavior.End();
        behaviors.Remove(__behavior);
        BehaviorDestroyedEvent?.Invoke(this, new BehaviorDestroyEvent(__behavior, this));
    }

    public Generic FindSingleton<Generic>() where Generic : Behavior
    {
        foreach (Behavior __behavior in behaviors)
            if (__behavior.GetType() == typeof(Generic))
                if(__behavior.EnforceSingleton)
                    return (Generic) __behavior;
                else
                    throw new Exception("Behavior is not a singleton");
        
        throw new Exception("Behavior not found");
        return null;
    }

    public bool SingletonExists<Generic>() where Generic : Behavior
    {
        
        foreach (Behavior __behavior in behaviors)
            if (__behavior.GetType() == typeof(Generic))
                if (__behavior.EnforceSingleton)
                    return true;
                else
                    return false;
        
        return false;
    }
    
    public void RecompileBehaviorOrder() {
        behaviors.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        behaviors.Reverse();
    }
}