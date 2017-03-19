using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
// ReSharper disable InconsistentNaming

namespace GLFWSharpie
{
    public enum ColorBit
    {
        ColorBuffer = 0x00004000,
        DepthBuffer = 0x00000100
    }

    public enum ShaderType
    {
        FragmentShader = 0x8B30,
        VertexShader = 0x8B31
    }

    public enum ErrorType
    {
        NoError = 0,
        InvalidEnum = 0x0500,
        InvalidValue = 0x0501,
        InvalidOperation = 0x0502
    }

    public enum ShaderParameter
    {
        CompileStatus = 0x8B81,
        InfoLogLength = 0x8B84
    }

    public enum ProgramParameter
    {
        LinkStatus = 0x8B82,
        InfoLogLength = 0x8B84
    }

    public enum BufferTarget
    {
        Array = 0x8892,
        ElementArray = 0x8893
    }

    public enum BufferUsageHint
    {
        StaticDraw = 0x88E4
    }

    public enum VertexAttribPointerDataType
    {
        Float = 0x1406
    }

    public enum DrawMode
    {
        Triangles = 0x0004
    }

    public enum DrawType
    {
        UnsignedInt = 0x1405
    }

    public enum TextureTarget
    {
        Texture2D = 0x0DE1
    }

    public enum TextureParameter
    {
        TextureWrapS = 0x2802,
        TextureWrapT = 0x2803,
        TextureMinFilter = 0x2801,
        TextureMagFilter = 0x2800

    }

    public enum PixelStoreParameter
    {
        UnpackAlignment = 0x0CF5
    }

    public enum InternalPixelFormat
    {
        RGB = 0x1907,
        RGBA = 0x1908
    }

    public enum TextureParameterWrapMode
    {
        ClampToEdge = 0x812F,
        ClampToBorder = 0x812D,
        MirroredRepeat = 0x8370,
        Repeat = 0x2901,
        MirrorClampToEdge = 0x8743
    }

    public enum TextureParameterMinifyingMode
    {
        Linear = 0x2601
    }

    public enum PixelFormat
    {
        RGB = 0x1907,
        BGRA = 0x80E1
    }

    public enum Capability
    {
        CullFace = 0x0B44,
        DepthTest = 0x0B71
    }

    public enum PixelType
    {
        UnsignedByte = 0x1401
    }

    public enum ActiveTextureTarget
    {
        Texture0 = 0x84C0
    }

    public static class Gl
    {
        private const string LibraryName = "opengl32.dll";

        private static readonly Dictionary<string, Delegate> _extensionFunctions = new Dictionary<string, Delegate>();

        // SuppressUnmanagedCodeSecurity can be used by methods that want to call into native code without incurring the
        // Performance loss of a run-time security check when doing so. The stack walk performed when calling unmanaged code
        // is omitted at run time, resulting in substantial performance savings.
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint glCreateShader(uint shaderType);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glShaderSource(uint shader, uint count, string[] sources, IntPtr length);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glCompileShader(uint shader);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGetShaderiv(uint shader, uint parameterName, out int parameter);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGetShaderInfoLog(uint shader, uint maxLength, out uint length, StringBuilder infoLog);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint glCreateProgram();

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glAttachShader(uint program, uint shader);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glDetachShader(uint program, uint shader);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glLinkProgram(uint program);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glUseProgram(uint program);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGetProgramiv(uint program, uint parameterName, out int parameter);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGetProgramInfoLog(uint program, uint maxLength, out uint length, StringBuilder infoLog);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGenVertexArrays(uint amount, uint[] array);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glEnableVertexAttribArray(uint index);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glBindVertexArray(uint array);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGenBuffers(uint amount, uint[] buffers);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glBindBuffer(uint target, uint buffer);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glVertexAttribPointer(
            uint target, int size, uint type, byte normalized, uint stride, IntPtr pointer);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glBufferData(uint target, int size, IntPtr data, uint usageHint);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glDrawElements(uint mode, uint count, uint type, IntPtr indices);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int glGetUniformLocation(uint program, string name);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        private delegate void glUniformMatrix4fv(int location, uint count, byte tranpose, float[] value);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glUniform1i(int location, int value);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glUniform3fv(int location, uint count, float[] value);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGenTextures(uint amount, uint[] textures);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glBindTexture(uint target, uint texture);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glActiveTexture(uint texture);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGenerateMipmap(uint target);

        private static T InvokeExtensionFunction<T>() where T : class
        {
            // Get the type of the extension function
            Type delegateType = typeof(T);

            // Get the name of the extension function
            string name = delegateType.Name;

            // Does the dictionary contain our extension function?
            Delegate del = null;
            if (_extensionFunctions.TryGetValue(name, out del) == false)
            {
                // We haven't loaded it yet. Load it now
                IntPtr proc = wglGetProcAddress(name);

                if (proc == IntPtr.Zero)
                    throw new InvalidOperationException($"Extension function {name} is not supported! :(");

                // Get the delegate for the function pointer
                del = Marshal.GetDelegateForFunctionPointer(proc, delegateType);

                // Add to the dictionary
                _extensionFunctions.Add(name, del);
            }

            return del as T;
        }

        #region Standard Functions
        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr wglGetProcAddress(string name);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall, EntryPoint = "glClearColor")]
        public static extern void ClearColor(float red, float green, float blue, float alpha);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall, EntryPoint = "glClear")]
        public static extern void Clear(uint colorBitmask);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall, EntryPoint = "glGetError")]
        public static extern uint GetError();

        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibraryName, EntryPoint = "glEnable")]
        public static extern void Enable(uint capability);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibraryName, EntryPoint = "glTexParameteri")]
        public static extern void TexParameter(uint target, uint parameterName, int parameter);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibraryName, EntryPoint = "glTexParameterf")]
        public static extern void TexParameter(uint target, uint parameterName, float parameter);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibraryName, EntryPoint = "glPixelStorei")]
        public static extern void PixelStore(uint parameterName, int parameter);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibraryName, EntryPoint = "glTexImage2D")]
        private static extern void glTexImage2D(uint target, int level, int internalFormat, uint width, uint height,
            int border, uint format, uint type, IntPtr data);
        #endregion

        #region Extension Functions
        public static uint CreateShader(ShaderType shaderType)
        {
            return (uint)InvokeExtensionFunction<glCreateShader>()((uint)shaderType);
        }

        // TODO: Should totally make this method more convinient (only have shader + string parameter)
        public static void ShaderSource(uint shader, uint count, string[] sources, IntPtr length)
        {
            InvokeExtensionFunction<glShaderSource>()(shader, count, sources, length);
        }

        public static void CompileShader(uint shader)
        {
            InvokeExtensionFunction<glCompileShader>()(shader);
        }

        public static void GetShader(uint shader, ShaderParameter shaderParameter, out int parameter)
        {
            InvokeExtensionFunction<glGetShaderiv>()(shader, (uint) shaderParameter, out parameter);
        }

        /// <summary>
        /// When you have a C/C++ interface having to manipulate a fixed-length buffer (such as for
        /// Returning a string to the caller), you will have to use StringBuilder as the argument.
        /// Even if you pass a string by reference, there is no way to initialize the buffer to a given size on the
        /// Unmanaged side.
        /// 
        /// If you pass a StringBuilder, on the other hand, it can be deferenced and modified by the unmanaged code, provided
        /// That it doesn't exceed the capacity of the StringBuilder. A StringBuilder can be initialized to a fixed length,
        /// Which is exactly what you want for a fixed-length buffer.
        /// </summary>
        /// <param name="shader"></param>
        /// <param name="maxLength"></param>
        /// <param name="length"></param>
        /// <param name="infoLog"></param>
        public static void GetShaderInfoLog(uint shader, uint maxLength, out uint length, StringBuilder infoLog)
        {
            InvokeExtensionFunction<glGetShaderInfoLog>()(shader, maxLength, out length, infoLog);
        }

        public static uint CreateProgram()
        {
            return (uint)InvokeExtensionFunction<glCreateProgram>()();
        }

        public static void AttachShader(uint program, uint shader)
        {
            InvokeExtensionFunction<glAttachShader>()(program, shader);
        }

        public static void LinkProgram(uint program)
        {
            InvokeExtensionFunction<glLinkProgram>()(program);
        }

        public static void DetachShader(uint program, uint shader)
        {
            InvokeExtensionFunction<glDetachShader>()(program, shader);
        }

        public static void GetProgram(uint program, ProgramParameter programParameter, out int parameter)
        {
            InvokeExtensionFunction<glGetProgramiv>()(program, (uint) programParameter, out parameter);
        }

        public static void UseProgram(uint program)
        {
            InvokeExtensionFunction<glUseProgram>()(program);
        }

        public static void GetProgramInfoLog(uint program, uint maxLength, out uint length, StringBuilder infoLog)
        {
            InvokeExtensionFunction<glGetProgramInfoLog>()(program, maxLength, out length, infoLog);
        }

        public static void GenVertexArrays(uint amount, uint[] array)
        {
            InvokeExtensionFunction<glGenVertexArrays>()(amount, array);
        }

        public static void EnableVertexAttribArray(uint index)
        {
            InvokeExtensionFunction<glEnableVertexAttribArray>()(index);
        }

        public static void BindVertexArray(uint array)
        {
            InvokeExtensionFunction<glBindVertexArray>()(array);
        }

        public static void GenBuffers(uint amount, uint[] buffers)
        {
            InvokeExtensionFunction<glGenBuffers>()(amount, buffers);
        }

        public static void BindBuffer(BufferTarget bufferTarget, uint buffer)
        {
            InvokeExtensionFunction<glBindBuffer>()((uint) bufferTarget, buffer);
        }

        public static void VertexAttribPointer(
            uint index, int size, VertexAttribPointerDataType type, byte normalized, uint stride, IntPtr pointer)
        {
            InvokeExtensionFunction<glVertexAttribPointer>()(index, size, (uint)type, normalized, stride, pointer);
        }

        public static void BufferData(BufferTarget target, int size, float[] data, BufferUsageHint usageHint)
        {
            /*
             * A GCHandle provides a way to access a managed object from unmanaged memory.
             * 
             * When the handle is allocated, it can be used to prevent the managed object from
             * Being collected or moved by the garbage collector while an unmanaged client holds a reference to it.
             * 
             * The pinned handle type allows to return a memory address to prevent the garbage collector from moving
             * the object in memory.
             * 
             * You must explicitly release this handle ASAP by calling the Free() method, otherwise memory leaks could occur.
             */
            //GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            //IntPtr address = handle.AddrOfPinnedObject();
            //InvokeExtensionFunction<glBufferData>()((uint)target, size, address, (uint)usageHint);
            //handle.Free();

            IntPtr p = Marshal.AllocHGlobal(data.Length * sizeof(float));
            Marshal.Copy(data, 0, p, data.Length);
            InvokeExtensionFunction<glBufferData>()((uint) target, size, p, (uint) usageHint);
            Marshal.FreeHGlobal(p);
        }

        public static void BufferData(BufferTarget target, int size, uint[] data, BufferUsageHint usageHint)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr address = handle.AddrOfPinnedObject();
            InvokeExtensionFunction<glBufferData>()((uint)target, size, address, (uint) usageHint);
            handle.Free();
        }

        public static void DrawElements(DrawMode drawMode, uint count, DrawType type, IntPtr indices)
        {
            InvokeExtensionFunction<glDrawElements>()((uint) drawMode, count, (uint) type, indices);
        }

        public static int UniformLocation(uint program, string name)
        {
            return InvokeExtensionFunction<glGetUniformLocation>()(program, name);
        }

        public static void UniformMatrix4(int location, byte transpose, float[] value)
        {
            InvokeExtensionFunction<glUniformMatrix4fv>()(location, 1, transpose, value);
        }

        public static void Uniform1(int location, int value)
        {
            InvokeExtensionFunction<glUniform1i>()(location, value);
        }

        public static void Uniform3(int location, float[] value)
        {
            InvokeExtensionFunction<glUniform3fv>()(location, 1, value);
        }

        public static void GenTextures(uint amount, uint[] textures)
        {
            InvokeExtensionFunction<glGenTextures>()(amount, textures);
        }

        public static void BindTexture(TextureTarget target, uint texture)
        {
            InvokeExtensionFunction<glBindTexture>()((uint) target, texture);
        }

        public static void TexImage2D(TextureTarget target, int level, InternalPixelFormat internalFormat, uint width,
            uint height,
            int border, PixelFormat format, PixelType pixelType, IntPtr data)
        {
            glTexImage2D((uint) target, level, (int) internalFormat, width, height, border,
                (uint) format, (uint) pixelType, data);
        }

        public static void GenerateMipMap(TextureTarget target)
        {
            InvokeExtensionFunction<glGenerateMipmap>()((uint) target);
        }

        public static void ActiveTexture(ActiveTextureTarget activeTextureTarget)
        {
            InvokeExtensionFunction<glActiveTexture>()((uint) activeTextureTarget);
        }
        #endregion

        #region Utility Functions

        public static void CheckForError([CallerLineNumber] int linenumber = 0)
        {
            ErrorType error = (ErrorType) GetError();

            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            if (error != ErrorType.NoError)
            {
                Console.WriteLine($"The following OpenGL error occoured: {error.ToString()} on line number: {linenumber}");
                Console.WriteLine($"This happened in method: {sf.GetMethod().Name} in file: {sf.GetFileName()}");
            }
        }
        #endregion
    }
}