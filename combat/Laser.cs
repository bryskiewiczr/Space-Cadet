using Godot;

public partial class Laser : CharacterBody2D {

	private float _timeToLive = 3.0f;
	private float _speed = 100.0f;
	private Vector2 _direction = Vector2.Zero;

	private Sprite2D _laserSprite;
	private HurtBox _laserHurtBox;
	
	private Instancer _instancer;  // declare an AutoLoad

	public override void _Ready() {
		_laserSprite = GetNode<Sprite2D>("Sprite2D");
		_laserHurtBox = GetNode<HurtBox>("HurtBox");
		_laserHurtBox.DamageApplied += OnDamageApplied;		// when projectile hits something
		_instancer = GetNodeOrNull<Instancer>("/root/Instancer");
	}
	
	public override void _PhysicsProcess(double delta) {
		Velocity = _direction * _speed;
		if (MoveAndSlide()) { 
			// MoveAndSlide() returns true when detects collision
			OnDamageApplied();
		}
	}

	public async void Launch(Vector2 direction) {
		_direction = direction;
		
		// flip sprite based on projectile direction
		if (_direction.X < 0) 
			_laserSprite.FlipH = true;
		
		// remove projectile after 3 seconds of not hitting anything
		await ToSignal(
			GetTree().CreateTimer(_timeToLive),
			SceneTreeTimer.SignalName.Timeout
		);
		QueueFree();
	}

	private void OnDamageApplied() {
		_instancer.InstanceSceneToLevel(_instancer.ProjectileHitEffectScene, GlobalPosition);
		QueueFree();
	}
}
