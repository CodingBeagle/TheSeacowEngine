using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using GLFWSharpie;
using TestApplication.Engine;
using TestApplication.Engine.ContentPipeline.Obj;

namespace TestApplication
{
    class Program
    {
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

            Shader myVertexShader = new Shader("Data/Shaders/Vertex.Shader", ShaderType.VertexShader);
            Shader myFragmentShader = new Shader("Data/Shaders/Fragment.Shader", ShaderType.FragmentShader);

            Material mySimpleMaterial = new Material(new List<Shader>() {myVertexShader, myFragmentShader});
            
            // Load texture
            Texture dirtTexture = new Texture("Data/Textures/simpleTex.bmp");

            ObjLoader experimentalLoader = new ObjLoader("Data/Models/sphere/textureSphere.obj");
            ModelData experimentalModelData = experimentalLoader.LoadModel();
            ObjWeaver objWeave = new ObjWeaver();
            WeavedModelData weavedModelData = objWeave.WeaveModelData(experimentalModelData);

            Mesh mySphere = new Mesh(
                weavedModelData.WeavedVertexData,
                weavedModelData.Indexes.Select(i => (uint)i).ToList(),
                mySimpleMaterial,
                dirtTexture);

            mySimpleMaterial.Activate();
            mySphere.Activate();
            mySphere.Scale = 0.5f;

            // Enable depth testing
            Gl.Enable((uint)Capability.DepthTest);
            Gl.CheckForError();

            while (Glfw.WindowShouldClose(windowHandle) == 0)
            {
                Gl.ClearColor(0.39f, 0.58f, 0.92f, 0.0f);
                Gl.CheckForError();

                Gl.Clear((int)ColorBit.ColorBuffer | (int)ColorBit.DepthBuffer);
                Gl.CheckForError();

                mySphere.Orientation *= quat.FromAxisAngle(0.05f, vec3.UnitY);

                // Draw a mesh !=D
                mySphere.Draw();

                Glfw.SwapBuffers(windowHandle);
                Glfw.PollEvents();
            }

            Glfw.Terminate();
        }
    }
}