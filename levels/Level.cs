using Godot;

public partial class Level : Node {

    private Player _player;
    private Camera2D _gameCamera;

    public override void _Ready() {
        _player = GetNode<Player>("Player");
        _gameCamera = GetNode<Camera2D>("GameCamera");
        _player.PlayerRemoteTransform2D.RemotePath = _gameCamera.GetPath();
    }

    public override void _Process(double delta) {
        if (Input.IsActionJustPressed("reset_game")) GetTree().ReloadCurrentScene();
        if (Input.IsActionJustPressed("exit_game")) GetTree().Quit();
    }
}

