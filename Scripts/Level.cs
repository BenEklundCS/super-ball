using Godot;

namespace SuperMonkeyBall.Scripts;

using static GD;

public partial class Level : Node3D {
    private int[] _expectedCollisionLayers = [1, 2];
    
    private Camera _camera;
    private Area3D _end;
    private Player _player;
    private Node3D _start;
    private Node3D _staticBodies;
    private Ui _ui;
    private double _elapsedTime = 0.0f;
    private bool _timerRunning = true;
    
    [Export] public Vector2 LevelYBoundaries = new(-50.0f, 50.0f);

    public override void _Ready() {
        _start = GetNode<Node3D>("Start");
        _end = GetNode<Area3D>("End");
        _end.BodyEntered += OnBodyEnteredEnd;
        _camera = GetNode<Camera>("Camera");
        _staticBodies = GetNode<Node3D>("StaticBodies");
        _ui = GetNode<Ui>("UI");
        Callable.From(SpawnPlayer).CallDeferred();
    }

    public override void _Process(double delta) {
        if (_timerRunning) {
            _elapsedTime += delta;
        }
        if (DetectFall()) ResetPlayer();
        var uiData = new UiData(Name, _elapsedTime);
        _ui.UpdateUi(uiData);
    }

    private void SetupCollisions() {
        foreach (var body in _staticBodies.GetChildren()) {
            if (body is StaticBody3D staticBody) {
                foreach (var layer in _expectedCollisionLayers) {
                    staticBody.SetCollisionLayerValue(layer, true);
                }
            }
        }
    }

    private void SpawnPlayer() {
        var player = (Player)Load<PackedScene>("res://Scenes/player.tscn").Instantiate();
        AddChild(player);
        _player = player;
        _player.Collision += OnPlayerCollision;
        ResetPlayer();
    }

    private void ResetPlayer() {
        _player.OnRespawn(_start.GetTransform());
        _camera.AttachTarget = _player;
        LevelYBoundaries = new (-50.0f, 50.0f);
    }

    private bool DetectFall() {
        return _player.GlobalPosition.Y <= LevelYBoundaries.X;
    }

    private void OnBodyEnteredEnd(Node3D body) {
        if (body is Player) {
            Print("You win!");
            _timerRunning = false;
        }
    }

    private void OnPlayerCollision(Node3D body) {
        if (body is StaticBody3D) {
            var mesh = body.GetNodeOrNull<MeshInstance3D>("MeshInstance3D");
            if (mesh != null) {
                LevelYBoundaries = new (mesh.GlobalPosition.Y - 50.0f, 50.0f);
            }
        }
        Print(LevelYBoundaries);
    }
}