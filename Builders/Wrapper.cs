using System.Text;

namespace WrapperGenerator
{
    class Wrapper : IWrapperBuilder
    {
        public string Name => "Wrapper";

        public string BuildWrapper(string Name, Function[] Exports)
        {
            Exports.SetAnonType(true);

            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine("using System;");
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

            foreach (Function Export in Exports)
            {
                Builder.AppendLine($"        [DllImport(\"{Name.Trim().Replace(" ", "")}.dll\", CallingConvention = CallingConvention.{Export.Calling})]");
                Builder.AppendLine($"        public static extern {Export};");
                Builder.AppendLine();
            }

            Builder.AppendLine("    }");
            Builder.AppendLine("}");

            return Builder.ToString();
        }
    }
}
