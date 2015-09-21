using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Renamer.NewClasses.Util;

[assembly: log4net.Config.XmlConfigurator(ConfigFile="log4net.config", Watch=true)]

namespace Renamer.NewClasses.Main
{
    
        

        
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern Boolean AttachConsole(int dwProcessId);
        const int ATTACH_PARENT_PROCESS = -1;
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger
	(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Program entry point.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if(args.Length>0){
                if (File.Exists(args[0]))
                {
                    args[0] = FileSystemUtils.goUpwards(args[0], 1);
                }
                if (!Directory.Exists(args[0]))
                {
                    AttachConsole(ATTACH_PARENT_PROCESS);
                    Console.Out.WriteLine();
                    Console.Out.WriteLine("Series Renamer command line argument(s):");
                    Console.Out.WriteLine("\"Series Renamer.exe [Path]\": Opens the program in the folder [Path].");
                    Console.Out.WriteLine("\"Series Renamer.exe /help\": Displays this help message.");
                    return;
                }
            }
            
            Console.Out.WriteLine("");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			log.Info("Starting Renamer...");
            MainForm.Instance = new MainForm(args);
            Application.Run(MainForm.Instance);
			log.Info("Ending Renamer...");
		}
    }
}
