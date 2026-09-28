using Godot;
using System;

public partial class HurtBox : Area2D {
    [Export] private float _damage = 10.0f;
    
    [Export] private bool _oneShot = true;      // determines if damage is applied continuously on contact
    private bool _isActive = true;

    private HitBox _toDamage;
    
    [Signal]
    public delegate void DamageAppliedEventHandler();

    public override void _Ready() {
        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
    }
    
    private bool ApplyDamage(HitBox hitbox) {
        var tookDamage = hitbox.TakeDamage(_damage);
        if (tookDamage) EmitSignal(SignalName.DamageApplied);
        return tookDamage;
    }

    private void OnAreaEntered(Area2D area) {
        if (!_isActive) return;
        if (area is HitBox hitbox) {
            var isDamageApplied = ApplyDamage(hitbox);
            if (_oneShot) {
                if (isDamageApplied) {
                    _isActive = false;
                }
            } else {
                _toDamage = hitbox;
            }
        }
    }

    private void OnAreaExited(Area2D area) {
        if (area is HitBox) {
            if (area == _toDamage) {
                _toDamage = null;
            }
        }
    }

    public override void _Process(double delta) {
        if ((_isActive) && _toDamage != null) {
            ApplyDamage(_toDamage);
        }
    }
}
