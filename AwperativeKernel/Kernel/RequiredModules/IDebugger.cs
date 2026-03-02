using System.Collections.Generic;



namespace AwperativeKernel;



[DependencyAttributes.RequiredModule(Source: "Debug")]
public interface IDebugger : IModule
{

    public void LogAction(string __message);
    public void LogAction(string __message, IEnumerable<string> __values, IEnumerable<string> __args);
    
    public void LogWarning(string __message);
    public void LogWarning(string __message, IEnumerable<string> __values, IEnumerable<string> __args);
    
    public void LogError(string __message);
    public void LogError(string __message, IEnumerable<string> __values, IEnumerable<string> __args);

    
    
    public bool ThrowExceptions { get; set; }
    public bool IgnoreErrors { get; set; }
    public bool DebugErrors { get; set; }
    
    
}