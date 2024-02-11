namespace Silk.NET_Tutorials.Rendering;

using Silk.NET.OpenGL;
using StbImageSharp;

public class Texture : IDisposable
{
    private readonly GL gl;
    private readonly uint handle;

    public unsafe Texture(GL gl, string path)
    {
        this.gl = gl;
        this.handle = this.gl.GenTexture();

        this.Bind();

        var imageBytes = LoadTexture(path);
        var result = ImageResult.FromMemory(imageBytes, ColorComponents.RedGreenBlueAlpha);

        fixed (byte* d = result.Data)
        {
            this.gl.TexImage2D(
                TextureTarget.Texture2D,
                0, InternalFormat.Rgba,
                (uint)result.Width, (uint)
                result.Height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte, d);
        }

        this.SetParameters();
    }

    public unsafe Texture(GL gl, Span<byte> data, uint width, uint height)
    {
        this.gl = gl;
        this.handle = this.gl.GenTexture();

        this.Bind();

        fixed (void* d = &data[0])
        {
            //Setting the data of a texture.
            this.gl.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba,
                PixelType.UnsignedByte, d);
            this.SetParameters();
        }
    }

    public void Bind(TextureUnit textureSlot = TextureUnit.Texture0)
    {
        this.gl.ActiveTexture(textureSlot);
        this.gl.BindTexture(TextureTarget.Texture2D, this.handle);
        this.gl.Enable(EnableCap.Blend);
        this.gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
    }

    public void Dispose()
    {
        this.gl.DeleteTexture(this.handle);
    }

    private static byte[] LoadTexture(string filename)
    {
        var path = $"Textures/{filename}";
        if (!File.Exists(path))
        {
            throw new TextureException($"Texture file {Path.Join(Directory.GetCurrentDirectory(), path)} not found");
        }

        return File.ReadAllBytes(path);
    }


    private void SetParameters()
    {
        this.gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
        this.gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);

        this.gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
            (int)TextureMinFilter.LinearMipmapLinear);
        this.gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
            (int)TextureMagFilter.Linear);

        this.gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
        this.gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 8);

        this.gl.GenerateMipmap(TextureTarget.Texture2D);
    }
}

internal class TextureException : Exception
{
    public TextureException(string message)
        : base(message)
    {
    }
}
