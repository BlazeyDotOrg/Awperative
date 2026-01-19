

using Microsoft.Xna.Framework;


namespace Gravity.Kernel;

public abstract class Behavior
{
    public Scene scene;
    
    public bool Enabled = false;
    public bool EnforceSingleton = false;

    public int Priority = 0;
    
    
    //scene relay
    protected Body AddBody() => scene.AddBody();
    protected Body AddBody(Transform __transform) => scene.AddBody(__transform);
    
    protected Body GetBody(string __tag) => scene.GetBody(__tag);
    protected Body[] GetBodies(string __tag) => scene.GetBodies(__tag);
    
    protected void DestroyBody(Body __body) => scene.DestroyBody(__body);

    
    
    protected Generic AddBehavior<Generic>() where Generic : Behavior => scene.AddBehavior<Generic>();
    protected Generic AddBehavior<Generic>(object[] __args) where Generic : Behavior => scene.AddBehavior<Generic>(__args);
    
    protected Generic GetBehavior<Generic>() where Generic : Behavior => scene.GetBehavior<Generic>();
    protected Generic[] GetBehaviors<Generic>() where Generic : Behavior => scene.GetBehaviors<Generic>();
    
    protected void RemoveBehavior<Generic>() where Generic : Behavior => scene.RemoveBehavior<Generic>();
    
    
    
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

    //New behavior functionality
    internal void Initiate(Scene __scene) {
        scene = __scene;
        Create();
    }

    //destroy behavior functionality
    internal void End()
    {
        Destroy();
    }
}