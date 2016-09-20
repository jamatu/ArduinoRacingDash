using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iRacingSLI
{
    static class Program
    {

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            AppDomain currentDomain = default(AppDomain);
            currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += GlobalUnhandledExceptionHandler;
            Application.ThreadException += GlobalThreadExceptionHandler;

            Application.Run(new iRacingSLI());
        }

        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = (Exception)e.ExceptionObject;
            ExceptionHelper.writeToLogFile(ex.Message, ex.ToString(), "Global Unhandled Exception", ex.LineNumber(), "Global");
        }

        private static void GlobalThreadExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = e.Exception;
            ExceptionHelper.writeToLogFile(ex.Message, ex.ToString(), "Global Thread Exception", ex.LineNumber(), "Global");
        }
    }

    public static class ExceptionHelper
    {
        public static int LineNumber(this Exception e)
        {
            int linenum = 0;

            try
            {
                linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(":line") + 5));
            }
            catch { }
            return linenum;
        }

        public static void writeToLogFile(string sExceptionName, string sEventName, string sControlName, int nErrorLineNo, string sFormName)
        {

            StreamWriter log;

            if (!File.Exists("logfile.txt"))
            {
                log = new StreamWriter("logfile.txt");
            }
            else
            {
                log = File.AppendText("logfile.txt");
            }

            log.WriteLine("Data Time:" + DateTime.Now);
            log.WriteLine("Version:" + iRacingSLI.Version);
            log.WriteLine("Reccomended Arduino Version:" + iRacingSLI.ArduinoVersion);
            log.WriteLine("Current Arduino Version:" + iRacingSLI.currArduinoVersion);
            log.WriteLine("Exception Name:" + sExceptionName);
            log.WriteLine("Event Name:" + sEventName);
            log.WriteLine("Control Name:" + sControlName);
            log.WriteLine("Error Line No.:" + nErrorLineNo);
            log.WriteLine("Form Name:" + sFormName);
            log.WriteLine(" ");

            log.Close();
        }
    }
}
