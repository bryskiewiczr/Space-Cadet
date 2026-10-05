using Godot;
using System;

public partial class FlyingEnemy : CharacterBody2D {

    [Export] private float _speed = 20.0f;
    private Vector2 _direction = new Vector2(1.0f, 0.0f);

    private HitBox _flyingEnemyHitbox;

    private Instancer _instancer;
    
    public override void _Ready() {
        _flyingEnemyHitbox = GetNode<HitBox>("HitBox");
        _instancer = GetNodeOrNull<Instancer>("/root/Instancer");

        _flyingEnemyHitbox.Died += OnHitboxDied;
        _flyingEnemyHitbox.TookDamage += (float damage) => OnHitboxTookDamage(damage);
    }

    public override void _PhysicsProcess(double delta) {
        Velocity = _direction * _speed;
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

