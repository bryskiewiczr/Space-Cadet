using Godot;
using System;

public partial class ShootingEnemy : CharacterBody2D {

    private HitBox _shootingEnemyHitbox;
    
    private Instancer _instancer;

    public override void _Ready() {
        _instancer = GetNode<Instancer>("/root/Instancer");
        
        _shootingEnemyHitbox = GetNode<HitBox>("HitBox");
        
        _shootingEnemyHitbox.Died += OnHitboxDied;
        _shootingEnemyHitbox.TookDamage += (float damage) => OnHitboxTookDamage(damage);
    }
    
    public override void _PhysicsProcess(double delta) {
        // apply gravity
        if (!IsOnFloor()) {
            Velocity += GetGravity() * (float)delta;
        }
        MoveAndSlide();
    }
    
    public void OnHitboxDied() {
        QueueFree();
        _instancer.InstanceSceneToLevel(_instancer.EnemyExplodeScene, GlobalPosition);
    }

    public void OnHitboxTookDamage(float damage) {
        //
    }
}
