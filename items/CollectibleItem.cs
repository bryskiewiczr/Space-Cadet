using Godot;
using System;
using System.Collections;

public partial class CollectibleItem : Area2D
{
	[Export] public string ItemName = "";
	public override void _Ready() {
		BodyEntered += OnBodyEntered;
	}

	public override void _Process(double delta)
	{
	}

	private void OnBodyEntered(Node2D body) {
		if (body is Player) {
			QueueFree();
		}
	}
}
