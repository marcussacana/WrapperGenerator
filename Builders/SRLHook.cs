using System;
using System.Collections.Generic;
using System.Text;

namespace WrapperGenerator.Builders
{
    class SRLHook : IWrapperBuilder
    {
        public string Name => "SRL Hook";

        public string BuildWrapper(string Name, Function[] Exports)
        {
            for (int i = 0; i < Exports.Length; i++) {
                Exports[i].AnonType = true;
                Exports[i].Unsafe = true;
                for (int x = 0; x < Exports[i].Arguments.Length; x++)
                {
                    Exports[i].Arguments[x].Unsafe = true;
                    Exports[i].Arguments[x].AnonType = true;
                }
            }

            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine("namespace StringReloads.Hook");
            Builder.AppendLine("{");
            for (int i = 0; i < Exports.Length; i++)
            {
                var Export = Exports[i];
                Builder.AppendLine($"    public unsafe class {Export.Name} : Base.Hook<{Export.Name}Delegate>");
                Builder.AppendLine("    {");
                Builder.AppendLine($"        public override string Library => \"{Name}.dll\";");
                Builder.AppendLine();
                Builder.AppendLine($"        public override string Export => \"{Export.Name}\";");
                Builder.AppendLine();
                Builder.AppendLine("        public override void Initialize()");
                Builder.AppendLine("        {");
                
                string ExportName = Export.Name;                
                Export.Name += "Hook";
                Builder.AppendLine($"            HookDelegate = new {Export.Name}Delegate({Export.Name});");
                Builder.AppendLine("            Compile();");
                Builder.AppendLine("        }");
                Builder.AppendLine();
                Builder.AppendLine($"        private {Export}");
                Builder.AppendLine("        {");
                Builder.AppendLine($"            return Bypass({Export.ArgumentNames});");
                Builder.AppendLine("        }");
                Builder.AppendLine();                
                Export.Name = ExportName;
                Builder.AppendLine($"        [UnmanagedFunctionPointer(CallingConvention.{Export.Calling}, CharSet = CharSet.{Export.Charset})]");
                Export.Name += "Delegate";
                Builder.AppendLine($"        public delegate {Export};");
                Builder.AppendLine("    }");
                Builder.AppendLine();
                Export.Name = ExportName;
            }
            Builder.AppendLine("}");

            return Builder.ToString();
        }
    }
}
