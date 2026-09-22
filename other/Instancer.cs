using Godot;

public partial class Instancer : Node {

	public PackedScene DustEffectScene;		// dust cloud when landing
	public PackedScene LaserShotScene;		// laser projectile
	
	public override void _Ready() {
		DustEffectScene = ResourceLoader.Load<PackedScene>("res://fx/DustEffect.tscn");
		LaserShotScene = ResourceLoader.Load<PackedScene>("res://combat/laser.tscn");
	}
	
	public Node2D InstanceSceneToLevel(PackedScene scene, Vector2 position) {
		var sceneInstance = scene.Instantiate<Node2D>();
		sceneInstance.GlobalPosition = position;
		GetTree().CurrentScene.AddChild(sceneInstance);
		return sceneInstance;
	}
}
