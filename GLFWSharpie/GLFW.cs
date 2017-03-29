using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GLFWSharpie
{
    /// <summary>
    /// 
    /// </summary>
    public enum GlfwWindowHint
    {
        ContextVersionMajor = 0x00022002,
        ContextVersionMinor = 0x00022003
    }

    /// <summary>
    /// 
    /// </summary>
    public static class Glfw
    {
        private const string LibraryName = "glfw3.dll";

        public enum KeyboardAction
        {
            Release = 0,
            Press = 1,
            Repeat = 2
        }

        public enum KeyboardKey
        {
            A = 65,
            D = 68,
            S = 83,
            W = 87
        }

        // Delegates
        // TODO: READ MORE UP ON MARSHALLED CALLBACKS
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void InputCallback(IntPtr window, int key, int scancode, int action, int mods);

        [DllImport(LibraryName, EntryPoint = "glfwInit")]
        public static extern int Init();

        [DllImport(LibraryName, EntryPoint = "glfwTerminate")]
        public static extern int Terminate();

        [DllImport(LibraryName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwCreateWindow")]
        public static extern IntPtr CreateWindow(int width, int height, string title, IntPtr monitor, IntPtr share);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSwapBuffers")]
        public static extern void SwapBuffers(IntPtr windowHandle);

        [DllImport(LibraryName, EntryPoint = "glfwPollEvents")]
        public static extern void PollEvents();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwWindowShouldClose")]
        public static extern int WindowShouldClose(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwWindowHint")]
        public static extern void WindowHint(int hint, int value);

        /// <summary>
        /// If the native functions return strings, such as "const char*", you must use IntPtr for the
        /// Return type, NOT string!
        /// 
        /// The CLR assumes the following two things about a PInvoke which returns a string type:
        /// - The native memory needs to be freed
        /// - The native memory was allocated with CoTaskMemAlloc
        /// 
        /// So, if you specify the return type as "string", the CLR will simply call CoTaskMemFree on the native
        /// Memory blob... which will at best crash your application if the native library does not expect your application
        /// To free it! Which it almost certainly does not.
        /// </summary>
        /// <returns></returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "glfwGetVersionString")]
        public static extern IntPtr GetVersionString();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetWindowAttrib")]
        public static extern int GetWindowAttrib(IntPtr window, int attribute);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwMakeContextCurrent")]
        public static extern void MakeContextCurrent(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetKeyCallback")]
        public static extern void SetKeyCallback(IntPtr window, IntPtr callback);
    }
}