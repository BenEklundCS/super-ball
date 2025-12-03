using Godot;

public partial class MainMenu : Control {
    private Button _start;

    public override void _Ready() {
        _start = GetNode<Button>("Start");
        _start.Pressed += OnStartPressed;
    }

    private void OnStartPressed() {
        GetTree().ChangeSceneToFile("res://Scenes/level_one.tscn");
    }
}