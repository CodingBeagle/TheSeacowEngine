using System.Collections.Generic;
using System.Text;
using GLFWSharpie;
using TestApplication.Exceptions;

namespace TestApplication.Engine
{
    public class Material
    {
        public uint ProgramObject { get; }

        public Material(List<Shader> shaders)
        {
            // Create shader program
            ProgramObject = Gl.CreateProgram();

            // Attach provided shaders
            foreach (Shader shader in shaders)
            {
                Gl.AttachShader(ProgramObject, shader.ShaderObject);
            }

            // Link program
            Gl.LinkProgram(ProgramObject);

            /*
             * After linking (no matter if it was successful or not)
             * It is a good idea to detach all shader objects from the program.
             * 
             * This is because the shaders does not need to be attached after linking
             * OCcurs, and having the shaders attached means
             * That they cannot eventually be deleted, because calls
             * To glDeleteShader seems to require that they will only be flagged
             * For deletion, and that the actual deletion will only happen once it is not
             * Attached to any shader program any longer
             */
            foreach (Shader shader in shaders)
            {
                Gl.DetachShader(ProgramObject, shader.ShaderObject);
            }

            // Check link status
            int programLinkStatus = 0;
            Gl.GetProgram(ProgramObject, ProgramParameter.LinkStatus, out programLinkStatus);

            if (programLinkStatus == 0)
            {
                // Get program info log length
                int infoLogLength = 0;
                Gl.GetProgram(ProgramObject, ProgramParameter.InfoLogLength, out infoLogLength);

                // Get program info log
                StringBuilder infoLogText = new StringBuilder(infoLogLength);
                uint actualInfoLogLength = 0;
                Gl.GetProgramInfoLog(ProgramObject, (uint)infoLogLength, out actualInfoLogLength, infoLogText);

                throw new MaterialException($"Program failed to link with the following error: {infoLogText}");
            }
;        }

        public void Activate()
        {
            Gl.UseProgram(ProgramObject);
            Gl.CheckForError();
        }

        public void DeActivate()
        {
            Gl.UseProgram(0);
            Gl.CheckForError();
        }
    }
}