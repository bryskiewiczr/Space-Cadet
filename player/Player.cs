using Godot;

public partial class Player : CharacterBody2D {
    
    private float _jumpSpeed = 300.0f;
    private float _moveSpeed = 100.0f;
    private float _slideStopDelta = 10.0f;
    
    private bool _isMoving;
    private bool _isFacingLeft;

    private AnimationPlayer _playerAnimationPlayer;
    private Sprite2D _playerSprite;

    public override void _Ready() {
        _playerAnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _playerSprite = GetNode<Sprite2D>("Sprite2D");
    }
    
    public override void _PhysicsProcess(double delta) {
        
        // get axis
        var direction = Input.GetAxis("move_left", "move_right");
        _isMoving = direction is > 0.0f or < 0;
        
        // flip the sprite
        if (_isMoving) {
            // we only want to flip the direction when movement command is issued
            _isFacingLeft = direction < 0.0f;
            _playerSprite.FlipH = _isFacingLeft;
        }
        
        // apply gravity
        if (!IsOnFloor()) {
            Velocity += GetGravity() * (float)delta;
        }

        // jumping
        if (Input.IsActionJustPressed("jump")) {
            if (IsOnFloor()) {
                Velocity += Vector2.Up * _jumpSpeed;
            }
        }
        
        // movement
        if (_isMoving) {  
            // horizontal movement
            Velocity = Velocity with { X = direction * _moveSpeed };
        } else {  
            // sliding stopping
            Velocity = Velocity with { X = (float)Mathf.MoveToward(Velocity.X, 0.0, _slideStopDelta) };
        }
        
        // handle animations
        if (IsOnFloor()) {
            if (_isMoving) {
                _playerAnimationPlayer.Play("run");
            } else {
                _playerAnimationPlayer.Play("idle");
            }
        } else {
            if (Velocity.Y >= 0.0) {
                _playerAnimationPlayer.Play("jump");
            } else {
                _playerAnimationPlayer.Play("fall");
            }
        }

        MoveAndSlide();
    }
}
