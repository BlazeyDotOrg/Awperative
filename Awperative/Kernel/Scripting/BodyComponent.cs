namespace Awperative;


public class BodyComponent : Component
{
    
    
    public Body Body;
    
    internal override void Initiate(DockerEntity __docker) {
        Docker = __docker;
        
        Body = (Body)__docker;
        Create();
    }
    
    public Transform Transform => Body.transform;
    
}