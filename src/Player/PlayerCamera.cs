using Godot;

public partial class PlayerCamera : Camera3D
{
    [Export]
    private float zoomSpeed = 0.1f;

    public float Zoom = 1.0f;
    private float targetZoom = 1.0f;
    private Sprite3D player;

    private Vector3 offset;

    public override void _Ready()
    {
        player = GetNode<Sprite3D>("%Sprite3D");
    }

    public override void _Process(double delta)
    {
        if (Zoom != targetZoom)
        {
            float newZoom = Mathf.Lerp(Zoom, targetZoom, zoomSpeed);
            targetZoom = Zoom;
            SetCameraZoom(newZoom);
        }

        LookAt(player.GlobalTransform.Origin, Vector3.Up);
    }

    public void SetCameraZoom(float zoom)
    {
        Zoom = Mathf.Clamp(zoom, 0.1f, 5.0f);
        Projection = ProjectionType.Perspective;
        Fov = Mathf.RadToDeg(Mathf.Atan(1 / zoom));
    }
}
