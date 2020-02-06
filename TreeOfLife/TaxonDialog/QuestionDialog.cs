using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.TaxonDialog
{
    public partial class QuestionDialog : Localization.Form
    {
        public QuestionDialog()
        {
            InitializeComponent();
        }
        public QuestionDialog(string _message, string _title, IEnumerable<OneAnswerDesc> _buttons = null, MessageBoxIcon _icon = MessageBoxIcon.Question) : this()
        {
            StartPosition = FormStartPosition.CenterParent;
            Text = _title;
            labelMessage.Text = _message;
            int X = labelMessage.Right + 10;
            int Y = labelMessage.Bottom + 10;

            if (_buttons != null)
            {
                int buttonMaxWidth = 0;
                foreach (OneAnswerDesc desc in _buttons)
                {
                    desc.Button = new Button() { Name = "Button"+desc.Text, Text = desc.Text, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowOnly, Tag = desc };
                    desc.Button.MouseClick += Button_MouseClick;
                    Controls.Add(desc.Button);
                    desc.Label = new Label() { Name= "Label" + desc.Text, Text = desc.Desc, AutoSize = true };
                    Controls.Add(desc.Label);
                    if (desc.Button.Width > buttonMaxWidth) buttonMaxWidth = desc.Button.Width;
                }
                foreach (OneAnswerDesc desc in _buttons)
                {
                    int height = Math.Max(desc.Button.Height, desc.Label.Height);
                    desc.Button.Left = 20;
                    desc.Button.Width = buttonMaxWidth;
                    desc.Button.Top = Y + (height - desc.Button.Height) / 2;
                    desc.Label.Left = 25 + buttonMaxWidth;
                    desc.Label.Top = Y + (height - desc.Label.Height) / 2;
                    Y += height + 5;
                }
            }
            Text = _title;
            labelMessage.Text = _message;
            pictureIcon.Image = SystemIcons.Question.ToBitmap();
            pictureIcon.Left = labelMessage.Right + 10;
            pictureIcon.Left = ClientSize.Width - pictureIcon.Width;
            pictureIcon.Top = 2;
        }

        public OneAnswerDesc Answer = null;

        private void Button_MouseClick(object sender, MouseEventArgs e)
        {
            Answer = (sender as Button).Tag as OneAnswerDesc;
            Close();
        }
    }

    public class AnswersDesc : List<OneAnswerDesc>
    {
        public AnswersDesc Add(string _text, string _desc, int _id = 0)
        {
            this.Add(new OneAnswerDesc() { Text = _text, Desc = _desc, ID = _id });
            return this;
        }
    }

    public class OneAnswerDesc
    {
        public int ID;
        public string Text;
        public string Desc;

        public Button Button;
        public Label Label;
    }
}
