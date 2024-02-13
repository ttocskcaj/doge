using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using System.Drawing;
using System.Numerics;
using Silk.NET_Tutorials.Rendering;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using MouseButton = Silk.NET.Input.MouseButton;
using Shader = Silk.NET_Tutorials.Rendering.Shader;
using Texture = Silk.NET_Tutorials.Rendering.Texture;

namespace Silk.NET_Tutorials;


internal static class Program
{
    private static IWindow window = null!;
    private static GL gl = null!;
    private static ImGuiController imGuiController = null!;

    private static Shader shader = null!;
    private static Texture texture = null!;
    private static Camera camera = null!;
    private static Renderer renderer = null!;

    private static List<GameObject> gameObjects = new();
    private static Lamp lamp = null!;
    private static Vector3 lightSource = new (100f);

    private static int frameCount = 0;
    private static double fpsNextUpdate = 0.0;
    private static double fps = 0.0;
    private const double FpsUpdateRate = 2.0;

    private static void Main(string[] args)
    {
        var monitor = NET.Windowing.Monitor.GetMainMonitor(null);

        var options = WindowOptions.Default;
        options.Size = monitor.Bounds.Size;
        options.Title = "Doge";
        options.VSync = true;
        options.WindowState = WindowState.Fullscreen;

        window = monitor.CreateWindow(options);

        window.Load += OnLoad;
        window.Update += OnUpdate;
        window.Render += OnRender;
        window.Closing += OnClose;
        window.FramebufferResize += Resize;
        window.Resize += Resize;
    
        // Run and block until closed.
        window.Run();

        window.Dispose();
    }

    private static void Resize(Vector2D<int> size)
    {
        gl!.Viewport(0, 0, (uint)size.X, (uint)size.Y);
        camera.Resize(size);
    }

    private static void OnLoad()
    {
        var input = window!.CreateInput();

        foreach (var keyboard in input.Keyboards)
        {
            keyboard.KeyDown += KeyDown;
        }

        foreach (var mouse in input.Mice)
        {
            mouse.MouseDown += MouseDown;
            // mouse.Cursor.CursorMode = CursorMode.Hidden;
            // mouse.Cursor.Type = CursorType.Standard;
        }
        
        gl = GL.GetApi(window);

        gl.Enable(EnableCap.DepthTest);

        shader = new Shader(gl, "shader.vert", "shader.frag");
        texture = new Texture(gl, "doge.png");
        camera = new Camera(input)
        {
            Position = new Vector3(0, 0, 3),
        };
        
        camera.Resize(window.Size);
        renderer = new Renderer(gl, camera);

        int startSize = 200;

        var random = new Random();
        for (int i = 0; i < 100; i++)
        {
            var doge = new DogeGameObject(gl, texture, shader)
            {
                Transform = new Transform { Position = new Vector3(random.Next(-(startSize/2), (startSize/2)), random.Next(-(startSize/2), (startSize/2)), random.Next(-(startSize/2), (startSize/2))) },
            };
            
            gameObjects.Add(doge);
        }

        var grumpyTexture = new Texture(gl, "grumpy.png");
        var grumpy = new GrumpyGameObject(gl, grumpyTexture, shader)
        {
            Transform = new Transform { Position = new Vector3(random.Next(-(startSize/2), (startSize/2)), random.Next(-(startSize/2), (startSize/2)), random.Next(-(startSize/2), (startSize/2))) },
        };
        
        gameObjects.Add(grumpy);
        
        lamp = new Lamp(gl, lightSource);

        imGuiController = new ImGuiController(gl, window, input);
    }

    private static void MouseDown(IMouse mouse, MouseButton button)
    {
    }

    private static void OnRender(double deltaTime)
    {
        StbImageSharp.StbImage.stbi_set_flip_vertically_on_load(1);
        imGuiController.Update((float)deltaTime);

        renderer.Render(gameObjects, lamp);
        
        
        ImGuiNET.ImGui.Text($"FPS: {fps}");
        ImGuiNET.ImGui.Text($"Doge Count: {gameObjects.Count}");

        imGuiController.Render();
    }

    private static void OnUpdate(double deltaTime)
    {
        ArgumentNullException.ThrowIfNull(window);

        frameCount++;
        if (window.Time > fpsNextUpdate)
        {
            fpsNextUpdate += 1.0 / FpsUpdateRate;
            fps = frameCount * FpsUpdateRate;
            frameCount = 0;
        }

        window.Title = $"Doge --- FPS: {fps}";

        foreach (var gameObject in gameObjects)
        {
            gameObject.OnUpdate(deltaTime, window.Time);
        }

        camera.OnUpdate(deltaTime, window.Time);
    }

    private static void OnClose()
    {
        foreach (var gameObject in gameObjects)
        {
            gameObject.Dispose();
        }
    }

    private static void KeyDown(IKeyboard arg1, Key arg2, int arg3)
    {
        //Check to close the window on escape.
        if (arg2 == Key.Escape)
        {
            window!.Close();
        }
    }
}
