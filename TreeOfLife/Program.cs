﻿using System;
using System.Collections.Generic;
using System.IO;
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
#if USER
            SystemConfig.RunningMode = SystemConfig.RunningModeEnum.User;
#else
            SystemConfig.RunningMode = SystemConfig.RunningModeEnum.Admin;
#endif
            TaxonUtils.MyConfig = Config.Load("auto");
            // TaxonUtils.MyConfig.ToData();

            //Console.WriteLine(TaxonUtils.);
            if (! TaxonUtils.MyConfig.offline)
            {
                bool ok = TaxonUtils.CheckConnection();

                if (! ok)
                {
                    TaxonUtils.MyConfig.dataInitialized = false;
                    Console.WriteLine(TaxonUtils.MyConfig.dataInitialized);
                }
            }

            InitForm initForm = new InitForm();
            if (!TaxonUtils.MyConfig.dataInitialized)
            {
                initForm.ShowDialog();
                TaxonUtils.MyConfig.rootDirectory = TOLData.AppDataDirectory();
                Console.WriteLine(TaxonUtils.MyConfig.rootDirectory);
            }
            else
            {
                TOLData.offline = TaxonUtils.MyConfig.offline;
            }

            if (! initForm.quit)
            {

                TOLData.initSounds();


                FormAbout.DoSplashScreen();
                //----- config
                FormAbout.SetSplashScreenMessage(".. Loading config ...");

                TaxonUtils.initCollections();

                Application.Run(new Form1(args));
            }

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
