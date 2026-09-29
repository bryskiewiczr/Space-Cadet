using System.Numerics;
using Godot;
using Vector2 = Godot.Vector2;

public partial class Player : CharacterBody2D {
	
	private float _jumpSpeed = 300.0f;
	private float _moveSpeed = 100.0f;
	private float _slideStopDelta = 10.0f;
	private float _invincibilityDuration = 3.0f;
	
	private bool _isMoving;
	private bool _isFacingLeft;
	private bool _canDoubleJump = true;
	private bool _isShooting;
	private bool _canShoot = true;

	private AnimationPlayer _playerAnimationPlayer;
	private AnimationPlayer _playerInvincibleAnimationPlayer;
	private Sprite2D _playerSprite;
	public HitBox PlayerHitbox;
	private Marker2D _shootLeftMarker;
	private Marker2D _shootRightMarker;
	private Timer _shootCooldownTimer;
	
	public RemoteTransform2D PlayerRemoteTransform2D;

	[Signal]
	public delegate void HealthChangedEventHandler(float maxHp, float hpLeft);
	
	private Instancer _instancer;  // declare an AutoLoad

	public override void _Ready() {
		_playerAnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_playerInvincibleAnimationPlayer = GetNode<AnimationPlayer>("InvincibilityAnimationPlayer");
		_playerSprite = GetNode<Sprite2D>("Sprite2D");
		PlayerHitbox = GetNode<HitBox>("HitBox");
		_shootLeftMarker = GetNode<Marker2D>("ShootLeftMarker");
		_shootRightMarker = GetNode<Marker2D>("ShootRightMarker");
		_shootCooldownTimer = GetNode<Timer>("ShootCooldown");
		PlayerRemoteTransform2D = GetNode<RemoteTransform2D>("RemoteTransform2D");
		
		_instancer = GetNodeOrNull<Instancer>("/root/Instancer");  // access AutoLoad

		_shootCooldownTimer.Timeout += OnShootCooldownTimeout;
		PlayerHitbox.Died += () => OnHitboxDied();
		PlayerHitbox.TookDamage += (float damage) => OnHitboxTookDamage(damage);
		PlayerHitbox.InvincibilityEnded += OnInvincibilityEnded;
	}

	public override void _Process(double delta) {
		if (Input.IsActionJustPressed("shoot")) {
			if (!_isShooting && IsOnFloor() && _canShoot) {
				_isShooting = true;
				_playerAnimationPlayer.Play("shoot");
			}
		}
	}
	
	public override void _PhysicsProcess(double delta) {
		// get axis
		var direction = Input.GetAxis("move_left", "move_right");

		if (_isShooting) {
			direction = 0.0f;
		}
		
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
				Jump();
			} else if (_canDoubleJump) {
				Jump();
				_canDoubleJump = false;
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
		if (!_isShooting) {
			if (IsOnFloor()) {
				_playerAnimationPlayer.Play(_isMoving ? "run" : "idle");
			} else {
				_playerAnimationPlayer.Play(Velocity.Y >= 0.0 ? "jump" : "fall");
			}
		}

		var wasOnFloor = IsOnFloor();
		
		MoveAndSlide();
		
		if (!wasOnFloor) {
			if (IsOnFloor()) {
				_instancer.InstanceSceneToLevel(_instancer.DustEffectScene, GlobalPosition);
			}
		}
		
		if (IsOnFloor() && !_canDoubleJump) {
			_canDoubleJump = true;
		}
	}

	private void Jump() {
		Velocity += Vector2.Up * _jumpSpeed;
		_isShooting = false;
	}

	private void Shoot() {
		var laser = (Laser)_instancer.InstanceSceneToLevel(
			_instancer.LaserShotScene,
			_isFacingLeft ? _shootLeftMarker.GlobalPosition : _shootRightMarker.GlobalPosition
		);
		laser.Launch(_isFacingLeft ? Vector2.Left : Vector2.Right);
		_canShoot = false;
		_isShooting = Input.IsActionPressed("shoot");
		_shootCooldownTimer.Start();
		
	}

	private void OnShootCooldownTimeout() {
		_canShoot = true;
		if (_isShooting && Input.IsActionPressed("shoot")) Shoot();
		else _isShooting = false;
	}

	private void OnHitboxDied() {
		EmitSignalHealthChanged(PlayerHitbox.MaxHp, PlayerHitbox.Hp);
		QueueFree();
	}

	private void OnHitboxTookDamage(float amount) {
		PlayerHitbox.TurnInvincible(_invincibilityDuration);
		_playerInvincibleAnimationPlayer.Play("hurt");
		EmitSignalHealthChanged(PlayerHitbox.MaxHp, PlayerHitbox.Hp);
	}

	private void OnInvincibilityEnded() {
		_playerInvincibleAnimationPlayer.Stop();
	}
}
