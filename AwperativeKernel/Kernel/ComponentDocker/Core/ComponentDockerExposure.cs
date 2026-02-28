

namespace AwperativeKernel;


public abstract partial class ComponentDocker
{
       
    
    /// <summary>
    /// Returns a bool based on if the Docker contains a Component with the given tag or not
    /// </summary>
    /// <param name="__tag"></param>
    /// <returns></returns>
    public bool Contains([NotNull] string __tag) => NotNull.VerifyOrThrow(__tag) && _taggedComponents.ContainsKey(__tag);


    /// <summary>
    /// Returns a bool for if the Docker contains a Component of that type or not
    /// </summary>
    /// <typeparam name="__Type">Type of the Component</typeparam>
    public bool Contains<__Type>() where __Type : Component => _ComponentDictionary.ContainsKey(typeof(__Type));
    
    
    
    /// <summary>
    /// Returns a bool based on if the current __Component is owned by this Docker
    /// </summary>
    /// <param name="__component"></param>
    /// <returns></returns>
    public bool Contains([ComponentNotNull,DockerOwns] Component __component) => NotNull.VerifyOrThrow(__component) && DockerOwns.VerifyOrThrow(this, __component) && _Components.Contains(__component);
}