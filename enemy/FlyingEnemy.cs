using Godot;

public partial class FlyingEnemy : CharacterBody2D {

    [Export] private float _speed = 20.0f;
    private Vector2 _direction = new Vector2(1.0f, 0.0f);
    private Vector2 _startPosition = Vector2.Zero;
    private Vector2 _endPosition = Vector2.Zero;
    private Vector2 _destination = Vector2.Zero;

    private Marker2D _endPositionMarker;
    private Sprite2D _flyingEnemySprite;
    
    private HitBox _flyingEnemyHitbox;

    private Instancer _instancer;
    
    public override void _Ready() {
        _endPositionMarker = GetNode<Marker2D>("EndPositionMarker");
        _flyingEnemySprite = GetNode<Sprite2D>("Sprite2D");
        _flyingEnemyHitbox = GetNode<HitBox>("HitBox");
        _instancer = GetNodeOrNull<Instancer>("/root/Instancer");

        _startPosition = GlobalPosition;
        _endPosition = _endPositionMarker.GlobalPosition;
        _destination = _endPosition;
        // _direction = (_destination - GlobalPosition).Normalized();
        
        _flyingEnemyHitbox.Died += OnHitboxDied;
        _flyingEnemyHitbox.TookDamage += (float damage) => OnHitboxTookDamage(damage);
    }

    public override void _PhysicsProcess(double delta) {
        Velocity = _direction * _speed;
        SetFlyingEnemyMovementDirection();
        MoveAndSlide();
    }

    public void SetFlyingEnemyMovementDirection() {
        _direction = (_destination - GlobalPosition).Normalized();
        var distanceToDestination = GlobalPosition.DistanceTo(_destination);
        if (distanceToDestination <= 1.0f) {
            if (_destination.IsEqualApprox(_endPosition)) {
                _destination = _startPosition;
                _flyingEnemySprite.FlipH = !_flyingEnemySprite.FlipH;
            } else {
                _destination = _endPosition;
                _flyingEnemySprite.FlipH = !_flyingEnemySprite.FlipH;
            }
        }
    }

    public void OnHitboxDied() {
        QueueFree();
        _instancer.InstanceSceneToLevel(_instancer.EnemyExplodeScene, GlobalPosition);
    }

    public void OnHitboxTookDamage(float damage) {
        //
    }
}

