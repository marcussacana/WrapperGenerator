using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                
                if (Line.EndsWith(";") && !(Line.Contains("__stdcall") || Line.Contains("__fastcall") || Line.Contains("__thiscall")))
                    continue;

                if (Line.EndsWith(";"))
                    Line = Line.Substring(0, Line.Length - 1);
                
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

                    while (FuncInfo.Name.StartsWith("*"))
                    {
                        FuncInfo.Type += '*';
                        FuncInfo.Name = FuncInfo.Name.Substring(1);
                    }

                    if (FuncInfo.Name.Contains("@<"))
                        FuncInfo.Name = FuncInfo.Name.Substring(0, FuncInfo.Name.IndexOf("@<"));

                    FuncInfo.Arguments = ParseArguments(Line.Substring(Line.IndexOf("(")));

                    if (string.IsNullOrWhiteSpace(FuncInfo.Name))
                        continue;

                    Functions.Add(FuncInfo);
                }
                catch
                {
                    continue;
                }
            }

            List<Function> Additionals = new List<Function>();
            foreach (var Function in Functions)
            {
                if (Function.Name.Contains("Stub") && !Functions.Where(x => Function.Name.Replace("Stub", "") == x.Name).Any())
                {
                    Additionals.Add(new Function()
                    {
                        AnonType =  Function.AnonType,
                        Arguments =  Function.Arguments,
                        Line =  Function.Line,
                        Name =  Function.Name.Replace("Stub", ""),
                        Type =  Function.Type,
                        Unsafe =  Function.Unsafe
                    });
                }
            }

            return Functions.Concat(Additionals).GroupBy(x=>x.Name).Select(grp => grp.First()).ToArray();
        }

        private Argument[] ParseArguments(string Source)
        {
            //(__int64 (***a1)(void))
            //(__int64 (__fastcall ***a1)(_QWORD, signed __int64))
            //(__int64 a1@<rdx>, __int64 a2@<rcx>, float *a3@<r8>, double a4@<xmm0>)
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
                        if (Group != 0)
                            goto default;
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
                    default:
                        ;
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

            if (Arg.Name.Contains(")("))//Func Argument
            {
                Arg.Type = "void*";
                Arg.Name = Arg.Name.Substring(0, Arg.Name.IndexOf(")("));
                Arg.Name = Arg.Name.Substring(Arg.Name.LastIndexOf("(") + 1);//__fastcall ***a1
                Arg.Name = Arg.Name.Split(' ').Last();
            }

            while (Arg.Name.StartsWith('*'))//Ptr Argument
            {
                Arg.Type += '*';
                Arg.Name = Arg.Name.Substring(1);
            }

            if (Arg.Name.Contains("@<"))//FastCall Register
                Arg.Name = Arg.Name.Substring(0, Arg.Name.IndexOf("@<"));
        }
    }

    struct Function
    {
        public bool Unsafe;
        public bool AnonType;
        public int Line;
        public string Type;
        public string Name;

        public string ReturnType
        {
            get
            {
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

        public string ArgumentNames
        {
            get
            {
                var Str = string.Empty;
                foreach (var Arg in Arguments)
                {
                    Str += $"{Arg.Name}, ";
                }
                return Str.TrimEnd(' ', ',');
            }
        }
        public CallingConvention? Calling
        {
            get
            {
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

        public string Charset
        {
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

        public string ReturnType
        {
            get
            {
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
