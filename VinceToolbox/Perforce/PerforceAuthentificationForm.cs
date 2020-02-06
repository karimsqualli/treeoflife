using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VinceToolbox
{
    public partial class PerforceAuthentificationForm : Form
    {
        P4API m_p4 = null;
        
        public PerforceAuthentificationForm( P4API _p4 )
        {
            InitializeComponent();
            m_p4 = _p4 ;
            label_Info.Text = String.Empty;
            textBox_Server.Text = m_p4.m_config.Port;
            textBox_Login.Text = m_p4.m_config.User;
            textBox_Password.Text = m_p4.m_config.Password;
            textBox_Client.Text = m_p4.m_config.Client;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            m_p4.m_connection.Disconnect();
            m_p4.m_connection.Port = textBox_Server.Text;
            m_p4.m_connection.User = textBox_Login.Text;
            m_p4.m_connection.Password = textBox_Password.Text;
            m_p4.m_connection.Client = textBox_Client.Text;
            m_p4.m_connection.Connect();

            try
            {
                m_p4.m_connection.Login(textBox_Password.Text);
                if (!m_p4.ClientIsValid())
                {
                    label_Info.Text = "Wrong client.";
                    return;
                }
            }
            catch
            {
                label_Info.Text = "Wrong login or password.";
                return;
            }

            m_p4.m_config.Password = textBox_Password.Text;
            m_p4.m_config.User = m_p4.m_connection.User;
            m_p4.m_config.Port = m_p4.m_connection.Port;
            m_p4.m_config.Client = m_p4.m_connection.Client;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
