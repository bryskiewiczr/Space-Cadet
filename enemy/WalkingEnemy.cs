using Godot;
using System;

public partial class WalkingEnemy : CharacterBody2D {

    private float _speed = 30.0f;
    private float _direction = 1.0f;

    private Sprite2D _walkingEnemySprite;
    private bool _isSpriteFlipped;

    private RayCast2D _leftRayDown;
    private RayCast2D _rightRayDown;
    private RayCast2D _leftRayWall;
    private RayCast2D _rightRayWall;

    public override void _Ready() {
        _walkingEnemySprite = GetNode<Sprite2D>("Sprite2D");
        _leftRayDown = GetNode<RayCast2D>("LeftRayDown");
        _rightRayDown = GetNode<RayCast2D>("RightRayDown");
        _leftRayWall = GetNode<RayCast2D>("LeftRayWall");
        _rightRayWall = GetNode<RayCast2D>("RightRayWall");
    }
    
    public override void _PhysicsProcess(double delta) {
        // raycasting
        // on platforms
        if (!_rightRayDown.IsColliding() || !_leftRayDown.IsColliding()) _direction *= -1;
        // against walls
        if (_rightRayWall.IsColliding() || _leftRayWall.IsColliding()) _direction *= -1;
        
        // apply gravity
        if (!IsOnFloor()) {
            Velocity += GetGravity() * (float)delta;
        }

        // apply movement speed
        Velocity = Velocity with { X = _direction * _speed };

        MoveAndSlide();

        // flip the sprite based on movement direction
        _isSpriteFlipped = _direction < 0 ? _walkingEnemySprite.FlipH = false : _walkingEnemySprite.FlipH = true;
    }
}
