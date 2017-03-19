using System;
using System.IO;
using System.Text;
using GLFWSharpie;
using TestApplication.Exceptions;

namespace TestApplication.Engine
{
    // TODO: Describe class properly
    /// <summary>
    /// 
    /// </summary>
    public class Shader
    {
        public uint ShaderObject { get; }

        public Shader(string filepath, ShaderType shaderType)
        {
            string shaderFileContent = File.ReadAllText(filepath);

            if (string.IsNullOrEmpty(shaderFileContent))
                throw new ShaderException("The specified shader file was empty."); 

            // Create shader object
            ShaderObject = Gl.CreateShader(shaderType);
            Gl.CheckForError();

            // Set shader source code
            Gl.ShaderSource(ShaderObject, 1, new string[] {shaderFileContent}, IntPtr.Zero);
            Gl.CheckForError();

            // Compile shader source code
            Gl.CompileShader(ShaderObject);
            Gl.CheckForError();

            // Check shader compilation status
            int shaderCompilationStatus = 0;
            Gl.GetShader(ShaderObject, ShaderParameter.CompileStatus, out shaderCompilationStatus);

            if (shaderCompilationStatus == 0)
            {
                // Get shader info length
                int shaderInfoLenght = 0;
                Gl.GetShader(ShaderObject, ShaderParameter.InfoLogLength, out shaderInfoLenght);

                // Get shader info text
                uint actualShaderInfoLength = 0;
                StringBuilder shaderInfo = new StringBuilder(shaderInfoLenght);
                Gl.GetShaderInfoLog(ShaderObject, (uint)shaderInfoLenght, out actualShaderInfoLength, shaderInfo);
                
                throw new ShaderException($"Shader failed to compile with the following error: {shaderInfo.ToString()}");
            }
        }
    }
}