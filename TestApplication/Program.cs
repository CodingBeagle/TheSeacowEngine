using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using GLFWSharpie;
using TestApplication.Engine;
using TestApplication.Engine.Collision;
using TestApplication.Engine.ContentPipeline.Obj;
using TestApplication.Engine.Physics;

namespace TestApplication
{
    class Program
    {
        private static List<Particle> ActiveParticles = new List<Particle>();

        private static Glfw.InputCallback callback = new Glfw.InputCallback(CallerBacker);

        private static Mesh mySphere;
        private static Mesh indicatorSphere;

        static void Main(string[] args)
        {
            // Initialize GLFW
            int result = Glfw.Init();

            if (result == 0)
            {
                Console.WriteLine("Failed to initialize GLFW");
                return;
            }

            // Display GLFW version initialized
            Console.WriteLine("GLFW Version: " + Marshal.PtrToStringAnsi(Glfw.GetVersionString()));

            // Set window creation hints
            Glfw.WindowHint((int)GlfwWindowHint.ContextVersionMajor, 3);
            Glfw.WindowHint((int)GlfwWindowHint.ContextVersionMinor, 2);

            // Create window
            IntPtr windowHandle = Glfw.CreateWindow(800, 600, "Seacow Engine", IntPtr.Zero, IntPtr.Zero);

            // Make the window's context current
            Glfw.MakeContextCurrent(windowHandle);

            // Display context information
            int majorVersion = Glfw.GetWindowAttrib(windowHandle, (int)GlfwWindowHint.ContextVersionMajor);
            int minorVersion = Glfw.GetWindowAttrib(windowHandle, (int)GlfwWindowHint.ContextVersionMinor);

            Console.WriteLine("OpenGL Major Version: " + majorVersion);
            Console.WriteLine("OpenGL Minor Version: " + minorVersion);

            // Set GLFW input callback
            Glfw.SetKeyCallback(windowHandle, Marshal.GetFunctionPointerForDelegate(callback));

            // Own init stuff
            Renderer myRenderer = new Renderer();

            Shader myVertexShader = new Shader("Data/Shaders/Vertex.Shader", ShaderType.VertexShader);
            Shader myFragmentShader = new Shader("Data/Shaders/Fragment.Shader", ShaderType.FragmentShader);

            Material mySimpleMaterial = new Material(new List<Shader>() {myVertexShader, myFragmentShader});
            
            // Load texture
            Texture dirtTexture = new Texture("Data/Textures/simpleTex.bmp");

            // Load sphere
            ObjLoader experimentalLoader = new ObjLoader("Data/Models/sphere/textureSphere.obj");
            ModelData experimentalModelData = experimentalLoader.LoadModel();
            ObjWeaver objWeave = new ObjWeaver();
            var weavedModelData = objWeave.WeaveModelData(experimentalModelData);

            // Load box
            ObjLoader boxLoader = new ObjLoader("Data/Models/Box/simpleBox.obj");
            ModelData boxModelData = boxLoader.LoadModel();
            ObjWeaver boxModelDataWeaver = new ObjWeaver();
            var weavedBoxModelData = boxModelDataWeaver.WeaveModelData(boxModelData);

            // Create sphere
            mySphere = new Mesh(
                weavedModelData.weavedVertexData,
                weavedModelData.indexes.Select(i => (uint)i).ToList(),
                mySimpleMaterial,
                dirtTexture);

            mySphere.Scale = 0.1f;
            mySphere.Position = new vec3(0.3f, 1.0f, 0.0f);
            mySimpleMaterial.Activate();

            // Create box
            Mesh myBox = new Mesh(
                weavedBoxModelData.weavedVertexData,
                weavedBoxModelData.indexes.Select(i => (uint)i).ToList(),
                mySimpleMaterial
                );

            myBox.Position = vec3.UnitX * 1;
            myBox.Scale = 0.5f;
            myBox.UserColor = new Vector3(0.0f, 0.0f, 1.0f);

            // Create indicator sphere
            indicatorSphere = new Mesh(
                weavedModelData.weavedVertexData,
                weavedModelData.indexes.Select(i => (uint)i).ToList(),
                mySimpleMaterial
                );

            indicatorSphere.UserColor = new Vector3(1.0f, 0.0f, 0.0f);
            indicatorSphere.Scale = 0.1f;

            // Add objects to world
            myRenderer.WorldMeshes.Add(mySphere);
            myRenderer.WorldMeshes.Add(myBox);
            //myRenderer.WorldMeshes.Add(indicatorSphere);

            // ** Do all sorts of fun collision stuff! :D :D **
            Aabb colBox = new Aabb(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(1.5f, 0.5f, 0.5f));
            Vector3 circlePoint = new Vector3(mySphere.Position.x, mySphere.Position.y, mySphere.Position.z);

            // Enable depth testing
            Gl.Enable((uint)Capability.DepthTest);
            Gl.CheckForError();

            const double dt = 0.01;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            double currentTime = stopwatch.ElapsedMilliseconds / 1000.0f;
            double accumulator = 0.0;

            while (Glfw.WindowShouldClose(windowHandle) == 0)
            {
                // Timer stuff
                double newTime = stopwatch.ElapsedMilliseconds / 1000.0f;
                double frameTime = newTime - currentTime;
                currentTime = newTime;

                accumulator += frameTime;

                int counter = 0;

                // Fixed timestep
                while (accumulator >= dt)
                {
                    Sphere theSphere = new Sphere(new Vector3(mySphere.Position.x, mySphere.Position.y, mySphere.Position.z), 0.1f);
                    var colResult = IntersectionDetector.TestSphereAgainstAabb(theSphere, colBox);

                    if (colResult)
                        Console.WriteLine("COLLISION");

                    //Integrate(dt);
                    accumulator -= dt;
                }

                // Rendering
                Gl.ClearColor(0.39f, 0.58f, 0.92f, 0.0f);
                Gl.CheckForError();

                Gl.Clear((int)ColorBit.ColorBuffer | (int)ColorBit.DepthBuffer);
                Gl.CheckForError();

                myRenderer.Render((float)(accumulator / dt));

                Glfw.SwapBuffers(windowHandle);
                Glfw.PollEvents();
            }

            Glfw.Terminate();
        }

        private static void CallerBacker(IntPtr window, int key, int scancode, int action, int mods)
        {
            if (key == (int) Glfw.KeyboardKey.W && (action == (int) Glfw.KeyboardAction.Press || action == (int)Glfw.KeyboardAction.Repeat))
            {
                mySphere.Position = mySphere.Position + vec3.UnitY * 0.05f;
            }

            if (key == (int)Glfw.KeyboardKey.S && (action == (int)Glfw.KeyboardAction.Press || action == (int)Glfw.KeyboardAction.Repeat))
            {
                mySphere.Position = mySphere.Position - vec3.UnitY * 0.05f;
            }

            if (key == (int)Glfw.KeyboardKey.A && (action == (int)Glfw.KeyboardAction.Press || action == (int)Glfw.KeyboardAction.Repeat))
            {
                mySphere.Position = mySphere.Position - vec3.UnitX * 0.05f;
            }

            if (key == (int)Glfw.KeyboardKey.D && (action == (int)Glfw.KeyboardAction.Press || action == (int)Glfw.KeyboardAction.Repeat))
            {
                mySphere.Position = mySphere.Position + vec3.UnitX * 0.05f;
            }
        }

        private static void Integrate(double state)
        {
            //CommonPhysics.Integrate((float)state);
        }
    }
}