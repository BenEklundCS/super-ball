using Godot;
using Godot.Collections;

namespace SuperBall.Scripts;

[GlobalClass]
public partial class GameSave : Resource
{
    [Export] public Dictionary Levels { get; set; } = new ();
}