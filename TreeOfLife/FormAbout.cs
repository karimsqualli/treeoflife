using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TreeOfLife
{
    public partial class FormAbout : Localization.Form
    {
        public FormAbout( bool _splashScreen = false)
        {
            InitializeComponent();
            buttonClose.Visible = !_splashScreen;
            /*
            try { richTextBox1.LoadFile(NewsFile); }
            catch (Exception e)
            {
                richTextBox1.Text = e.Message;
                if (e.InnerException != null)
                    richTextBox1.Text += e.InnerException.Message;
            }
            */
            Focus();
            
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        public static void DisplayFreshNews()
        {
            // TODO : AVIRER ?
            /*           
            if (!File.Exists(NewsFile))
            {
                TaxonUtils.MyConfig.DateOfLastNews = DateTime.MinValue;
                return;
            }

            DateTime lastModified = File.GetLastWriteTime(NewsFile);
            if (lastModified > TaxonUtils.MyConfig.DateOfLastNews)
            {
                TaxonUtils.MyConfig.DateOfLastNews = lastModified;
                FormAbout aboutForm = new FormAbout();
                aboutForm.ShowDialog();
            }
            */
        }

        static FormAbout _SplashScreen = null;
        public static void DoSplashScreen()
        {
            if (_SplashScreen != null)
                return;
            _SplashScreen = new FormAbout(true);
            _SplashScreen.Show();
            Application.DoEvents();
        }

        public static void EndSplashScreen()
        {
            if (_SplashScreen == null)
                return;
            _SplashScreen.Close();
            _SplashScreen = null;
        }

        public static void SetSplashScreenMessage( string _msg, bool _showWaitImage = false, Action _onCancel = null )
        {
            if (_SplashScreen == null) return;
            if (String.IsNullOrEmpty(_msg))
                ClearSplashScreenMessage();
            else
            {
                int y = _SplashScreen.Height - 22;
                int h = 22;
                int w = _SplashScreen.Width;

                if (_showWaitImage)
                {
                    _SplashScreen.pictureWait.Visible = true;
                    _SplashScreen.pictureWait.Top = y - _SplashScreen.pictureWait.Height;
                    _SplashScreen.pictureWait.Left = w - _SplashScreen.pictureWait.Width;
                }
                else
                    _SplashScreen.pictureWait.Visible = false;

                if (_onCancel != null)
                {
                    _SplashScreen._OnCancel = _onCancel;
                    _SplashScreen.buttonCancel.Visible = true;
                    _SplashScreen.buttonCancel.Top = y;
                    _SplashScreen.buttonCancel.Height = h;
                    w -= _SplashScreen.buttonCancel.Width;
                }
                else
                    _SplashScreen.buttonCancel.Visible = false;

                _SplashScreen.labelMessage.Visible = true;
                _SplashScreen.labelMessage.Width = w;
                _SplashScreen.labelMessage.Top = y;
                _SplashScreen.labelMessage.Height = h;
                _SplashScreen.labelMessage.Text = _msg;
                Application.DoEvents();
            }
        }

        public static void UpdateSplashScreenMessage(string _msg)
        {
            if (_SplashScreen == null) return;
            _SplashScreen.labelMessage.Text = _msg ?? "";
            Application.DoEvents();
        }

        public static void InvokeUpdateSplashScreenMessage(string _msg)
        {
            if (_SplashScreen == null) return;
            _SplashScreen.BeginInvoke( new Action( () => UpdateSplashScreenMessage(_msg )) );
            Application.DoEvents();
        }

        public static void ClearSplashScreenMessage()
        {
            if (_SplashScreen == null) return;
            _SplashScreen.labelMessage.Visible = false;
            _SplashScreen.pictureWait.Visible = false;
            _SplashScreen.buttonCancel.Visible = false;
            Application.DoEvents();
        }

        Action _OnCancel;
        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            _OnCancel?.Invoke();
        }
    }
}
