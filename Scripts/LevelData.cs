using Godot;

namespace SuperBall.Scripts;

public partial class LevelData : Resource
{
    [Export] public bool Completed { get; set; }
    [Export] public double CompletionTime { get; set; }
    
    public LevelData()
    {
        Completed = false;
        CompletionTime = 0.0;
    }

    public LevelData(bool completed = false, double completionTime = 0.0)
    {
        Completed = completed;
        CompletionTime = completionTime;
    }
}