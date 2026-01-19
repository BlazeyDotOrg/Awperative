using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Gravity.Kernel;

//todo:  make static
public class Base : Game
{
    public static GraphicsDeviceManager GraphicsDeviceManager;
    
    public static ContentManager ContentManager { get; private set; }
    public static SpriteBatch SpritesBatch;

    public static List<Scene> LoadedScenes { get; private set; } = [];

    public static Scene MainScene { get; private set; }
    
    public Base()
    {
        
        //todo: move this asshole to camera
        GraphicsDeviceManager = new GraphicsDeviceManager(this);
        GraphicsDeviceManager.PreferredBackBufferWidth = 1920;
        GraphicsDeviceManager.PreferredBackBufferHeight = 1080;
        GraphicsDeviceManager.IsFullScreen = true;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        
        
        ContentManager = Content;
        SpritesBatch = new SpriteBatch(GraphicsDevice);
        
        MainScene = new Scene();
        LoadedScenes.Add(MainScene);
        
        //todo: generalize initialization, load a json file containing scripts to run and try running them
        //intptr.size
        //Marshal.Sizeof<Class>
        
        foreach (AwperativeHook hook in Core.ScriptingHooks) 
            hook.Initialize();
        
        // TODO: Add your initialization logic here
        foreach(Scene scene in LoadedScenes)
            scene.Initialize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        foreach (AwperativeHook hook in Core.ScriptingHooks) 
            hook.Load();
        
        foreach(Scene scene in LoadedScenes)
            scene.Load();
    }
    
    protected override void Update(GameTime gameTime)
    {
        
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        //TODO: add specific error codes so i know when json went wrong
        foreach(Scene scene in LoadedScenes)
            scene.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        //collider.Center += Vector2.One;
        
        
        
        
        //ADD MOVING COLLIDERS
        
        foreach(Scene scene in LoadedScenes)
            scene.Draw(gameTime);
        base.Draw(gameTime);
    }

    protected override void EndRun()
    {
        foreach (AwperativeHook hook in Core.ScriptingHooks)
            hook.Terminate();
        
        foreach (Scene scene in LoadedScenes)
            scene.Terminate();
    }
}