using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WrapperGenerator
{
    public partial class Main : Form
    {
        IWrapperBuilder[] Builders = (from Asm in AppDomain.CurrentDomain.GetAssemblies()
                                      from Typ in Asm.GetTypes()
                                      where typeof(IWrapperBuilder).IsAssignableFrom(Typ) && !Typ.IsInterface
                                      select (IWrapperBuilder)Activator.CreateInstance(Typ)).ToArray();

        IWrapperBuilder CurrentBuilder {
            get {
                return (from x in Builders where x.Name == CBoxMode.Text select x).Single();
            }
        }
        public Main()
        {
            InitializeComponent();

            foreach (var Builder in Builders)
                CBoxMode.Items.Add(Builder.Name);

            CBoxMode.SelectedIndex = 0;
        }

        private void SelectFileClicked(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();
            Dialog.Filter = "All Supported Files|*.c;*.h;*.dll|All Files|*.*";
            Dialog.Title = "Select a File";
            if (Dialog.ShowDialog() != DialogResult.OK)
                return;

            tbFilePath.Text = Dialog.FileName;
            BeginInvoke(new MethodInvoker(async () => await PostFileSelect(Dialog.FileName)));
        }

        private void ModeChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFilePath.Text) || !File.Exists(tbFilePath.Text))
                return;
            BeginInvoke(new MethodInvoker(async () => await PostFileSelect(tbFilePath.Text)));
        }

        async Task PostFileSelect(string FileName)
        {
            IntPtr Handler = IntPtr.Zero;
            if (Path.GetExtension(FileName).ToLower() == ".dll")
            {
                Handler = LoadLibraryW(FileName);
                
                if (Marshal.GetLastWin32Error() == 0x000000c1)
                    MessageBox.Show("This Library isn't to the current architeture of the WrapperGenerator instance.", "WrapperGenerator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                FileName = await Decompile(FileName);
            }

            if (!File.Exists(FileName))
            {
                MessageBox.Show("Failed to Open the File:\n" + FileName, "WrapperGenerator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] Source = await File.ReadAllLinesAsync(FileName);

            SourceParser Parser = new SourceParser(Source);
            var Functions = (from x in Parser.Parse() where !x.Name.StartsWith("sub_") && !x.Name.StartsWith("SEH_") select x).ToArray();

            if (Handler != IntPtr.Zero)            
                Functions = (from x in Functions where GetProcAddress(Handler, x.Name) != IntPtr.Zero select x).ToArray();
            
            var Builder = CurrentBuilder;

            tbCodeBox.Text = Builder.BuildWrapper(Path.GetFileNameWithoutExtension(FileName), Functions);
        }

        private async Task<string> Decompile(string FileName)
        {
            string OutFile = Path.Combine(Path.GetDirectoryName(FileName), Path.GetFileNameWithoutExtension(FileName) + ".c");
            if (File.Exists(OutFile))
                return OutFile;

            Text = "Decompiling...";
            Enabled = false;
            bool x64 = MessageBox.Show("This is a x64 application?", "Decompiler", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
            string IDA = SearchIDA(x64);
            if (IDA == null)
            {
                MessageBox.Show("Please, Decompile your library using IDA PRO and try again.", "IDA PRO Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            //-Ohexrays:-nosave:-new:outfile:ALL -A "C:\Users\Marcus\Documents\My Games\Ustrack\恋×シンアイ彼女\koikake.exe"

            ProcessStartInfo ProcSI = new ProcessStartInfo();
            ProcSI.FileName = IDA;
            ProcSI.Arguments = $"\"-Ohexrays:-nosave:-new:{Path.GetFileNameWithoutExtension(FileName)}:ALL\" -A \"{FileName}\"";
            ProcSI.WorkingDirectory = Path.GetDirectoryName(FileName);
            ProcSI.CreateNoWindow = true;
            ProcSI.UseShellExecute = false;

            var Proc = Process.Start(ProcSI);
            await Proc.WaitForExitAsync();

            Text = "WrapperGenerator";
            Enabled = true;

            return OutFile;
        }

        private string LastIDADir = null;
        private string SearchIDA(bool x64)
        {
            string[] Names = x64 ? new string[] { "idat64.exe", "idaw64.exe", "idaq64.exe", "ida64.exe" } : new string[] { "idat.exe", "idaw.exe", "idaq.exe", "ida.exe" };
            if (LastIDADir != null)
            {
                foreach (string Name in Names)
                {
                    string FullPath = Path.Combine(LastIDADir, Name);
                    if (File.Exists(FullPath))
                        return FullPath;
                }
            }

            string X64ProgFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).Replace(" (x86)", "");
            string X86ProgFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

            X64ProgFiles = X64ProgFiles.Substring(3);
            X86ProgFiles = X86ProgFiles.Substring(3);

            List<string> AllProgramFiles = new List<string>();

            foreach (DriveInfo Drive in DriveInfo.GetDrives())
            {
                string ProgFilesPath = Path.Combine(Drive.RootDirectory.FullName, X64ProgFiles);
                if (Directory.Exists(ProgFilesPath))
                    AllProgramFiles.AddRange(Directory.GetDirectories(ProgFilesPath));


                ProgFilesPath = Path.Combine(Drive.RootDirectory.FullName, X86ProgFiles);
                if (Directory.Exists(ProgFilesPath))
                    AllProgramFiles.AddRange(Directory.GetDirectories(ProgFilesPath));
            }

            foreach (string Dir in AllProgramFiles)
            {
                foreach (string Name in Names)
                {
                    LastIDADir = Dir;
                    string FullPath = Path.Combine(Dir, Name);
                    if (File.Exists(FullPath))
                        return FullPath;
                }
            }

            return null;
        }

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr LoadLibraryW(string FileName);
        
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
    }
}
