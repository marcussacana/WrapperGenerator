using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WrapperGenerator
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }


        public static void SetUnsafeMode(this Function[] Exports, bool Unsafe) {
            for (int i = 0; i < Exports.Length; i++)
            {
                Exports[i].AnonType = Unsafe;
                Exports[i].Unsafe = Unsafe;
                for (int x = 0; x < Exports[i].Arguments.Length; x++)
                {
                    Exports[i].Arguments[x].Unsafe = Unsafe;
                    Exports[i].Arguments[x].AnonType = Unsafe;
                }
            }
        }
        public static void SetAnonType(this Function[] Exports, bool Anon)
        {
            for (int i = 0; i < Exports.Length; i++)
            {
                Exports[i].AnonType = Anon;
                for (int x = 0; x < Exports[i].Arguments.Length; x++)
                {
                    Exports[i].Arguments[x].AnonType = Anon;
                }
            }
        }
    }
}
