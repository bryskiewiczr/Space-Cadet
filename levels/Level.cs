using Godot;

public partial class Level : Node {

    private Player _player;
    private Camera2D _gameCamera;
    private Hud _hud;

    public override void _Ready() {
        _player = GetNode<Player>("Player");
        _gameCamera = GetNode<Camera2D>("GameCamera");
        _player.PlayerRemoteTransform2D.RemotePath = _gameCamera.GetPath();
        _hud = GetNode<Hud>("UILayer/Hud");
        _hud.SetHealthBarValues(
            _player.PlayerHitbox.MaxHp,
            _player.PlayerHitbox.Hp
        );

        // fix this
        Player.HealthChangedEventHandler += OnPlayerHealthChanged;
    }

    public override void _Process(double delta) {
        if (Input.IsActionJustPressed("reset_game")) GetTree().ReloadCurrentScene();
        if (Input.IsActionJustPressed("exit_game")) GetTree().Quit();
    }

    public void OnPlayerHealthChanged(float maxHp, float hpLeft) {
        _hud.SetHealthBarValues(maxHp, hpLeft);
    }
}

