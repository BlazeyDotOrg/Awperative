using Microsoft.Xna.Framework;

namespace Awperative;

public abstract partial class Docker
{
    internal virtual void ChainUnload() { foreach (Behavior component in _behaviors) component.Unload(); }
    internal virtual void ChainLoad() { foreach (Behavior component in _behaviors) { component.Load(); } }

    
    
    internal virtual void ChainUpdate() { foreach (Behavior component in _behaviors) { component.Update(); } }
    internal virtual void ChainDraw() { foreach (Behavior component in _behaviors) { component.Draw(); } }
    
    
    
    internal virtual void ChainDestroy() { foreach(Behavior component in _behaviors) component.Destroy(); }
    internal virtual void ChainCreate() { foreach (Behavior component in _behaviors) component.Create(); }
}