using Godot;
using System;
using System.Globalization;

public struct UiData(string currentLevel, double timeElapsed) {
    public string CurrentLevel = currentLevel;
    public double TimeElapsed = timeElapsed;
}

public partial class Ui : Control {
    private Label _currentLevel;
    private Label _timeElapsed;

    public override void _Ready() {
        _currentLevel = GetNode<Label>("CurrentLevel");
        _timeElapsed = GetNode<Label>("TimeElapsed");
    }

    public void UpdateUi(UiData data) {
        _currentLevel.Text = data.CurrentLevel;
        _timeElapsed.Text = data.TimeElapsed.ToString(CultureInfo.CurrentCulture);
    }
}
