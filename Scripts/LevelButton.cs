using System.Globalization;

namespace SuperBall.Scripts;

using Godot;
using System;

public partial class LevelButton : Control {
    [Export] public string LevelName;
    
    private LevelData _levelData;
    
    private Button _button;
    private Label _completionTime;
    private CheckBox _completed;

    public override void _Ready() {
        _button = GetNode<Button>("Button");
        _completed = GetNode<CheckBox>("Completed");
        _completionTime = GetNode<Label>("CompletionTime");
        
        _button.Text = LevelName;
        _button.Pressed += OnButtonPressed;
        
        if (Global.Instance.Save.Levels.ContainsKey(LevelName))
        {
            _levelData = (LevelData)Global.Instance.Save.Levels[LevelName];
            _completionTime.Text = Math.Round(_levelData.CompletionTime).ToString(CultureInfo.CurrentCulture);
            _completed.ButtonPressed = _levelData.Completed;
        }
        else {
            _completionTime.Text = "";
            _completed.ButtonPressed = false;
        }
    }

    private void OnButtonPressed() {
        var snakifiedLevelName = LevelName.ToSnakeCase();
        GetTree().ChangeSceneToFile($"res://Scenes/{snakifiedLevelName}.tscn");
    }
}
