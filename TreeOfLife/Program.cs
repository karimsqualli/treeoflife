using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TreeOfLife
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("fr-FR");
            Application.CurrentCulture = cultureInfo;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Init system 
            Loggers.Register(new LogFile(null, true));

            LogMessageBox logMessageBox = new LogMessageBox();
            logMessageBox.SetVerboseMode(VerboseModeEnum.OnlyErrors);
            Loggers.Register(logMessageBox);

            SystemConfig.OnRunningModeChanged += Loggers_OnRunningModeChanged;

            Loggers.WriteInformation("Starting TreeOfLife : " + DateTime.Now.ToLongDateString());
            FormAbout.DoSplashScreen();
            FormAbout.SetSplashScreenMessage(".. Starting TreeOfLife ...");
#if USER
            SystemConfig.RunningMode = SystemConfig.RunningModeEnum.User;
#else
            SystemConfig.RunningMode = SystemConfig.RunningModeEnum.Admin;
#endif

            Application.Run(new Form1(args));
        }

        private static void Loggers_OnRunningModeChanged(object sender, EventArgs e)
        {
            VerboseModeEnum verboseMode = VerboseModeEnum.OnlyErrors;
            if (SystemConfig.RunningMode == SystemConfig.RunningModeEnum.User)
                verboseMode = VerboseModeEnum.NothingAtAll;
            else if (SystemConfig.RunningMode == SystemConfig.RunningModeEnum.Debugger)
                verboseMode = VerboseModeEnum.Full;

            IEnumerable<LogMessageBox> list = Loggers.List.OfType<LogMessageBox>();
            foreach (LogMessageBox logger in list)
                logger.SetVerboseMode(verboseMode);
        }
    }
}
