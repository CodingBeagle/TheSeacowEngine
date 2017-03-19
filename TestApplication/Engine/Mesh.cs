using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using GlmSharp;
using GLFWSharpie;

namespace TestApplication.Engine
{
    public class Mesh
    {
        private readonly List<uint> indices;
        private readonly List<float> vertices;

        private readonly int vertexArrayObject;

        public IList<uint> Indices => indices.AsReadOnly();
        public IList<float> Vertices => vertices.AsReadOnly();

        public quat Orientation { get; set; }
        public vec3 Position { get; set; }
        public float Scale { get; set; }
        public Material Material { get; }
        public Texture Texture { get; }

        public Mesh(List<float> vertices, List<uint> indices, Material material, Texture texture)
            : this(vertices, indices, material)
        {
            Texture = texture;
        }

        public Mesh(List<float> vertices, List<uint> indices, Material material)
        {
            this.vertices = vertices;
            this.indices = indices;
            Material = material;

            Orientation = quat.Identity;
            Position = vec3.Zero;
            Scale = 1.0f;

            /*
             * A VAO (Vertex Array Object) stores state needed to supply vertex data from the application
             * To the vertex shader during what is called the "vertex fetch" stage.
             * 
             * This state includes the buffer object bound when the VAO was active,
             * And other vertex array related states, such as glEnableVertexAttribArray and
             * glVertexAttribPointer.
             * 
             * In order for a vertex attribute to be used, it must be manually enabled!
             * 
             * In the OpenGL core profile, it is required to use a VAO.
             * 
             * Also, remember that the actual VBO is NOT a part of the VAO state!!!! Which would
             * Totally make sense, but is not the case. So remember to actually bind the relevant
             * VBO when you bind your VAO!
             */
            uint[] generatedVertexArray = new uint[1];
            Gl.GenVertexArrays(2, generatedVertexArray);
            Gl.CheckForError();

            vertexArrayObject = (int)generatedVertexArray[0];

            // Bind the vertex array
            Gl.BindVertexArray((uint)vertexArrayObject);
            Gl.CheckForError();

            // Generate a vertex buffer object to store vertex data for the mesh
            uint[] generatedVBOs = new uint[1];
            Gl.GenBuffers(1, generatedVBOs);
            Gl.CheckForError();

            var vertexBufferObject = (int)generatedVBOs[0];

            // Bind the vertex buffer object to store vertex data for the mesh
            Gl.BindBuffer(BufferTarget.Array, (uint)vertexBufferObject);
            Gl.CheckForError();

            /* Enable vertex attribute at location 0 and 1 in the vertex shader
             * Location 0 = Vertex Buffer Object
             * Location 1 = Texture Coordinates
             *
             */
            Gl.EnableVertexAttribArray(0);
            Gl.CheckForError();

            Gl.EnableVertexAttribArray(1);
            Gl.CheckForError();

            // Specif vertex buffer layout
            Gl.VertexAttribPointer(0, 3, VertexAttribPointerDataType.Float, 0, sizeof(float) * 5, (IntPtr)0);
            Gl.CheckForError();

            Gl.VertexAttribPointer(1, 2, VertexAttribPointerDataType.Float, 0, sizeof(float) * 5, (IntPtr) (sizeof(float) * 3));
            Gl.CheckForError();

            // Upload vertex data
            Gl.BufferData(BufferTarget.Array, (vertices.Count * sizeof(float)), vertices.ToArray(), BufferUsageHint.StaticDraw);
            Gl.CheckForError();

            /*
             * Element buffer objects are used for indexed rendering.
             * Indexed rendering can help overcome the problem of having duplicate
             * Vertices when rendering more complex meshes.
             * The way it works is that you have a VBO of vertices that your model consists of.
             * In this VBO, htere will be no overlapping/duplicated vertices.
             * 
             * Instead, you then complement this VBO with an EBO. The EBO is simply an array of
             * Indices, each consecutive 3 indices representing one triangle. Each index is an
             * Index into the vertex elements of the VBO.
             * 
             * This way, you can specify exactly which vertices to use for each triangle.
             * More importantly, you can reuse vertices for different triangles!
             * This gives major benefits in the reduction of vertices send to the GPU!
             * 
             * The VAO also saves state related to GL_ELEMENT_ARRAY_BUFFER, which is the
             * Buffer target used for indexes for the vertex array
             */
            uint[] elemBuffers = new uint[1];
            Gl.GenBuffers(1, elemBuffers);
            Gl.CheckForError();

            var elementBufferObject = elemBuffers[0];

            // Bind buffer
            Gl.BindBuffer(BufferTarget.ElementArray, elementBufferObject);
            Gl.CheckForError();

            // Upload index data
            Gl.BufferData(BufferTarget.ElementArray, (sizeof(uint) * indices.Count), indices.ToArray(), BufferUsageHint.StaticDraw);
            Gl.CheckForError();

            // Clean up OpenGL state by unbinding
            Gl.BindVertexArray(0);
            Gl.CheckForError();

            Gl.BindBuffer(BufferTarget.Array, 0);
            Gl.CheckForError();

            Gl.BindBuffer(BufferTarget.ElementArray, 0);
            Gl.CheckForError();
        }

        public void Activate()
        {
            // Remember, GL_ARRAY_BUFFER binding is **NOT** part of the VAO's state!
            Gl.BindBuffer(BufferTarget.Array, (uint)vertexArrayObject);
            Gl.CheckForError();

            Gl.BindVertexArray((uint)vertexArrayObject);
            Gl.CheckForError();
        }

        public void DeActivate()
        {
            Gl.BindVertexArray(0);
            Gl.CheckForError();
        }

        public void Draw()
        {
            // Enable texture if applicable
            Texture?.Activate();

            // Create perspective matrix
            mat4 perspectiveMatrix = mat4.PerspectiveFov(
                glm.Radians(45.0f),
                800.0f,
                600.0f,
                0.1f,
                500.0f);

            // Create view matrix
            mat4 viewMatrix = mat4.LookAt(
                new vec3(0.0f, 0.0f, -3.0f),
                vec3.Zero,
                vec3.UnitY);

            // Model matrix
            mat4 scaleMatrix = mat4.Scale(Scale);
            mat4 translationMatrix = mat4.Translate(Position.x, Position.y, Position.z);
            mat4 rotationMatrix = Orientation.ToMat4;

            mat4 modelMatrix = scaleMatrix * rotationMatrix * translationMatrix;

            // Transfer to shader uniforms
            int modelLocation = Gl.UniformLocation(Material.ProgramObject, "model");
            Gl.CheckForError();
            Gl.UniformMatrix4(modelLocation, 0, modelMatrix.ToArray());
            Gl.CheckForError();

            int projectionLocation = Gl.UniformLocation(Material.ProgramObject, "projection");
            Gl.CheckForError();
            Gl.UniformMatrix4(projectionLocation, 0, perspectiveMatrix.ToArray());
            Gl.CheckForError();

            int viewLocation = Gl.UniformLocation(Material.ProgramObject, "view");
            Gl.CheckForError();
            Gl.UniformMatrix4(viewLocation, 0, viewMatrix.ToArray());
            Gl.CheckForError();

            // Turn on fragment color usage
            int colorBoolLocation = Gl.UniformLocation(Material.ProgramObject, "isTexture");
            Gl.Uniform1(colorBoolLocation, 1);
            Gl.CheckForError();

            // Pass on fragment color
            //int userColorLocation = Gl.UniformLocation(Material.ProgramObject, "userColor");
            //Gl.Uniform3(userColorLocation, new vec3(1.0f, 0.0f, 0.0f).ToArray());
            //Gl.CheckForError();

            Gl.DrawElements(DrawMode.Triangles, (uint)Vertices.Count, DrawType.UnsignedInt, IntPtr.Zero);
            Gl.CheckForError();
        }
    }
}