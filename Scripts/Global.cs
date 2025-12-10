using Godot;

namespace SuperBall.Scripts;

public partial class Global : Node {
    public static Global Instance { get; private set; }
    
    public GameSave Save;
    public override void _Ready() {
        Load();
        Instance = this;
    }

    private void Load() {
        var path = "user://game_save.res";
        if (FileAccess.FileExists(path)) {
            var loaded = ResourceLoader.Singleton.Load(path);
            Save = loaded as GameSave;
            if (Save == null) {
                Save = new GameSave();
                ResourceSaver.Singleton.Save(Save, path);
            }
        } else {
            Save = new GameSave();
            ResourceSaver.Singleton.Save(Save, path);
        }
    }
}