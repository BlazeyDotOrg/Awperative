using Microsoft.Xna.Framework;

namespace Awperative;

public abstract partial class Container
{
    internal virtual void ChainUnload() { foreach (Component component in (Component[])[.._behaviors]) component.Unload(); }
    internal virtual void ChainLoad() { foreach (Component component in (Component[])[.._behaviors]) { component.Load(); } }

    
    
    internal virtual void ChainUpdate() { foreach (Component component in (Component[])[.._behaviors]) { component.Update(); } }
    internal virtual void ChainDraw() { foreach (Component component in (Component[])[.._behaviors]) { component.Draw(); } }
    
    
    
    internal virtual void ChainDestroy() { foreach(Component component in (Component[])[.._behaviors]) component.Destroy(); }
    internal virtual void ChainCreate() { foreach (Component component in (Component[])[.._behaviors]) component.Create(); }
}