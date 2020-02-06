using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife
{
    [Description("Log messages")]
    [DisplayName("Log")]
    [Controls.IconAttribute("TaxonLog")]
    public partial class TaxonLog : Controls.TaxonControl
    {
        LogMessages _Log;

        public TaxonLog()
        {
            _Log = new LogMessages();
            Loggers.Register(_Log);

            InitializeComponent();
            logListBox1.DataSource = _Log;

            _Log.OnLogMessagesChanged += OnLogMessagesChanged;
        }

        void OnLogMessagesChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                logListBox1.DataSource = null;
                logListBox1.DataSource = _Log;
            }));
        }
    }
}
