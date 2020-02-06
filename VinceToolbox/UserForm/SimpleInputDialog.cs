using System;
using System.Windows.Forms;

namespace VinceToolbox.UserForm
{
    public class SimpleInputDialog : Form
    {
        //-----------------------------------------------------------------------------------------
        public enum Type
        {
            Text = 0,
            Number = 1,
        };
        //-----------------------------------------------------------------------------------------
        private string m_message;
        private Type m_type = Type.Text;

        //-----------------------------------------------------------------------------------------
        public string inputMessage
        {
            get { return m_message; }
            set { m_message = value; textBox1.Text = value; }
        }

        //-----------------------------------------------------------------------------------------
        public Type inputType
        {
            get { return m_type; }
            set { m_type = value; }
        }

        //-----------------------------------------------------------------------------------------
        public SimpleInputDialog(string headerText)
        {
            InitializeComponent();
            Text = headerText;
        }

        //-----------------------------------------------------------------------------------------
        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            inputMessage = textBox1.Text;
            this.Close();
        }

        //-----------------------------------------------------------------------------------------
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                okButton_Click(sender, e);
                e.Handled = true;
            }
            if (e.KeyChar == (char)Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                this.Close();
                e.Handled = true;
            }

            if (m_type == Type.Number)
            {
                if (!Char.IsDigit(e.KeyChar))
                    e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Delete || e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false;
            }
        }

        //-----------------------------------------------------------------------------------------
        public int inputNumber()
        {
            int result = 0;
            try
            {
                result = int.Parse(inputMessage);
            }
            catch { }
            return result;
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.okButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.okButton.Location = new System.Drawing.Point(120, 38);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(80, 20);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(291, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // SimpleInputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(315, 65);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.okButton);
            this.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SimpleInputDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Input Dialog Box";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox textBox1;
    }

    public class InputDialogBox
    {

        /// <summary>
        /// Shows a question-message dialog requesting input from the user.
        /// </summary>
        /// <param name="text">The text display in the message box</param>
        /// <returns>Return String that user enter. If no such input given, string with zero length will return</returns>
        public static string Show(String text)
        {
            SimpleInputDialog simpleDialog = new SimpleInputDialog(text);
            simpleDialog.inputMessage = "";
            simpleDialog.ShowDialog();
            return simpleDialog.inputMessage;
        }

        /// <summary>
        /// Shows a question-message dialog requesting input from the user.
        /// </summary>
        /// <param name="text">The text display in the message box</param>
        /// <param name="inputText">the value used to initialize the input text field</param>
        /// <returns>Return String that user enter. If no such input given, string with zero length will return</returns>
        public static string Show(String text, String inputText)
        {
            SimpleInputDialog simpleDialog = new SimpleInputDialog(text);
            simpleDialog.inputMessage = inputText;
            simpleDialog.ShowDialog();
            return simpleDialog.inputMessage;
        }

        /// <summary>
        /// Shows a question-message dialog requesting input from the user.
        /// </summary>
        /// <param name="text">The text display in the message box</param>
        /// <param name="inputText">the value used to initialize the input text field</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <returns>Return String that user enter. If no such input given, string with zero length will return</returns>
        public static string Show(String text, String inputText, String caption)
        {
            SimpleInputDialog simpleDialog = new SimpleInputDialog(text);
            simpleDialog.Text = caption;
            simpleDialog.inputMessage = inputText;
            simpleDialog.ShowDialog();
            return simpleDialog.inputMessage;
        }
    }
}
