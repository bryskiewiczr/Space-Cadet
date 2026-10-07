using Godot;
using System;

public partial class ShootingEnemy : CharacterBody2D {

    private HitBox _shootingEnemyHitbox;
    private Sprite2D _shootingEnemySprite;

    [Export] private bool _isFacingLeft = true;
    
    private Instancer _instancer;

    public override void _Ready() {
        _instancer = GetNode<Instancer>("/root/Instancer");
        
        _shootingEnemySprite = GetNode<Sprite2D>("Sprite2D");
        _shootingEnemyHitbox = GetNode<HitBox>("HitBox");
        
        _shootingEnemyHitbox.Died += OnHitboxDied;
        _shootingEnemyHitbox.TookDamage += (float damage) => OnHitboxTookDamage(damage);
        
        if (!_isFacingLeft) _shootingEnemySprite.FlipH = true;
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
