using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;



namespace AwperativeKernel;


public interface IDebugger
{
    
    public void Start();
    public void Stop();

    public void LogAction(string __message);
    public void LogAction(string __message, IReadOnlyList<string> __values, IReadOnlyList<string> __args);
    
    public void LogWarning(string __message);
    public void LogWarning(string __message, IReadOnlyList<string> __values, IReadOnlyList<string> __args);
    
    public void LogError(string __message);
    public void LogError(string __message, IReadOnlyList<string> __values, IReadOnlyList<string> __args);

    
    
    public bool ThrowExceptions { get; set; }
    public bool IgnoreErrors { get; set; }
    public bool DebugErrors { get; set; }
    
    
}