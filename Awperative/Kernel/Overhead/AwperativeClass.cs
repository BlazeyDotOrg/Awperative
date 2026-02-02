namespace Awperative;

/// <summary>
/// Awperative hooks are the source of entry for scripts using Awperative. Create a hook and send into Start() to be recognized by the engine.
/// </summary>
/// <author> Avery Norris </author>
public interface AwperativeHook
{
    /// <summary>
    /// Called when the program starts; It is not recommended you load assets here.
    /// </summary>
    public void Load() {}
    
    
    
    
    
    /// <summary>
    /// Called when the program closes. 
    /// </summary>
    public void Unload() {}
    
    
    
}