using System;


namespace AwperativeKernel;


internal static class DependencyAttributes
{
    
    
    
    /// <summary> Shows the source for a given module interface </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Property)]
    public class RequiredModule : Attribute
    {
        /// <summary> Where to assign in the Awperative class.</summary>
        public string Source { get; set; }
        
        public RequiredModule() {}

        public RequiredModule(string Source) {
            this.Source = Source;
        }
    }
    
    
    
}