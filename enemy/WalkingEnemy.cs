using Godot;
using System;

public partial class WalkingEnemy : CharacterBody2D {
    
    public override void _PhysicsProcess(double delta) {
        // apply gravity
        if (!IsOnFloor()) {
            Velocity += GetGravity() * (float)delta;
        }
    }
}
