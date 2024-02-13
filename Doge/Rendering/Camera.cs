namespace Silk.NET_Tutorials.Rendering;

using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;

public class Camera
{
    private readonly IInputContext inputContext;
    

    private Vector2 previousMousePosition = Vector2.Zero;

    private Vector3 up = Vector3.UnitY;
    private Vector3 front = -Vector3.UnitZ;
    private Vector3 right = Vector3.UnitY;

    private float yaw = 0f;
    private float pitch = 0f;
    
    private float aspectRatio = 1.33333333f;
    private float fov = (MathF.PI / 2);

    private bool firstMouse = true;

    public Camera(IInputContext inputContext)
    {
        this.inputContext = inputContext;
    }
    
    public Matrix4x4 ViewMatrix => Matrix4x4.CreateLookAt(this.Position, this.Position + this.front, this.up);
    
    public Matrix4x4 ProjectionMatrix => Matrix4x4.CreatePerspectiveFieldOfView(this.fov, this.aspectRatio, 0.01f, 100f);

    public Vector3 Position { get; set; }
    
    public void OnUpdate(double deltaTime, double gameTime)
    {
        const int speed = 10;
        const float mouseSensitivity = 25.0f;

        var mouse = this.inputContext.Mice[0];

        // Stop the mouse jumping at the very start
        if (this.firstMouse)
        {
            this.previousMousePosition = mouse.Position;
            this.firstMouse = false;
        }
        
        // Handle mouse position
        var mouseOffsetX = mouse.Position.X - this.previousMousePosition.X;
        var mouseOffsetY = mouse.Position.Y - this.previousMousePosition.Y;
        this.previousMousePosition = mouse.Position;

        var yawAdjust = mouseOffsetX * mouseSensitivity * (float)deltaTime;
        var pitchAdjust = -(mouseOffsetY * mouseSensitivity * (float)deltaTime);
        this.AdjustViewDirection(yawAdjust, pitchAdjust);
        
        // Handle keyboard position
        this.HandleInput(deltaTime, speed);
    }

    private void HandleInput(double deltaTime, int speed)
    {
        var keyboard = this.inputContext.Keyboards[0];

        if (keyboard.IsKeyPressed(Key.Up) || keyboard.IsKeyPressed(Key.W))
        {
            this.Position += this.front * (float)(deltaTime * speed);
        }
        
        if (keyboard.IsKeyPressed(Key.Down) || keyboard.IsKeyPressed(Key.S))
        {
            this.Position -= this.front * (float)(deltaTime * speed);
        }

        if (keyboard.IsKeyPressed(Key.Left) || keyboard.IsKeyPressed(Key.A))
        {
            this.Position -= this.right * (float)(deltaTime * speed);
        }

        if (keyboard.IsKeyPressed(Key.Right) || keyboard.IsKeyPressed(Key.D))
        {
            this.Position += this.right * (float)(deltaTime * speed);
        }
        
        if (keyboard.IsKeyPressed(Key.ShiftLeft))
        {
            this.Position -= Vector3.UnitY * (float)(deltaTime * speed);
        }
        
        if (keyboard.IsKeyPressed(Key.Space))
        {
            this.Position += Vector3.UnitY * (float)(deltaTime * speed);
        }
    }

    private void AdjustViewDirection(float yawAdjust, float pitchAdjust)
    {
        this.yaw += MathHelper.DegreesToRadians(yawAdjust);
        this.pitch += MathHelper.DegreesToRadians(pitchAdjust);
        const float pitchPadding = MathF.PI / 10;
        this.pitch = MathHelper.Clamp(this.pitch, -(MathHelper.PiOver2 - pitchPadding), MathHelper.PiOver2 - pitchPadding);

        this.front.X = MathF.Cos(this.pitch) * MathF.Cos(this.yaw);
        this.front.Y = MathF.Sin(this.pitch);
        this.front.Z = MathF.Cos(this.pitch) * MathF.Sin(this.yaw);
        
        this.front = Vector3.Normalize(this.front);
        
        this.right = Vector3.Normalize(Vector3.Cross(this.front, Vector3.UnitY));
        this.up = Vector3.Normalize(Vector3.Cross(this.right, this.front));
    }

    public void DrawDebug()
    {
        ImGuiNET.ImGui.BeginGroup();
        ImGuiNET.ImGui.Text($"Camera Location: {this.Position}");
        ImGuiNET.ImGui.Text($"Camera Front: {this.front}");
        ImGuiNET.ImGui.Text($"Camera Up: {this.up}");
        ImGuiNET.ImGui.Text($"Camera Yaw: {MathHelper.RadiansToDegrees(this.yaw)}");
        ImGuiNET.ImGui.Text($"Camera Pitch: {MathHelper.RadiansToDegrees(this.pitch)}");

        ImGuiNET.ImGui.SliderFloat("Field of View:", ref this.fov, 0.1f, (float)(MathF.PI - 0.1));
    }

    public void Resize(Vector2D<int> size)
    {
        this.aspectRatio = size.X / size.Y;
    }
}
