namespace AwperativeKernel;

public sealed partial class Scene : ComponentDocker
{
    
    /// <summary>
    /// Whether the scene is enabled or not.
    /// </summary>
    public bool Enabled = true;



    /// <summary>
    /// Unique Name of the Scene
    /// </summary>
    public string Name;


    
    internal Scene() {}
    
    
    
    internal Scene(string __name) { Name = __name; }
}
