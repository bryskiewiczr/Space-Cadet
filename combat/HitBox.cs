using Godot;

public partial class HitBox : Area2D {
    [Export]
    private float _maxHp = 100.0f;
    private float _hp = 100.0f;

    [Signal]
    public delegate void DiedEventHandler();

    [Signal]
    public delegate void TookDamageEventHandler(float damage);
    
    public override void _Ready() {
        _hp = _maxHp;
    }

    public bool TakeDamage(float damage) {
        var tookDamage = false;
        if (damage > 0 ) {
            _hp -= damage;
            tookDamage = true;
            
            if (_hp <= 0) {
                _hp = 0;
                EmitSignal(SignalName.Died);
            } else {
                EmitSignal(SignalName.TookDamage, damage);
            }
        }
        return tookDamage;
    }
}
