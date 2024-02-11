namespace Silk.NET_Tutorials.Rendering;

using System.Numerics;
using Silk.NET.OpenGL;
using Silk.NET.SDL;

public class Shader : IDisposable
{
    private readonly GL gl;
    private readonly uint handle;
    
    public Shader(GL gl, string vertexShaderPath, string fragmentShaderPath)
    {
        this.gl = gl;
        
        var vertexShader = this.LoadAndCompileShader(ShaderType.VertexShader, vertexShaderPath);
        var fragmentShader = this.LoadAndCompileShader(ShaderType.FragmentShader, fragmentShaderPath);

        this.handle = this.gl.CreateProgram();
        this.gl.AttachShader(this.handle, vertexShader);
        this.gl.AttachShader(this.handle, fragmentShader);
        this.gl.LinkProgram(this.handle);
        
        this.gl.GetProgram(this.handle, GLEnum.LinkStatus, out var status);
        if (status == 0)
        {
            throw new ShaderException($"Program failed to link with error: {this.gl.GetProgramInfoLog(this.handle)}");
        }
        
        this.gl.DetachShader(this.handle, vertexShader);
        this.gl.DetachShader(this.handle, fragmentShader);
        this.gl.DeleteShader(vertexShader);
        this.gl.DeleteShader(fragmentShader);
    }

    public unsafe void SetUniform(string name, Matrix4x4 value)
    {
        var location = this.gl.GetUniformLocation(this.handle, name);
        if (location == -1)
        {
            throw new ShaderException($"uniform '{name}' not found on shader");
        }
        
        this.gl.UniformMatrix4(location,  1, false, (float*) &value);
    }
    
    public void SetUniform(string name, int value)
    {
        var location = this.gl.GetUniformLocation(this.handle, name);
        if (location == -1)
        {
            throw new ShaderException($"uniform '{name}' not found on shader");
        }
        
        this.gl.Uniform1(location, value);
    }
    
    public void SetUniform(string name, float value)
    {
        var location = this.gl.GetUniformLocation(this.handle, name);
        if (location == -1)
        {
            throw new ShaderException($"uniform '{name}' not found on shader");
        }
        
        this.gl.Uniform1(location, value);
    }

    public void SetUniform(string name, Vector3 value)
    {
        var location = this.gl.GetUniformLocation(this.handle, name);
        if (location == -1)
        {
            throw new ShaderException($"uniform '{name}' not found on shader");
        }
        
        this.gl.Uniform3(location, value);
    }
    
    public void Use()
    {
        this.gl.UseProgram(this.handle);
    }
    
    public void Dispose()
    {
        this.gl.DeleteProgram(this.handle);
    }
    
    private uint LoadAndCompileShader(ShaderType shaderType, string file)
    {
        var shader = this.gl.CreateShader(shaderType);
        var source = LoadShaderSource(file);
        this.gl.ShaderSource(shader, source);
        this.gl.CompileShader(shader);

        var infoLog = this.gl.GetShaderInfoLog(shader);
        if (!string.IsNullOrWhiteSpace(infoLog))
        {
            throw new ShaderException($"Error compiling {shaderType} shader - {file}: {infoLog}");
        }

        return shader;
    }
    
    private static string LoadShaderSource(string filename)
    {
        var path = $"Shaders/{filename}";
        if (!File.Exists(path))
        {
            throw new ShaderException($"Shader file {Path.Join(Directory.GetCurrentDirectory(), path)} not found");
        }

        return File.ReadAllText($"Shaders/{filename}");
    }
}