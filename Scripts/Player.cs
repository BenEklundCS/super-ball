using Godot;
using static Godot.GD;

namespace SuperBall.Scripts;

public partial class Player : RigidBody3D {
    [Signal] public delegate void CollisionEventHandler(Node3D body);
    
    private float _yaw;
    private bool _pendingRespawn = false;
    private Transform3D _respawnTransform;

    private Area3D _area;
    
    [Export] public float JumpForce = 12f;
    [Export] public float MaxSpeed = 50f;
    [Export] public float MoveAcceleration = 20f;
    [Export] public float RotationSpeedDegrees = 90f;

    [Export] public RayCast3D GroundRay; // assign in editor

    public override void _Ready() {
        _area = GetNode<Area3D>("Area3D");
        _area.BodyEntered += EmitSignalCollision;
    }

    public override void _IntegrateForces(PhysicsDirectBodyState3D state) {
        if (_pendingRespawn)
        {
            state.Transform = _respawnTransform;
            state.LinearVelocity = Vector3.Zero;
            state.AngularVelocity = Vector3.Zero;
            state.Sleeping = false;
            _pendingRespawn = false;
            return;
        }
        var dt = state.Step;
        ApplyRotation(state, dt);
        ApplyMovement(state, dt);
        ApplyGravity(state, dt);
        ClampSpeed(state);
    }

    public void OnRespawn(Transform3D respawnTransform) {
        _pendingRespawn = true;
        _respawnTransform = respawnTransform;
    }

    private void ApplyRotation(PhysicsDirectBodyState3D state, float dt) {
        var rotSpeedRad = Mathf.DegToRad(RotationSpeedDegrees);

        if (Input.IsActionPressed("left"))
            _yaw += rotSpeedRad * dt;
        if (Input.IsActionPressed("right"))
            _yaw -= rotSpeedRad * dt;

        var basis = Basis.FromEuler(new Vector3(0, _yaw, 0));
        state.Transform = new Transform3D(basis, state.Transform.Origin);
    }

    private void ApplyMovement(PhysicsDirectBodyState3D state, float dt) {
        Vector3 forward = -state.Transform.Basis.Z;
        forward.Y = 0f;
        forward = forward.Normalized();

        if (Input.IsActionPressed("forward"))
            state.ApplyCentralForce(forward * MoveAcceleration);
    }

    private void ApplyGravity(PhysicsDirectBodyState3D state, float dt) {
        if (Input.IsActionJustPressed("jump") && IsGrounded()) {
            state.ApplyCentralImpulse(Vector3.Up * JumpForce);
        }
    }

    private bool IsGrounded() {
        return GroundRay != null && GroundRay.IsColliding();
    }

    private void ClampSpeed(PhysicsDirectBodyState3D state) {
        var v = state.LinearVelocity;
        var h = new Vector3(v.X, 0, v.Z);

        if (h.Length() > MaxSpeed) {
            h = h.Normalized() * MaxSpeed;
            state.LinearVelocity = new Vector3(h.X, v.Y, h.Z);
        }
    }
}
