using System.Text;

namespace WrapperGenerator
{
    class SRLWrapper : IWrapperBuilder
    {
        public string Name => "SRL Wrapper";

        public string BuildWrapper(string Name, Function[] Exports)
        {
            for (int i = 0; i < Exports.Length; i++) {
                Exports[i].AnonType = true;
                for (int x = 0; x < Exports[i].Arguments.Length; x++) {
                    Exports[i].Arguments[x].AnonType = true;
                }
            }

            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine("using System;");
            Builder.AppendLine("using System.Runtime.InteropServices;");
            Builder.AppendLine("using SRLWrapper.Wrapper.Base;");
            Builder.AppendLine("using static SRLWrapper.Wrapper.Base.Wrapper;");
            Builder.AppendLine();
            Builder.AppendLine("namespace SRLWrapper.Wrapper");
            Builder.AppendLine("{");
            Builder.AppendLine("    /// <summary>");
            Builder.AppendLine($"    /// This is a wrapper to the {Name.ToUpperInvariant()}.dll");
            Builder.AppendLine("    /// </summary>");
            Builder.AppendLine($"    public unsafe static class {Name.Trim().Replace(" ", "")}");
            Builder.AppendLine("    {");
            Builder.AppendLine("        public static void* RealHandler;");
            Builder.AppendLine("        public static void LoadRetail()");
            Builder.AppendLine("        {");
            Builder.AppendLine("            if (RealHandler != null)");
            Builder.AppendLine("                return;");
            Builder.AppendLine();
            Builder.AppendLine("            RealHandler = LoadLibrary(CurrentDllName);");
            Builder.AppendLine();
            Builder.AppendLine("            if (RealHandler == null)");
            Builder.AppendLine("                Environment.Exit(0x505);//ERROR_DELAY_LOAD_FAILED");
            
            Builder.AppendLine();            
            foreach (Function Export in Exports)
            {
                Builder.AppendLine($"            d{Export.Name} = GetDelegate<RET_{Export.Arguments.Length}>(RealHandler, \"{Export.Name}\", false);");
            }
            Builder.AppendLine();

            Builder.AppendLine("            InitializeSRL();");
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

            foreach (Function Export in Exports)
            {
                Builder.AppendLine($"        static RET_{Export.Arguments.Length} d{Export.Name};");
            }
            Builder.AppendLine();
            Builder.AppendLine("    }");
            Builder.AppendLine("}");

            return Builder.ToString();
        }
    }
}
