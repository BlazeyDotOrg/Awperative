using System.Collections.Generic;
using System.Reflection;


namespace AwperativeKernel;


public interface IModuleManager
{
    public IReadOnlyList<Assembly> GetDependencies();
}