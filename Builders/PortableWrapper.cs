using System.Text;

namespace WrapperGenerator
{
    class PortableWrapper : IWrapperBuilder
    {
        public string Name => "Portable Wrapper";

        public string BuildWrapper(string Name, Function[] Exports)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine("using System;");
            Builder.AppendLine("using System.IO;");
            Builder.AppendLine("using System.Runtime.InteropServices;");
            Builder.AppendLine();
            Builder.AppendLine("namespace Wrapper");
            Builder.AppendLine("{");
            Builder.AppendLine("    /// <summary>");
            Builder.AppendLine($"    /// This is a wrapper to the {Name}.dll");
            Builder.AppendLine("    /// </summary>");
            Builder.AppendLine($"    public static class {Name.Trim().Replace(" ", "")}");
            Builder.AppendLine("    {");
            Builder.AppendLine();
            Builder.AppendLine("        static string CurrentDllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);");
            Builder.AppendLine("        static string RealDllPath  = null;");
            Builder.AppendLine("        static bool WOW64 => !Environment.Is64BitProcess && Environment.Is64BitOperatingSystem;");
            Builder.AppendLine();
            Builder.AppendLine("        public static IntPtr RealHandler;");
            Builder.AppendLine("        public static void LoadRetail()");
            Builder.AppendLine("        {");
            Builder.AppendLine("            if (RealHandler != IntPtr.Zero)");
            Builder.AppendLine("                return;");
            Builder.AppendLine();
            Builder.AppendLine("            RealHandler = LoadLibrary(CurrentDllName);");
            Builder.AppendLine();
            Builder.AppendLine("            if (RealHandler == IntPtr.Zero)");
            Builder.AppendLine("                Environment.Exit(0x505);//ERROR_DELAY_LOAD_FAILED");

            Builder.AppendLine();
            foreach (Function Export in Exports)
            {
                Builder.AppendLine($"            d{Export.Name} = GetDelegate<t{Export.Name}>(RealHandler, \"{Export.Name}\", false);");
            }
            Builder.AppendLine();
            Builder.AppendLine("        }");

            Builder.AppendLine();

            foreach (Function Export in Exports)
            {
                Builder.AppendLine($"        [DllExport(CallingConvention = CallingConvention.{Export.Calling})]");
                Builder.AppendLine($"        public static {Export.ToString()}");
                Builder.AppendLine("        {");
                Builder.AppendLine("            LoadRetail();");
                Builder.AppendLine($"            return d{Export.Name}({Export.ArgumentNames});");
                Builder.AppendLine("        }");
                Builder.AppendLine();
            }

            Builder.AppendLine();


            Builder.AppendLine("        [DllImport(\"kernel32\", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]");
            Builder.AppendLine("        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);");
            Builder.AppendLine();
            Builder.AppendLine("        [DllImport(\"kernel32\", SetLastError = true, CharSet = CharSet.Unicode)]");
            Builder.AppendLine("        internal static extern IntPtr LoadLibraryW(string lpFileName);");
            Builder.AppendLine();
            Builder.AppendLine("        internal static IntPtr LoadLibrary(string lpFileName)");
            Builder.AppendLine("        {");
            Builder.AppendLine("            string DllPath = lpFileName;");
            Builder.AppendLine("            if (lpFileName.Length < 2 || lpFileName[1] != ':')");
            Builder.AppendLine("            {");
            Builder.AppendLine("                string DLL = Path.GetFileNameWithoutExtension(lpFileName);");
            Builder.AppendLine("                DllPath = Path.Combine(Environment.CurrentDirectory, $\"{DLL}_ori.dll\");");
            Builder.AppendLine("                if (!File.Exists(DllPath) && CurrentDllName != lpFileName.ToLower())");
            Builder.AppendLine("                    DllPath = Path.Combine(Environment.CurrentDirectory, $\"{DLL}.dll\");");
            Builder.AppendLine("                if (!File.Exists(DllPath) && CurrentDllName != lpFileName.ToLower())");
            Builder.AppendLine("                    DllPath = Path.Combine(CurrentDllPath, $\"{DLL}_ori.dll\");");
            Builder.AppendLine("                if (!File.Exists(DllPath) && CurrentDllName != lpFileName.ToLower())");
            Builder.AppendLine("                    DllPath = Path.Combine(CurrentDllPath, $\"{DLL}.dll.ori\");");
            Builder.AppendLine("                if (!File.Exists(DllPath))");
            Builder.AppendLine("                {");
            Builder.AppendLine("                    DllPath = WOW64 ? Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) : Environment.SystemDirectory;");
            Builder.AppendLine("                    DllPath = Path.Combine(DllPath, $\"{DLL}.dll\");");
            Builder.AppendLine("                }");
            Builder.AppendLine("            }");
            Builder.AppendLine("            RealDllPath = DllPath;");
            Builder.AppendLine();
            Builder.AppendLine("            IntPtr Handler = LoadLibraryW(DllPath);");
            Builder.AppendLine();
            Builder.AppendLine("            if (Handler == IntPtr.Zero)");
            Builder.AppendLine("                Environment.Exit(0x505);//ERROR_DELAY_LOAD_FAILED");
            Builder.AppendLine();
            Builder.AppendLine("            return Handler;");
            Builder.AppendLine("        }");
            Builder.AppendLine();
            Builder.AppendLine("        internal static T GetDelegate<T>(IntPtr Handler, string Function, bool Optional = true) where T : Delegate");
            Builder.AppendLine("        {");
            Builder.AppendLine("            IntPtr Address = GetProcAddress(Handler, Function);");
            Builder.AppendLine("            if (Address == IntPtr.Zero)");
            Builder.AppendLine("            {");
            Builder.AppendLine("                if (Optional)");
            Builder.AppendLine("                {");
            Builder.AppendLine("                    return null;");
            Builder.AppendLine("                }");
            Builder.AppendLine("                Environment.Exit(0x505);//ERROR_DELAY_LOAD_FAILED");
            Builder.AppendLine("            }");
            Builder.AppendLine("            return (T)Marshal.GetDelegateForFunctionPointer(Address, typeof(T));");
            Builder.AppendLine("        }");

            Builder.AppendLine();



            foreach (Function Export in Exports)
            {

                Builder.AppendLine($"        static t{Export.Name} d{Export.Name};");
            }

            Builder.AppendLine();
            foreach (Function Export in Exports)
            {
                Function tmp = Export;
                tmp.Name = "t" + tmp.Name;
                Builder.AppendLine($"        delegate {tmp};");
            }

            Builder.AppendLine();
            Builder.AppendLine("    }");
            Builder.AppendLine("}");

            return Builder.ToString();
        }
    }

}
