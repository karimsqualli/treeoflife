using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using P4API;
using System.Security.Cryptography;
using System.IO;
using System.Windows.Forms;

namespace VinceToolbox
{
    //=========================================================================================
    // using Perforce with P4API
    public class P4API
    {
        //=========================================================================================
        // perforce config
        //
        public class PerforceConfig
        {
            [XmlElement("Port")]
            public string Port { get; set; }
            [XmlElement("User")]
            public string User { get; set; }
            [XmlElement("Password")]
            public string Password { get; set; }
            [XmlElement("Client")]
            public string Client { get; set; }


            [XmlIgnore]
            public string m_file = "";

            static private string s_defaultPort = "10.0.15.35:1798";

            //-----------------------------------------------------------------------------------------
            static public PerforceConfig load( string _file )
            {
                string error = null;
                PerforceConfig result = VinceToolbox.xmlFunctions.load<PerforceConfig>(_file, out error);
                if (result == null)
                    result = new PerforceConfig();
                result.m_file = _file;
                if (String.IsNullOrEmpty(result.Port)) result.Port = s_defaultPort;
                if (!string.IsNullOrEmpty(result.Password))
                    result.Password = result.decrypt(result.Password, "AED25BA0F6395CCA325F4A77B4613B91");
                return result;
            }

            //-----------------------------------------------------------------------------------------
            public void save()
            {
                if (!String.IsNullOrEmpty(Password))
                    Password = encrypt(Password, "AED25BA0F6395CCA325F4A77B4613B91");
                string error = "";
                VinceToolbox.xmlFunctions.save<PerforceConfig>(this, m_file, out error);
            }

            //-----------------------------------------------------------------------------------------
            private string encrypt(string clearText, string Password)
            {
                byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                MemoryStream ms = new MemoryStream();
                Rijndael alg = Rijndael.Create();
                alg.Key = pdb.GetBytes(32);
                alg.IV = pdb.GetBytes(16);
                CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(clearBytes, 0, clearBytes.Length);
                cs.Close();
                return Convert.ToBase64String(ms.ToArray());
            }

            //-----------------------------------------------------------------------------------------
            private string decrypt(string cipherText, string Password)
            {
                try
                {
                    byte[] cipherBytes = Convert.FromBase64String(cipherText);
                    PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    MemoryStream ms = new MemoryStream();
                    Rijndael alg = Rijndael.Create();
                    alg.Key = pdb.GetBytes(32);
                    alg.IV = pdb.GetBytes(16);
                    CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                    return System.Text.Encoding.Unicode.GetString(ms.ToArray());
                }
                catch
                {
                    return String.Empty;
                }
            }
        }

        //-----------------------------------------------------------------------------------------
        // vars 
        //
        public P4Connection m_connection = null;
        public bool m_connected = false;
        
        public string m_error = null;
        public string m_warnings = null;
        public void clearError() { m_error = null; m_warnings = null; }

        //=========================================================================================
        // constructor / init / connection 
        //

        //-----------------------------------------------------------------------------------------
        public P4API( string _configFile = null )
        {
            m_connection = new P4Connection();

            if (_configFile != null) m_configFile = _configFile;
            loadConfig();
        }


        //-----------------------------------------------------------------------------------------
        public PerforceConfig m_config = null;
        private string m_configFile = "P4Config.xml";
        private void loadConfig()
        {
            m_config = PerforceConfig.load(m_configFile);
            if (m_config == null) return;
            if (!String.IsNullOrEmpty(m_config.User)) m_connection.User = m_config.User;
            if (!String.IsNullOrEmpty(m_config.Client)) m_connection.Client = m_config.Client;
            m_connection.Port = m_config.Port;
            
        }

        //-----------------------------------------------------------------------------------------
        // create connection
        //
        public bool connect()
        {
            try
            {
                m_connection.Connect();
                try
                {
                    m_connection.Login(m_config.Password);

                    if (ClientIsValid())
                    {
                        m_connected = true;
                        return true;
                    }
                }
                catch{}

                if (Authentification())
                {
                    m_connected = true;
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                m_error = ex.ToString();
            }

            try { m_connection.Disconnect(); } catch { }
            return false;
        }

        //-----------------------------------------------------------------------------------------
        public bool ClientIsValid()
        {
            P4RecordSet p4rs = m_connection.Run("clients");
            foreach (P4Record p4r in p4rs)
            {
                if (p4r["client"] == m_connection.Client && p4r["Host"] == m_connection.Host && p4r["Owner"] == m_connection.User)
                    return true;
            }
            return false;
        }

        //-----------------------------------------------------------------------------------------
        private bool Authentification()
        {
            PerforceAuthentificationForm p4Frm = new PerforceAuthentificationForm( this );
            if (p4Frm.ShowDialog() == DialogResult.OK)
            {
                m_connected = true;
                m_config.save();
                return true;
            }
            return false;
        }

        //=========================================================================================
        // file operation
        //

        private bool fileExists(string _file)
        {
            if (m_connection == null) return false;
            P4RecordSet p4rs = m_connection.Run("files", _file);
            return (p4rs.Records.Length != 0);
        }

        public bool isOpenForEdit(string _file)
        {
            if (m_connection == null) return false;
            P4RecordSet p4rs = m_connection.Run("opened", _file);
            return (p4rs.Records.Length != 0);
        }

        public bool addOrCheckout(string _file)
        {
            clearError();

            if (m_connection == null)
            {
                m_error = "Error: perforce not connected";
                return false;
            }

            if (isOpenForEdit(_file)) return true;
            
            string message = String.Empty;
            P4RecordSet p4rs = null;
            try
            {
                if (fileExists(_file))
                    p4rs = m_connection.Run("edit", _file);
                else
                    p4rs = m_connection.Run("add", _file);
            }
            catch (Exception ex)
            {
                m_error = ex.Message;
                return false;
            }

            if (p4rs.Messages.Length > 0)
            {
                m_warnings = "";
                foreach (string msg in p4rs.Messages)
                    m_warnings += msg + "\n";
            }

            return true;
        }

    }
}

// 
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Security.Cryptography;
// using System.IO;
// using P4API;
// using System.Windows.Forms;
// using System.Xml.Serialization;
// 
// namespace ResourceTools
// {
//     static public class PerforceUtils
//     {
//         
//         
// 

//         static public void AddOrCheckout(string file)
//         {
//             if (IsOpenForEdit(file))
//             {
//                 return;
//             }
// 
//             string message = String.Empty;
//             P4RecordSet p4rs = null;
//             
//             try
//             {
//                 if (FileExists(file))
//                 {
//                     //Checkout
//                     p4rs = ResourceTools.p4.Run("edit", file);
//                 }
//                 else
//                 {
//                     //Add
//                     p4rs = ResourceTools.p4.Run("add", file);
//                 }
//             }
//             catch (Exception ex)
//             {
//                 MessageBox.Show(ex.Message);
//             }
// 
//             if (p4rs.Messages.Length > 0)
//             {
//                 foreach (string msg in p4rs.Messages)
//                 {
//                     message += msg + "\n";
//                 }
// 
//                 MessageBox.Show(message, "Perforce Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//             }
//         }
// 
//         static private bool FileExists(string file)
//         {
//             P4RecordSet p4rs = ResourceTools.p4.Run("files", file);
// 
//             if (p4rs.Records.Length == 0)
//                 return false;
// 
//             return true;
//         }
//     }
// }
