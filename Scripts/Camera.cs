using Godot;

namespace SuperBall.Scripts;

public partial class Camera : Camera3D {
    private bool _attached = true;
    private bool _panning;
    private float _pitch = 0.25f;
    private bool _rotating;

    private float _yaw;
    [Export] public Vector3 AttachOffset = new(0, 5f, -10f);
    [Export] public Node3D AttachTarget;

    [Export] public float Distance = 10f;
    [Export] public float MaxDistance = 200f;
    [Export] public float MinDistance = 0.5f;
    [Export] public float PanSpeed = 0.01f;

    [Export] public float RotateSpeed = 0.01f;
    [Export] public Vector3 Target = Vector3.Zero;
    [Export] public float ZoomStep = 1.0f;

    public override void _Ready() {
        UpdateTransform();
    }

    public override void _UnhandledInput(InputEvent e) {
        if (e.IsActionPressed("camera_toggle_attach")) {
            _attached = !_attached;
            return;
        }

        if (_attached) return;

        if (e is InputEventMouseButton mb) {
            if (mb.ButtonIndex == MouseButton.Middle) _rotating = mb.Pressed;
            if (mb.ButtonIndex == MouseButton.Right) _panning = mb.Pressed;

            if (mb.Pressed && mb.ButtonIndex == MouseButton.WheelUp) {
                var step = ZoomStep * (Input.IsKeyPressed(Key.Shift) ? 5f : 1f);
                Distance = Mathf.Max(MinDistance, Distance - step);
                UpdateTransform();
            }

            if (mb.Pressed && mb.ButtonIndex == MouseButton.WheelDown) {
                var step = ZoomStep * (Input.IsKeyPressed(Key.Shift) ? 5f : 1f);
                Distance = Mathf.Min(MaxDistance, Distance + step);
                UpdateTransform();
            }
        }

        if (e is InputEventMouseMotion mm) {
            if (_rotating) {
                _yaw -= mm.Relative.X * RotateSpeed;
                _pitch -= mm.Relative.Y * RotateSpeed;
                _pitch = Mathf.Clamp(_pitch, -Mathf.Pi * 0.499f, Mathf.Pi * 0.499f);
                UpdateTransform();
            }
            else if (_panning) {
                var right = GlobalTransform.Basis.X.Normalized();
                var up = GlobalTransform.Basis.Y.Normalized();
                var scale = PanSpeed * Distance * 0.01f;

                Target -= right * mm.Relative.X * scale;
                Target += up * mm.Relative.Y * scale;
                UpdateTransform();
            }
        }
    }

    public override void _Process(double delta) {
        if (_attached && AttachTarget != null) {
            var ballXform = AttachTarget.GlobalTransform;
            var forward = -ballXform.Basis.Z; // FIX HERE

            _yaw = Mathf.Atan2(forward.X, forward.Z);

            var rotY = new Basis(Vector3.Up, _yaw);
            var worldOffset = rotY * AttachOffset;

            var pos = ballXform.Origin + worldOffset;

            GlobalTransform = new Transform3D(Basis.Identity, pos);
            LookAt(ballXform.Origin, Vector3.Up);
        }
    }

    private void UpdateTransform() {
        var rotY = new Basis(Vector3.Up, _yaw);
        var rotX = new Basis(Vector3.Right, _pitch);
        var basis = rotY * rotX;

        var offset = basis * new Vector3(0, 0, Distance);
        var pos = Target + offset;

        GlobalTransform = new Transform3D(Basis.Identity, pos);
        LookAt(Target, Vector3.Up);
    }
}