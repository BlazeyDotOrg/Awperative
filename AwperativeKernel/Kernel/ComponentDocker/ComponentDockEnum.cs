using System;
using System.Collections;
using System.Collections.Generic;


namespace AwperativeKernel;



/// <summary>
/// Provides an Enumerator for the Component Docker, Loops through all Components in the Docker all for your convenience :)
/// </summary>
/// <param name="__components">Reference to the Component Docker's components, (List to Enumerate through)</param>
/// <remarks> Enumerates on a snapshot of the list. This allows you to modify the ComponentDocker's collection during the loop. </remarks>
/// <author> Avery Norris </author>
public class ComponentDockEnum(Component[] __components) : IEnumerator, IEnumerator<Component>, IDisposable
{
    
    
    
    //Current index of the loop
    private int index = -1;
    
    
    
    /// <summary>
    /// Gets the current Component;
    /// </summary>
    public Component Current => __components[index];
    
    
    
    /// <summary>
    /// Get the current Component as a generic object
    /// </summary>
    object IEnumerator.Current => Current;


    
    /// <summary>
    /// Proceeds to the next index of the loop
    /// </summary>
    /// <returns> Returns true or false, depending on whether the Enumeration should continue</returns>
    bool IEnumerator.MoveNext() { ++index; return index < __components.Length; }

    
    
    /// <summary>
    /// Resets the loop back to the very start
    /// </summary>
    public void Reset() => index = -1;
    
    
    
    /// <summary>
    /// Destroys the enumerator
    /// </summary>
    public void Dispose() => GC.SuppressFinalize(this);
    
    
    
}