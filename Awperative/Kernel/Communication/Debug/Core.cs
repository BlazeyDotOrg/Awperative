using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Awperative.Kernel.Communication.Config;


namespace Awperative;


public static partial class Debugger
{

    
    
    
    
    /// <summary>
    /// True path of the log file Awperative dumps to.
    /// </summary>
    public static string LogFilePath { get; private set; }

    
    
    
    
    /// <summary>
    /// Sets up the Awperative debugger and finds the log file.
    /// </summary>
    internal static void Initiate() {
        string directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if(directoryPath == null) throw new Exception("Failed to get directory path!");
        
        if(!Directory.GetFiles(directoryPath).Contains(Config.logFileName + ".awlf")) throw new Exception("Failed to find log file!");
        LogFilePath = Path.Join(directoryPath, Config.logFileName + ".awlf");
    }
}