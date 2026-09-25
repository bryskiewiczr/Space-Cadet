using Godot;
using System;

public partial class HurtBox : Area2D {
    [Export] private float _damage = 10.0f;

    [Signal]
    public delegate void DamageAppliedEventHandler();

    public override void _Ready() {
        AreaEntered += OnAreaEntered;
    }
    
    private bool ApplyDamage(HitBox hitbox) {
        var tookDamage = hitbox.TakeDamage(_damage);
        if (tookDamage) EmitSignal(SignalName.DamageApplied);
        return tookDamage;
    }

    private void OnAreaEntered(Area2D area) {
        if (area is HitBox hitbox) {
            GD.Print("Damage applied");
            ApplyDamage(hitbox);
        }
    }
}
