using Godot;

public partial class ShootingEnemy : CharacterBody2D {

    private HitBox _shootingEnemyHitbox;
    private Sprite2D _shootingEnemySprite;
    private AnimationPlayer _animationPlayer;

    private Marker2D _leftShootingMarker;
    private Marker2D _rightShootingMarker;
    private Marker2D _muzzle;
    private Vector2 _fireballDirection = Vector2.Zero;
    
    private Timer _projectileCooldownTimer;

    [Export] private bool _isFacingLeft = true;
    
    private Instancer _instancer;

    public override void _Ready() {
        _instancer = GetNode<Instancer>("/root/Instancer");
        
        _shootingEnemySprite = GetNode<Sprite2D>("Sprite2D");
        _shootingEnemyHitbox = GetNode<HitBox>("HitBox");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        
        _leftShootingMarker = GetNode<Marker2D>("LeftShootingMarker");
        _rightShootingMarker = GetNode<Marker2D>("RightShootingMarker");
        _projectileCooldownTimer = GetNode<Timer>("ProjectileCooldownTimer");

        if (!_isFacingLeft) _shootingEnemySprite.FlipH = true;
        _muzzle = _isFacingLeft ? _leftShootingMarker : _rightShootingMarker;
        _fireballDirection = _isFacingLeft ? Vector2.Left : Vector2.Right;

        _projectileCooldownTimer.Timeout += OnShootCooldownExpired;
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

    public void ShootFireball() {
        var fireball = (Laser)_instancer.InstanceSceneToLevel(_instancer.FireballScene, _muzzle.GlobalPosition);
        fireball.Launch(_fireballDirection);
    }

    public void OnShootCooldownExpired() {
        _animationPlayer.Play("shoot");
    }
}
