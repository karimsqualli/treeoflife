using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TreeOfLife.Controls
{
    public partial class TaxonCommentControl : UserControl
    {
        public TaxonCommentControl()
        {
            InitializeComponent();
            richTextBox1.GotFocus += richTextBox1_GotFocus;
        }

        bool _IsRTF = false;
        int _Height = 0;

        void richTextBox1_GotFocus(object sender, EventArgs e)
        {
            richTextBox1.Parent.Focus();
        }

        public void Init( string _name, string _comment, int _width)
        {
            SuspendLayout();

            Width = _width;
            Height = label1.Height;
            
            label1.Text = _name;
            
            _Height = 0;

            if (String.IsNullOrEmpty(_comment))
            {
                richTextBox1.Visible = false;
                webBrowser1.Visible = false;
                buttonExpand.Visible = false;
            }
            else if (_comment.StartsWith("{\\" ))
            {
                _IsRTF = true;
                richTextBox1.Visible = true;
                webBrowser1.Visible = false;
                buttonExpand.Visible = true;
                buttonExpand.Text = "-";

                richTextBox1.SuspendLayout();
                richTextBox1.Width = _width;
                richTextBox1.Height = 32;
                richTextBox1.ReadOnly = true;
                using (MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(_comment)))
                    richTextBox1.LoadFile(stream, RichTextBoxStreamType.RichText);
                richTextBox1.ResumeLayout();

                _Height = VinceToolbox.Helpers.RichTextBoxFunctions.CalculateRichTextHeight(richTextBox1) + 5;
            }
            else // html
            {
                _IsRTF = false;
                richTextBox1.Visible = false;
                webBrowser1.Visible = true;
                //buttonExpand.Visible = true;
                //buttonExpand.Text = "-";
                buttonExpand.Visible = false;
                label1.Visible = false;

                webBrowser1.DocumentText = _comment;

                var doc = webBrowser1.Document;
                doc.Encoding = "utf-8";
                webBrowser1.Refresh();

                //_Height = 100;
            }

            Height += _Height;

            ResumeLayout();
        }

        private void buttonExpand_Click(object sender, EventArgs e)
        {
            if (buttonExpand.Text == "-")
            {
                if (_IsRTF) richTextBox1.Visible = false;
                else webBrowser1.Visible = false;
                buttonExpand.Text = "+";
                Height = label1.Height;
            }
            else
            {
                if (_IsRTF) richTextBox1.Visible = true;
                else webBrowser1.Visible = true;
                buttonExpand.Text = "-";
                Height = label1.Height + (int) _Height;
            }
        }
    }

    
}
