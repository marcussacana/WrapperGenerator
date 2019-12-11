# WrapperGenerator
My utility to create a C# dll wrapper class (Using DLLExport) automatically with help of the IDA PRO

## By Marcussacana

# Sample genareted code
```csharp
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Wrapper
{
    /// <summary>
    /// This is a wrapper to the d3d10.dll
    /// </summary>
    public static class d3d10
    {

        static string CurrentDllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static string RealDllPath  = null;
        static bool WOW64 => !Environment.Is64BitProcess && Environment.Is64BitOperatingSystem;

        public static IntPtr RealHandler;
        public static void LoadRetail()
        {
            if (RealHandler != IntPtr.Zero)
                return;
            RealHandler = LoadLibrary(CurrentDllName);
            if (RealHandler == IntPtr.Zero)
                Environment.Exit(0x505);//ERROR_DELAY_LOAD_FAILED

            dD3D10CreateDevice = GetDelegate<tD3D10CreateDevice>(RealHandler, "D3D10CreateDevice", false);
            dD3D10CreateDeviceAndSwapChain = GetDelegate<tD3D10CreateDeviceAndSwapChain>(RealHandler, "D3D10CreateDeviceAndSwapChain", false);
            dD3D10CreateBlob = GetDelegate<tD3D10CreateBlob>(RealHandler, "D3D10CreateBlob", false);
            dD3D10CompileShader = GetDelegate<tD3D10CompileShader>(RealHandler, "D3D10CompileShader", false);
            dD3D10GetPixelShaderProfile = GetDelegate<tD3D10GetPixelShaderProfile>(RealHandler, "D3D10GetPixelShaderProfile", false);
            dD3D10GetVertexShaderProfile = GetDelegate<tD3D10GetVertexShaderProfile>(RealHandler, "D3D10GetVertexShaderProfile", false);
            dD3D10GetGeometryShaderProfile = GetDelegate<tD3D10GetGeometryShaderProfile>(RealHandler, "D3D10GetGeometryShaderProfile", false);
            dD3D10GetShaderDebugInfo = GetDelegate<tD3D10GetShaderDebugInfo>(RealHandler, "D3D10GetShaderDebugInfo", false);
            dD3D10PreprocessShader = GetDelegate<tD3D10PreprocessShader>(RealHandler, "D3D10PreprocessShader", false);
            dD3D10GetInputSignatureBlob = GetDelegate<tD3D10GetInputSignatureBlob>(RealHandler, "D3D10GetInputSignatureBlob", false);
            dD3D10GetOutputSignatureBlob = GetDelegate<tD3D10GetOutputSignatureBlob>(RealHandler, "D3D10GetOutputSignatureBlob", false);
            dD3D10GetInputAndOutputSignatureBlob = GetDelegate<tD3D10GetInputAndOutputSignatureBlob>(RealHandler, "D3D10GetInputAndOutputSignatureBlob", false);
            dD3D10CreateEffectFromMemory = GetDelegate<tD3D10CreateEffectFromMemory>(RealHandler, "D3D10CreateEffectFromMemory", false);
            dD3D10CreateEffectPoolFromMemory = GetDelegate<tD3D10CreateEffectPoolFromMemory>(RealHandler, "D3D10CreateEffectPoolFromMemory", false);
            dD3D10CompileEffectFromMemory = GetDelegate<tD3D10CompileEffectFromMemory>(RealHandler, "D3D10CompileEffectFromMemory", false);
            dD3D10ReflectShader = GetDelegate<tD3D10ReflectShader>(RealHandler, "D3D10ReflectShader", false);
            dD3D10DisassembleShader = GetDelegate<tD3D10DisassembleShader>(RealHandler, "D3D10DisassembleShader", false);
            dD3D10DisassembleEffect = GetDelegate<tD3D10DisassembleEffect>(RealHandler, "D3D10DisassembleEffect", false);
            dD3D10CreateStateBlock = GetDelegate<tD3D10CreateStateBlock>(RealHandler, "D3D10CreateStateBlock", false);
            dD3D10StateBlockMaskUnion = GetDelegate<tD3D10StateBlockMaskUnion>(RealHandler, "D3D10StateBlockMaskUnion", false);
            dD3D10StateBlockMaskIntersect = GetDelegate<tD3D10StateBlockMaskIntersect>(RealHandler, "D3D10StateBlockMaskIntersect", false);
            dD3D10StateBlockMaskDifference = GetDelegate<tD3D10StateBlockMaskDifference>(RealHandler, "D3D10StateBlockMaskDifference", false);
            dD3D10StateBlockMaskEnableCapture = GetDelegate<tD3D10StateBlockMaskEnableCapture>(RealHandler, "D3D10StateBlockMaskEnableCapture", false);
            dD3D10StateBlockMaskDisableCapture = GetDelegate<tD3D10StateBlockMaskDisableCapture>(RealHandler, "D3D10StateBlockMaskDisableCapture", false);
            dD3D10StateBlockMaskEnableAll = GetDelegate<tD3D10StateBlockMaskEnableAll>(RealHandler, "D3D10StateBlockMaskEnableAll", false);
            dD3D10StateBlockMaskDisableAll = GetDelegate<tD3D10StateBlockMaskDisableAll>(RealHandler, "D3D10StateBlockMaskDisableAll", false);
            dD3D10StateBlockMaskGetSetting = GetDelegate<tD3D10StateBlockMaskGetSetting>(RealHandler, "D3D10StateBlockMaskGetSetting", false);

        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10CreateDevice(IntPtr pAdapter, IntPtr DriverType, IntPtr Software, uint Flags, uint SDKVersion, IntPtr ppDevice)
        {
            LoadRetail();
            return dD3D10CreateDevice(pAdapter, DriverType, Software, Flags, SDKVersion, ppDevice);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10CreateDeviceAndSwapChain(IntPtr pAdapter, IntPtr DriverType, IntPtr Software, uint Flags, uint SDKVersion, IntPtr pSwapChainDesc, IntPtr ppSwapChain, IntPtr ppDevice)
        {
            LoadRetail();
            return dD3D10CreateDeviceAndSwapChain(pAdapter, DriverType, Software, Flags, SDKVersion, pSwapChainDesc, ppSwapChain, ppDevice);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10CreateBlob(IntPtr NumBytes, IntPtr ppBuffer)
        {
            LoadRetail();
            return dD3D10CreateBlob(NumBytes, ppBuffer);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10CompileShader(IntPtr pSrcData, IntPtr SrcDataLen, IntPtr pFileName, IntPtr pDefines, IntPtr pInclude, IntPtr pFunctionName, IntPtr pProfile, uint Flags, IntPtr ppShader, IntPtr ppErrorMsgs)
        {
            LoadRetail();
            return dD3D10CompileShader(pSrcData, SrcDataLen, pFileName, pDefines, pInclude, pFunctionName, pProfile, Flags, ppShader, ppErrorMsgs);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10GetPixelShaderProfile(IntPtr pDevice)
        {
            LoadRetail();
            return dD3D10GetPixelShaderProfile(pDevice);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10GetVertexShaderProfile(IntPtr pDevice)
        {
            LoadRetail();
            return dD3D10GetVertexShaderProfile(pDevice);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10GetGeometryShaderProfile(IntPtr pDevice)
        {
            LoadRetail();
            return dD3D10GetGeometryShaderProfile(pDevice);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10GetShaderDebugInfo(IntPtr pShaderBytecode, IntPtr BytecodeLength, IntPtr ppDebugInfo)
        {
            LoadRetail();
            return dD3D10GetShaderDebugInfo(pShaderBytecode, BytecodeLength, ppDebugInfo);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10PreprocessShader(IntPtr pSrcData, IntPtr SrcDataSize, IntPtr pFileName, IntPtr pDefines, IntPtr pInclude, IntPtr ppShaderText, IntPtr ppErrorMsgs)
        {
            LoadRetail();
            return dD3D10PreprocessShader(pSrcData, SrcDataSize, pFileName, pDefines, pInclude, ppShaderText, ppErrorMsgs);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10GetInputSignatureBlob(IntPtr pShaderBytecode, IntPtr BytecodeLength, IntPtr ppSignatureBlob)
        {
            LoadRetail();
            return dD3D10GetInputSignatureBlob(pShaderBytecode, BytecodeLength, ppSignatureBlob);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10GetOutputSignatureBlob(IntPtr pShaderBytecode, IntPtr BytecodeLength, IntPtr ppSignatureBlob)
        {
            LoadRetail();
            return dD3D10GetOutputSignatureBlob(pShaderBytecode, BytecodeLength, ppSignatureBlob);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10GetInputAndOutputSignatureBlob(IntPtr pShaderBytecode, IntPtr BytecodeLength, IntPtr ppSignatureBlob)
        {
            LoadRetail();
            return dD3D10GetInputAndOutputSignatureBlob(pShaderBytecode, BytecodeLength, ppSignatureBlob);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10CreateEffectFromMemory(IntPtr pData, IntPtr DataLength, uint FXFlags, IntPtr pDevice, IntPtr pEffectPool, IntPtr ppEffect)
        {
            LoadRetail();
            return dD3D10CreateEffectFromMemory(pData, DataLength, FXFlags, pDevice, pEffectPool, ppEffect);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10CreateEffectPoolFromMemory(IntPtr pData, IntPtr DataLength, uint FXFlags, IntPtr pDevice, IntPtr ppEffectPool)
        {
            LoadRetail();
            return dD3D10CreateEffectPoolFromMemory(pData, DataLength, FXFlags, pDevice, ppEffectPool);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10CompileEffectFromMemory(IntPtr pData, IntPtr DataLength, IntPtr pSrcFileName, IntPtr pDefines, IntPtr pInclude, uint HLSLFlags, uint FXFlags, IntPtr ppCompiledEffect, IntPtr ppErrors)
        {
            LoadRetail();
            return dD3D10CompileEffectFromMemory(pData, DataLength, pSrcFileName, pDefines, pInclude, HLSLFlags, FXFlags, ppCompiledEffect, ppErrors);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10ReflectShader(IntPtr pShaderBytecode, IntPtr BytecodeLength, IntPtr ppReflector)
        {
            LoadRetail();
            return dD3D10ReflectShader(pShaderBytecode, BytecodeLength, ppReflector);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10DisassembleShader(IntPtr pShader, IntPtr BytecodeLength, bool EnableColorCode, IntPtr pComments, IntPtr ppDisassembly)
        {
            LoadRetail();
            return dD3D10DisassembleShader(pShader, BytecodeLength, EnableColorCode, pComments, ppDisassembly);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10DisassembleEffect(IntPtr pEffect, bool EnableColorCode, IntPtr ppDisassembly)
        {
            LoadRetail();
            return dD3D10DisassembleEffect(pEffect, EnableColorCode, ppDisassembly);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10CreateStateBlock(IntPtr pDevice, IntPtr pStateBlockMask, IntPtr ppStateBlock)
        {
            LoadRetail();
            return dD3D10CreateStateBlock(pDevice, pStateBlockMask, ppStateBlock);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10StateBlockMaskUnion(IntPtr pA, IntPtr pB, IntPtr pResult)
        {
            LoadRetail();
            return dD3D10StateBlockMaskUnion(pA, pB, pResult);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10StateBlockMaskIntersect(IntPtr pA, IntPtr pB, IntPtr pResult)
        {
            LoadRetail();
            return dD3D10StateBlockMaskIntersect(pA, pB, pResult);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10StateBlockMaskDifference(IntPtr pA, IntPtr pB, IntPtr pResult)
        {
            LoadRetail();
            return dD3D10StateBlockMaskDifference(pA, pB, pResult);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10StateBlockMaskEnableCapture(IntPtr pMask, IntPtr StateType, uint RangeStart, uint RangeLength)
        {
            LoadRetail();
            return dD3D10StateBlockMaskEnableCapture(pMask, StateType, RangeStart, RangeLength);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10StateBlockMaskDisableCapture(IntPtr pMask, IntPtr StateType, uint RangeStart, uint RangeLength)
        {
            LoadRetail();
            return dD3D10StateBlockMaskDisableCapture(pMask, StateType, RangeStart, RangeLength);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10StateBlockMaskEnableAll(IntPtr pMask)
        {
            LoadRetail();
            return dD3D10StateBlockMaskEnableAll(pMask);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static IntPtr D3D10StateBlockMaskDisableAll(IntPtr pMask)
        {
            LoadRetail();
            return dD3D10StateBlockMaskDisableAll(pMask);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static bool D3D10StateBlockMaskGetSetting(IntPtr pMask, IntPtr StateType, uint Entry)
        {
            LoadRetail();
            return dD3D10StateBlockMaskGetSetting(pMask, StateType, Entry);
        }


        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr LoadLibraryW(string lpFileName);

        internal static IntPtr LoadLibrary(string lpFileName)
        {
            string DllPath = lpFileName;
            if (lpFileName.Length < 2 || lpFileName[1] != ':')
            {
                string DLL = Path.GetFileNameWithoutExtension(lpFileName);
                DllPath = Path.Combine(Environment.CurrentDirectory, $"{DLL}_ori.dll");
                if (!File.Exists(DllPath) && CurrentDllName != lpFileName.ToLower())
                    DllPath = Path.Combine(Environment.CurrentDirectory, $"{DLL}.dll");
                if (!File.Exists(DllPath) && CurrentDllName != lpFileName.ToLower())
                    DllPath = Path.Combine(CurrentDllPath, $"{DLL}_ori.dll");
                if (!File.Exists(DllPath) && CurrentDllName != lpFileName.ToLower())
                    DllPath = Path.Combine(CurrentDllPath, $"{DLL}.dll.ori");
                if (!File.Exists(DllPath))
                {
                    DllPath = WOW64 ? Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) : Environment.SystemDirectory;
                    DllPath = Path.Combine(DllPath, $"{DLL}.dll");
                }
            }
            RealDllPath = DllPath;
            //System.Windows.Forms.MessageBox.Show("Mod: " + DllPath);
            IntPtr Handler = LoadLibraryW(DllPath);
            if (Handler == IntPtr.Zero)
                Environment.Exit(0x505);//ERROR_DELAY_LOAD_FAILED
            return Handler;
        }

        internal static T GetDelegate<T>(IntPtr Handler, string Function, bool Optional = true) where T : Delegate
        {
            IntPtr Address = GetProcAddress(Handler, Function);
            if (Address == IntPtr.Zero)
            {
                if (Optional)
                {
                    return null;
                }
                Environment.Exit(0x505);//ERROR_DELAY_LOAD_FAILED
            }
            return (T)Marshal.GetDelegateForFunctionPointer(Address, typeof(T));
        }

        static tD3D10CreateDevice dD3D10CreateDevice;
        static tD3D10CreateDeviceAndSwapChain dD3D10CreateDeviceAndSwapChain;
        static tD3D10CreateBlob dD3D10CreateBlob;
        static tD3D10CompileShader dD3D10CompileShader;
        static tD3D10GetPixelShaderProfile dD3D10GetPixelShaderProfile;
        static tD3D10GetVertexShaderProfile dD3D10GetVertexShaderProfile;
        static tD3D10GetGeometryShaderProfile dD3D10GetGeometryShaderProfile;
        static tD3D10GetShaderDebugInfo dD3D10GetShaderDebugInfo;
        static tD3D10PreprocessShader dD3D10PreprocessShader;
        static tD3D10GetInputSignatureBlob dD3D10GetInputSignatureBlob;
        static tD3D10GetOutputSignatureBlob dD3D10GetOutputSignatureBlob;
        static tD3D10GetInputAndOutputSignatureBlob dD3D10GetInputAndOutputSignatureBlob;
        static tD3D10CreateEffectFromMemory dD3D10CreateEffectFromMemory;
        static tD3D10CreateEffectPoolFromMemory dD3D10CreateEffectPoolFromMemory;
        static tD3D10CompileEffectFromMemory dD3D10CompileEffectFromMemory;
        static tD3D10ReflectShader dD3D10ReflectShader;
        static tD3D10DisassembleShader dD3D10DisassembleShader;
        static tD3D10DisassembleEffect dD3D10DisassembleEffect;
        static tD3D10CreateStateBlock dD3D10CreateStateBlock;
        static tD3D10StateBlockMaskUnion dD3D10StateBlockMaskUnion;
        static tD3D10StateBlockMaskIntersect dD3D10StateBlockMaskIntersect;
        static tD3D10StateBlockMaskDifference dD3D10StateBlockMaskDifference;
        static tD3D10StateBlockMaskEnableCapture dD3D10StateBlockMaskEnableCapture;
        static tD3D10StateBlockMaskDisableCapture dD3D10StateBlockMaskDisableCapture;
        static tD3D10StateBlockMaskEnableAll dD3D10StateBlockMaskEnableAll;
        static tD3D10StateBlockMaskDisableAll dD3D10StateBlockMaskDisableAll;
        static tD3D10StateBlockMaskGetSetting dD3D10StateBlockMaskGetSetting;

        delegate IntPtr tD3D10CreateDevice(IntPtr pAdapter, IntPtr DriverType, IntPtr Software, uint Flags, uint SDKVersion, IntPtr ppDevice);
        delegate IntPtr tD3D10CreateDeviceAndSwapChain(IntPtr pAdapter, IntPtr DriverType, IntPtr Software, uint Flags, uint SDKVersion, IntPtr pSwapChainDesc, IntPtr ppSwapChain, IntPtr ppDevice);
        delegate IntPtr tD3D10CreateBlob(IntPtr NumBytes, IntPtr ppBuffer);
        delegate IntPtr tD3D10CompileShader(IntPtr pSrcData, IntPtr SrcDataLen, IntPtr pFileName, IntPtr pDefines, IntPtr pInclude, IntPtr pFunctionName, IntPtr pProfile, uint Flags, IntPtr ppShader, IntPtr ppErrorMsgs);
        delegate IntPtr tD3D10GetPixelShaderProfile(IntPtr pDevice);
        delegate IntPtr tD3D10GetVertexShaderProfile(IntPtr pDevice);
        delegate IntPtr tD3D10GetGeometryShaderProfile(IntPtr pDevice);
        delegate IntPtr tD3D10GetShaderDebugInfo(IntPtr pShaderBytecode, IntPtr BytecodeLength, IntPtr ppDebugInfo);
        delegate IntPtr tD3D10PreprocessShader(IntPtr pSrcData, IntPtr SrcDataSize, IntPtr pFileName, IntPtr pDefines, IntPtr pInclude, IntPtr ppShaderText, IntPtr ppErrorMsgs);
        delegate IntPtr tD3D10GetInputSignatureBlob(IntPtr pShaderBytecode, IntPtr BytecodeLength, IntPtr ppSignatureBlob);
        delegate IntPtr tD3D10GetOutputSignatureBlob(IntPtr pShaderBytecode, IntPtr BytecodeLength, IntPtr ppSignatureBlob);
        delegate IntPtr tD3D10GetInputAndOutputSignatureBlob(IntPtr pShaderBytecode, IntPtr BytecodeLength, IntPtr ppSignatureBlob);
        delegate IntPtr tD3D10CreateEffectFromMemory(IntPtr pData, IntPtr DataLength, uint FXFlags, IntPtr pDevice, IntPtr pEffectPool, IntPtr ppEffect);
        delegate IntPtr tD3D10CreateEffectPoolFromMemory(IntPtr pData, IntPtr DataLength, uint FXFlags, IntPtr pDevice, IntPtr ppEffectPool);
        delegate IntPtr tD3D10CompileEffectFromMemory(IntPtr pData, IntPtr DataLength, IntPtr pSrcFileName, IntPtr pDefines, IntPtr pInclude, uint HLSLFlags, uint FXFlags, IntPtr ppCompiledEffect, IntPtr ppErrors);
        delegate IntPtr tD3D10ReflectShader(IntPtr pShaderBytecode, IntPtr BytecodeLength, IntPtr ppReflector);
        delegate IntPtr tD3D10DisassembleShader(IntPtr pShader, IntPtr BytecodeLength, bool EnableColorCode, IntPtr pComments, IntPtr ppDisassembly);
        delegate IntPtr tD3D10DisassembleEffect(IntPtr pEffect, bool EnableColorCode, IntPtr ppDisassembly);
        delegate IntPtr tD3D10CreateStateBlock(IntPtr pDevice, IntPtr pStateBlockMask, IntPtr ppStateBlock);
        delegate IntPtr tD3D10StateBlockMaskUnion(IntPtr pA, IntPtr pB, IntPtr pResult);
        delegate IntPtr tD3D10StateBlockMaskIntersect(IntPtr pA, IntPtr pB, IntPtr pResult);
        delegate IntPtr tD3D10StateBlockMaskDifference(IntPtr pA, IntPtr pB, IntPtr pResult);
        delegate IntPtr tD3D10StateBlockMaskEnableCapture(IntPtr pMask, IntPtr StateType, uint RangeStart, uint RangeLength);
        delegate IntPtr tD3D10StateBlockMaskDisableCapture(IntPtr pMask, IntPtr StateType, uint RangeStart, uint RangeLength);
        delegate IntPtr tD3D10StateBlockMaskEnableAll(IntPtr pMask);
        delegate IntPtr tD3D10StateBlockMaskDisableAll(IntPtr pMask);
        delegate bool tD3D10StateBlockMaskGetSetting(IntPtr pMask, IntPtr StateType, uint Entry);

    }
}
```