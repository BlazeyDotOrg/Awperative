namespace Gravity.Kernel;

public interface AwperativeHook
{
    //DONT LOAD ASSETS HERE
    public void Initialize() {}
    public void Terminate() {}

    public void Load() {}
}