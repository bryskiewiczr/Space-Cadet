using Godot;

public partial class Hud : Control {
    private ProgressBar _healthBar;

    public override void _Ready() {
        _healthBar = GetNode<ProgressBar>("HealthBar");
    }

    public void SetHealthBarValues(float maxHp, float hp) {
        _healthBar.MaxValue = maxHp;
        _healthBar.Value = hp;
    }
}
