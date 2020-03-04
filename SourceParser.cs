using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace WrapperGenerator
{
    internal class SourceParser
    {
        string[] Lines;
        public SourceParser(string[] Lines)
        {
            this.Lines = Lines;
        }

        public Function[] Parse()
        {
            bool Commented = false;
            List<Function> Functions = new List<Function>();
            for (int i = 0; i < Lines.Length; i++)
            {
                string Line = Lines[i].Trim();

                if (Line.Contains("*/"))
                {
                    Commented = false;
                    Line = Line.Substring(Line.IndexOf("*/")).Trim();
                }

                if (Commented)
                    continue;

                if (Line.Contains("/*"))
                {
                    Commented = true;
                    Line = Line.Substring(0, Line.IndexOf("/*")).Trim();
                }
                
                if (Line.Contains("//"))
                    Line = Line.Substring(0, Line.IndexOf("//")).Trim();
                if (string.IsNullOrWhiteSpace(Line))
                    continue;
                if (Line.EndsWith(";"))
                    continue;
                if (!Line.Contains("(") || !Line.EndsWith(")"))
                    continue;
                if (Line.StartsWith("#"))
                    continue;
                if (Line.StartsWith("&& ") || Line.StartsWith("|| "))
                    continue;
                if (Line.EndsWith(",") && (!Line.Contains(" ") || Line.Contains("\"")))
                    continue;
                if (Line.Contains("="))
                    continue;
                //int __thiscall sub_100BB7B0(int this, LPVOID lpBuffer, int a3, int a4, LPDWORD lpNumberOfBytesRead)
                try
                {
                    Function FuncInfo = new Function();
                    FuncInfo.Line = i;
                    FuncInfo.Type = Line.Substring(0, Line.IndexOf("("));
                    if (FuncInfo.Calling == null)
                        continue;

                    FuncInfo.Name = FuncInfo.Type.Split(' ').Last();
                    FuncInfo.Type = FuncInfo.Type.Substring(0, FuncInfo.Type.LastIndexOf(" "));

                    while (FuncInfo.Name.StartsWith("*")) {
                        FuncInfo.Type += '*';
                        FuncInfo.Name = FuncInfo.Name.Substring(1);
                    }

                    FuncInfo.Arguments = ParseArguments(Line.Substring(Line.IndexOf("(")));

                    if (string.IsNullOrWhiteSpace(FuncInfo.Name))
                        continue;

                    Functions.Add(FuncInfo);
                }
                catch {
                    continue;
                }
            }

            return Functions.ToArray();
        }

        private Argument[] ParseArguments(string Source)
        {
            //(int this, LPVOID lpBuffer, int a3, int a4, LPDWORD lpNumberOfBytesRead)
            if (Source.StartsWith("(") && Source.EndsWith(")"))
                Source = Source.Substring(1, Source.Length - 2);
            List<Argument> Arguments = new List<Argument>();
            Argument Arg = new Argument();

            int Group = 0;
            string Buffer = string.Empty;
            foreach (char c in Source)
            {
                switch (c)
                {
                    case ' ':
                        Arg.Type += Buffer + c;
                        Buffer = string.Empty;
                        break;
                    case ',':
                        if (Group != 0)
                            goto default;
                        CloseArg(ref Arg, Buffer);
                        Buffer = string.Empty;
                        Arguments.Add(Arg);
                        Arg = new Argument();
                        break;
                    case '(':
                        Group++;
                        goto default;
                    case ')':
                        Group--;
                        goto default;
                    default:;
                        Buffer += c;
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(Buffer))
            {
                CloseArg(ref Arg, Buffer);
                Arguments.Add(Arg);
            }


            return Arguments.ToArray();
        }

        private void CloseArg(ref Argument Arg, string Buffer)
        {
                Arg.Type = Arg.Type.Trim();
                Arg.Name = Buffer;

            while (Arg.Name.StartsWith('*'))
            {
                Arg.Type += '*';
                Arg.Name = Arg.Name.Substring(1);
            }
        }
    }

    struct Function
    {
        public bool Unsafe;
        public bool AnonType;
        public int Line;
        public string Type;
        public string Name;

        public string ReturnType { 
            get {
                if (Type.ToLower().Contains("void"))
                    return "void";

                if (!AnonType)
                {
                    if (Type.ToLower().Contains("bool"))
                        return "bool";
                    if (Type.ToLower().Contains("signed int"))
                        return "int";
                    if (Type.ToLower().Contains("int"))
                        return "uint";
                    if (Type.ToLower().Contains("dword"))
                        return "uint";
                }

                return Unsafe ? "void*" : "IntPtr";
            }
        }

        public string ArgumentNames { 
            get {
                var Str = string.Empty;
                foreach (var Arg in Arguments)
                {
                    Str += $"{Arg.Name}, ";
                }
                return Str.TrimEnd(' ', ',');
            }
        }
        public CallingConvention? Calling {
            get {
                if (Type.ToLower().Contains("stdcall"))
                    return CallingConvention.StdCall;
                if (Type.ToLower().Contains("cdecl"))
                    return CallingConvention.Cdecl;
                if (Type.ToLower().Contains("thiscall"))
                    return CallingConvention.ThisCall;
                if (Type.ToLower().Contains("fastcall"))
                    return CallingConvention.FastCall;
                if (Type.ToLower().Contains("winapi"))
                    return CallingConvention.Winapi;
                return CallingConvention.StdCall;
            }
        }

        public string Charset {
            get
            {
                string Charset = "Auto";
                if (Name.EndsWith("A"))
                    Charset = "Ansi";
                if (Name.EndsWith("W"))
                    Charset = "Unicode";
                return Charset;
            }
        }

        public Argument[] Arguments;

        public override string ToString()
        {
            string Args = string.Empty;
            foreach (var Arg in Arguments)
            {
                Args += $"{Arg.ReturnType} {Arg.Name}, ";
            }

            Args = Args.TrimEnd(' ', ',');
            return (Unsafe ? "unsafe " : "") + $"{ReturnType} {Name}({Args})";
        }
    }

    struct Argument
    {
        public bool Unsafe;
        public bool AnonType;

        public string Name;
        public string Type;

        public string ReturnType {
            get {
                if (!AnonType)
                {
                    if (Type.ToLower().Contains("bool"))
                        return "bool";
                    if (Type.ToLower().Contains("signed int"))
                        return "int";
                    if (Type.ToLower().Contains("int"))
                        return "uint";
                    if (Type.ToLower().Contains("dword"))
                        return "uint";
                }

                return Unsafe ? "void*" : "IntPtr";
            }
        }
    }
}
