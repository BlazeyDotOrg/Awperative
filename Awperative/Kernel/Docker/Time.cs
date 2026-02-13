using Microsoft.Xna.Framework;

namespace Awperative;

public abstract partial class Docker
{
    internal virtual void ChainUnload() { foreach (Behavior component in _behaviors) component.Unload(); }
    internal virtual void ChainLoad() { foreach (Behavior component in _behaviors) { component.Load(); } }

    
    
    internal virtual void ChainUpdate(GameTime __gameTime) { foreach (Behavior component in _behaviors) { component.Update(__gameTime); } }
    internal virtual void ChainDraw(GameTime __gameTime) { foreach (Behavior component in _behaviors) { component.Draw(__gameTime); } }
    
    
    
    internal virtual void ChainDestroy() { foreach(Behavior component in _behaviors) component.Destroy(); }
    internal virtual void ChainCreate() { foreach (Behavior component in _behaviors) component.Create(); }
}