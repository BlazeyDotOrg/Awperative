using Microsoft.Xna.Framework;


namespace Awperative;


public sealed partial class Body
{
    public void Initialize() { foreach (Component component in components) component.Initialize(); }
    public void Terminate() { foreach (Component component in components) component.Terminate(); }
    
    public void Load() { foreach (Component component in components) { component.Load(); } }

    public void Update(GameTime __gameTime) { foreach (Component component in components) { component.Update(__gameTime); } }
    public void Draw(GameTime __gameTime) { foreach (Component component in components) { component.Draw(__gameTime); } }
    
    public void Destroy() { foreach(Component component in components) component.Destroy(); }
    public void Create() { foreach (Component component in components) component.Create(); }
}