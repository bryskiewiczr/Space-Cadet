Velocity is a Vector2 struct. 
You can replace the whole value, 
but you can’t modify a component like Velocity.Y directly.

```csharp
public override void _PhysicsProcess(double delta) {
    Velocity += GetGravity() * (float)delta;
    MoveAndSlide();
}
```

If you need to change individual components, use a local copy:
```csharp
Vector2 velocity = Velocity;
velocity.Y += 100f * (float)delta;
Velocity = velocity;
```
