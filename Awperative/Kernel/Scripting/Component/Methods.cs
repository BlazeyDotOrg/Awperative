namespace Awperative;

public abstract partial class Component
{
    
    protected Body AddBody() => Scene.AddBody();
    protected Body AddBody(Transform __transform) => Scene.AddBody(__transform);
    
    protected Body GetBody(string __tag) => Scene.GetBody(__tag);
    protected Body[] GetBodies(string __tag) => Scene.GetBodies(__tag);
    
    protected void DestroyBody(Body __body) => Scene.DestroyBody(__body);
    
    
    
    
    
    public Component AddComponent<Generic>() where Generic : Component => Docker.AddComponent<Generic>();
    public Component AddComponent<Generic>(object[] __args) where Generic : Component => Docker.AddComponent<Generic>(__args);
    
    public Component GetComponent<Generic>() where Generic : Component => Docker.GetComponent<Generic>();
    public Component[] GetComponents<Generic>() where Generic : Component => Docker.GetComponents<Generic>();
    
    public void RemoveComponent<Generic>() where Generic : Component => Docker.RemoveComponent<Generic>();
    
}