namespace AwperativeKernel;

public abstract partial class Component
{
    #region Scenes
    
    /// <inheritdoc cref="Awperative.CreateScene"/>
    public static Scene CreateScene(string __name) => Awperative.CreateScene(__name);



    /// <inheritdoc cref="Awperative.GetScene"/>
    public static Scene GetScene(string __name) => Awperative.GetScene(__name);



    /// <inheritdoc cref="Awperative.CloseScene(AwperativeKernel.Scene)"/>
    public void RemoveScene(Scene __scene) => Awperative.CloseScene(__scene);



    /// <inheritdock cref="Awperative.CloseScene(string)" />
    public void RemoveScene(string __name) => Awperative.CloseScene(__name);
    
    #endregion
    
    #region Components
    
    public void Move(ComponentDocker __newDocker) => ComponentDocker.Move(this, __newDocker);
    
    #endregion
}