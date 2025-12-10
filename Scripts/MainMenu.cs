using Godot;
using static Godot.GD;
using SuperBall.Scripts;

public partial class MainMenu : Control {
    public override void _Ready() {
        var save = Global.Instance.Save;
        foreach (var level in save.Levels) {
            Print(level);
        }
    }
}