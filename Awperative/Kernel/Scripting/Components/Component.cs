using Microsoft.Xna.Framework;


namespace Gravity.Kernel;

public abstract class Component
{
    public Scene scene;
    public Body body;
    
    public bool Enabled = false;
    public bool EnforceSingleton = false;
    
    //0 = default  highest priority called first, lowest priority called last
    //todo: add optional parameter for priority at creation
    public int Priority {
        get => _priority; 
        set { _priority = value; body.RecompileComponentOrder(); }
    } private int _priority = 0;
    
    
    protected Transform transform => body.transform;

    protected Body AddBody() => scene.AddBody();
    protected Body AddBody(Transform __transform) => scene.AddBody(__transform);
    
    protected Body GetBody(string __tag) => scene.GetBody(__tag);
    protected Body[] GetBodies(string __tag) => scene.GetBodies(__tag);
    
    protected void DestroyBody(Body __body) => scene.DestroyBody(__body);

    
    
    public Generic AddBehavior<Generic>() where Generic : Behavior => scene.AddBehavior<Generic>();
    public Generic AddBehavior<Generic>(object[] __args) where Generic : Behavior => scene.AddBehavior<Generic>(__args);
    
    public Generic GetBehavior<Generic>() where Generic : Behavior => scene.GetBehavior<Generic>();
    public Generic[] GetBehaviors<Generic>() where Generic : Behavior => scene.GetBehaviors<Generic>();
    
    
    public void RemoveBehavior<Generic>() where Generic : Behavior => scene.RemoveBehavior<Generic>();
    
    
    
    public Generic AddComponent<Generic>() where Generic : Component => body.AddComponent<Generic>();
    public Generic AddComponent<Generic>(object[] __args) where Generic : Component => body.AddComponent<Generic>(__args);
    
    public Generic GetComponent<Generic>() where Generic : Component => body.GetComponent<Generic>();
    public Generic[] GetComponents<Generic>() where Generic : Component => body.GetComponents<Generic>();
    
    public void RemoveComponent<Generic>() where Generic : Component => body.RemoveComponent<Generic>();
    
    
    
    //GAME HAS JUST BEGUN/ended
    public virtual void Initialize() {}
    public virtual void Terminate() {}
    
    //WE ARE LOADING STUFF
    public virtual void Load() {}
    
    //You know what these do
    public virtual void Update(GameTime __gameTime) {}
    public virtual void Draw(GameTime __gameTime) {}
    
    //component/body/scene is being created or destroyed
    public virtual void Create() {}
    public virtual void Destroy() {}

    //creation logic
    internal void Initiate(Body __body)
    {
        body = __body;
        scene = __body.scene;
        Create();
    }

    internal void End()
    {
        Destroy();
    }
}