namespace Silk.NET_Tutorials.Rendering;

using System.Numerics;

public class Transform
{
    public Vector3 Position { get; set; } = new (0, 0, 0);

    public float Scale { get; set; } = 1f;

    public Quaternion Rotation { get; set; } = Quaternion.Identity;

    public Matrix4x4 Matrix =>
        Matrix4x4.Identity
        * Matrix4x4.CreateFromQuaternion(this.Rotation)
        * Matrix4x4.CreateScale(this.Scale)
        * Matrix4x4.CreateTranslation(this.Position);
}
