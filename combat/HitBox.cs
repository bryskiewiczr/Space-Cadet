using Godot;

public partial class HitBox : Area2D {
    [Export]
    public float MaxHp = 100.0f;
    public float Hp = 100.0f;

    private Timer _invincibilityTimer;

    private bool _isInvincible = false;

    [Signal]
    public delegate void DiedEventHandler();
    [Signal]
    public delegate void TookDamageEventHandler(float damage);

    [Signal]
    public delegate void InvincibilityEndedEventHandler();
    
    public override void _Ready() {
        Hp = MaxHp;
        _invincibilityTimer = GetNode<Timer>("InvincibilityTimer");
        _invincibilityTimer.Timeout += OnInvincibilityTimerTimeout;
    }

    public bool TakeDamage(float damage) {
        var tookDamage = false;
        if (damage > 0.0f && !_isInvincible) {
            Hp -= damage;
            tookDamage = true;
            GD.Print($"Damage taken: {damage}");
            if (Hp <= 0.0f) {
                Hp = 0.0f;
                EmitSignal(SignalName.Died);
            } else {
                EmitSignal(SignalName.TookDamage, damage);
            }
        }
        return tookDamage;
    }

    public void TurnInvincible(float seconds) {
        if (_isInvincible) return;
        _isInvincible = true;
        _invincibilityTimer.Start(seconds);
    }

    private void OnInvincibilityTimerTimeout() {
        _isInvincible = false;
        EmitSignal(SignalName.InvincibilityEnded);
    }
}
