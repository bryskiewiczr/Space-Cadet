using Godot;

public partial class Level : Node {

    public override void _Ready() { }

    public override void _Process(double delta) {
        if (Input.IsActionJustPressed("reset_game")) GetTree().ReloadCurrentScene();
        if (Input.IsActionJustPressed("exit_game")) GetTree().Quit();
    }
}

