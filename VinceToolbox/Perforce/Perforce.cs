using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;


namespace VinceToolbox
{
    
    public class Perforce
    {
        //-----------------------------------------------------------------------------------------
        // Access enumeration
        //
        public enum Access
        {
            Error = 0,
            OK,
            ReadOnly,
            DoNotExist,
            PerforceDisabled
        };

        //-----------------------------------------------------------------------------------------
        // Checkout method enumeration
        //
        public enum CheckoutMethod
        {
            Ask = 0,
            DoNothing,
            Auto
        };

        //-----------------------------------------------------------------------------------------
        // members
        //
        string m_server;
        public string Server { get { return m_server; } }
        string m_userName;
        public string UserName { get { return m_userName; } }
        string m_clientName;
        public string ClientName { get { return m_clientName; } }
        string m_clientRoot;
        public string ClientRoot { get { return m_clientRoot; } }

        bool m_getInfoDone;
        bool m_Enable;
        public bool Enable { get { return m_Enable; } set { m_Enable = value; } }

        CheckoutMethod m_CheckoutMethod;

        bool m_separateChange;
        public bool SeparateChange { get { return m_separateChange; } set { m_separateChange = value; } }
        string m_separateChangeName;
        int m_separateChangeNumber;

        Object lockPreforce = new Object();

        //-----------------------------------------------------------------------------------------
        //
        // Interface
        //
        //-----------------------------------------------------------------------------------------

        static Perforce m_singleton = null;

        //-----------------------------------------------------------------------------------------
        static public void createInterface()
        {
            m_singleton = new Perforce();
            m_singleton.getInformation();
        }

        //-----------------------------------------------------------------------------------------
        static public Perforce get()
        {
            if (m_singleton == null) createInterface();
            return m_singleton;
        }

        //-----------------------------------------------------------------------------------------
        //
        // High level function
        //
        //-----------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------
        public bool fileStart(string _fileName, ref bool _create)
        {
            // directory creation
            string directory = Path.GetDirectoryName(_fileName);
            Directory.CreateDirectory(directory);
            // new file ?
            _create = !File.Exists(_fileName);
            // perforce
            Perforce.get().getRightAccess(_fileName);
            if (Perforce.get().checkAccess(_fileName) != Perforce.Access.OK)
            {
                string msg = string.Format("can't access file \n    ") + _fileName;
                return false;
            }
            return true;
        }

        //-----------------------------------------------------------------------------------------
        public void fileEnd(string _fileName, bool _create)
        {
            if (_create)
                Perforce.get().checkoutOrAdd(_fileName);
        }

        //-----------------------------------------------------------------------------------------
        public Perforce()
        {
            m_Enable = true;
            m_CheckoutMethod = CheckoutMethod.Auto;
            m_server = "mtp-sc-perfproxy01:1780";
            m_userName = "";
            m_clientName = "";
            m_clientRoot = "";
            m_getInfoDone = false;
            m_separateChange = true;
            m_separateChangeName = "";
            m_separateChangeNumber = -1;
        }

        //-----------------------------------------------------------------------------------------
        string checkoutMethodName(CheckoutMethod _method)
        {
            return _method.ToString();
        }
        //-----------------------------------------------------------------------------------------
        CheckoutMethod checkoutMethodID(string _name)
        {
            if (_name == CheckoutMethod.Ask.ToString()) return CheckoutMethod.Ask;
            if (_name == CheckoutMethod.Auto.ToString()) return CheckoutMethod.Auto;
            if (_name == CheckoutMethod.DoNothing.ToString()) return CheckoutMethod.DoNothing;
            return CheckoutMethod.Auto;
        }
        //-----------------------------------------------------------------------------------------
        public void setCheckoutMethod(string _name)
        {
            m_CheckoutMethod = checkoutMethodID(_name);
        }
        //-----------------------------------------------------------------------------------------
        public string getCheckoutMethod()
        {
            return m_CheckoutMethod.ToString();
        }
        //-----------------------------------------------------------------------------------------
        static public List<string> getListOfCheckoutMethod()
        {
            List<string> chekoutMethod = new List<string>();
            chekoutMethod.Add(CheckoutMethod.Ask.ToString());
            chekoutMethod.Add(CheckoutMethod.Auto.ToString());
            chekoutMethod.Add(CheckoutMethod.DoNothing.ToString());
            return chekoutMethod;
        }

        //-----------------------------------------------------------------------------------------
        public void setSeparateChangeList(bool _useIt, string _name)
        {
            m_separateChange = _useIt;
            m_separateChangeName = _name;
        }

        //-----------------------------------------------------------------------------------------
        public Access checkAccess(string _file)
        {
            FileAttributes attributes;

            if (!File.Exists(_file))
                return Access.OK;

            lock (lockPreforce)
            {
                attributes = File.GetAttributes(_file);
            }

            if ((attributes & FileAttributes.Directory) == FileAttributes.Directory) return Access.Error;
            if ((attributes & FileAttributes.ReadOnly) == 0) return Access.OK;
            return Access.ReadOnly;
        }

        //-----------------------------------------------------------------------------------------
        Access getRightAccessInternal(string _file)
        {
            if (!m_Enable)
                return Access.PerforceDisabled;

            lock (lockPreforce)
            {
                if (checkAccess(_file) == Access.ReadOnly)
                    checkout(_file);
                return checkAccess(_file);
            }
        }

        //-----------------------------------------------------------------------------------------
        public Access getRightAccess(string _file)
        {
            Access result = getRightAccessInternal(_file);

            int maxLoop = 5;
            while (result != Access.OK && maxLoop > 0)
            {
                maxLoop--;
                Thread.Sleep(50);
                result = checkAccess(_file);
            }
            return result;
        }

        //-----------------------------------------------------------------------------------------
        public bool checkoutOrAdd(string _file)
        {
            bool result;
            result = add(_file);
            result |= checkout(_file);
            return result;
        }

        //-----------------------------------------------------------------------------------------
        public bool checkout(string _file)
        {
            string depotName = getDepotName(_file);
            if (depotName == "") return false;

            string command = "edit ";
            command += changeCommand();
            command += depotName;

            List<string> result = new List<string>();
            return Execute(command, result);
        }

        //-----------------------------------------------------------------------------------------
        public bool checkinSeparateChange()
        {
            if (!m_separateChange) return false;
            if (m_separateChangeName == "") return false;

            m_separateChangeNumber = getChangeNumber(m_separateChangeName);
            if (m_separateChangeNumber == -1)
                return false;

            // submit
            string command = string.Format("submit -c {0} -f revertunchanged", m_separateChangeNumber);
            List<string> result = new List<string>();
            if (!Execute(command, result)) return false;

            // delete normaly empty change list
            command = string.Format("change -d {0}", m_separateChangeNumber);
            if (!Execute(command, result)) return false;

            return true;
        }

        //-----------------------------------------------------------------------------------------
        bool add(string _file)
        {
            string depotName = getDepotName(_file);
            if (depotName == "") return false;

            string commandtemp = "filelog ";
            commandtemp += depotName;

            string command = "add ";
            command += changeCommand();
            command += depotName;

            List<string> result = new List<string>();
            //Execute(commandtemp, result);
            return Execute(command, result);
        }

        //-----------------------------------------------------------------------------------------
        string getDepotName(string _file)
        {
            int index;
            string depotName;

            depotName = _file;
            depotName = depotName.Replace('\\', '/');
            depotName = depotName.Replace("//", "/");

            index = depotName.IndexOf("/data/");
            if (index == -1)
                return "";

            depotName = depotName.Substring(index);
            depotName = "\"//Main" + depotName + "\"";
            return depotName;
        }

        //-----------------------------------------------------------------------------------------
        public void getInformation()
        {
            if (m_getInfoDone) return;

            string command = "info";
            m_getInfoDone = true;

            List<string> result = new List<string>();
            if (!Execute(command, result)) return;

            for (int i = 0; i < result.Count; i++)
            {
                string line = result[i];
                int index;

                if ((index = line.IndexOf("User name:")) != -1)
                    m_userName = line.Substring(index + 10).Trim();
                else if ((index = line.IndexOf("Client name:")) != -1)
                    m_clientName = line.Substring(index + 12).Trim();
                else if ((index = line.IndexOf("Client root:")) != -1)
                    m_clientRoot = line.Substring(index + 12).Trim();
            }
        }

        //-----------------------------------------------------------------------------------------
        int getChangeNumber(string _description)
        {
            int number = -1;

            getInformation();
            string command = "changes -s pending -u ";
            command += m_userName;

            List<string> result = new List<string>();
            if (!Execute(command, result)) return -1;

            for (int i = 0; i < result.Count; i++)
            {
                string line = result[i];
                string name;

                if (line.IndexOf("Change ") == -1)
                    continue;

                int index = line.IndexOf('\'');
                if (index == -1) continue;
                name = line.Substring(index + 1);
                if (name.Length > _description.Length)
                    name = name.Substring(0, _description.Length);
                if (name != _description)
                    continue;

                line = line.Substring(7);
                index = line.IndexOf(' ');
                line = line.Substring(0, index);
                number = int.Parse(line);

                break;
            }

            return number;
        }

        //-----------------------------------------------------------------------------------------
        const int c_inputSize = 4096;
        //---------------------------
        int createChange(string _description)
        {
            int index;

            index = getChangeNumber(_description);
            if (index != -1) return index;

            // new change list description
            string input = "";
            input += "Change: new\n";
            input += "Description: " + m_separateChangeName + "\n";
            input += "\x1a\n";

            string command = "change -i";

            List<string> result = new List<string>();
            Execute(command, result, input);

            return getChangeNumber(_description);
        }

        //-----------------------------------------------------------------------------------------
        string changeCommand()
        {
            if (!m_separateChange) return "";

            m_separateChangeNumber = createChange(m_separateChangeName);
            if (m_separateChangeNumber == -1) return "";

            string change = string.Format(" -c {0} ", m_separateChangeNumber);
            return change;
        }

        //-----------------------------------------------------------------------------------------
        bool Execute(string _command, List<string> _resultLineList, string _input = null)
        {
            if (!m_Enable) return false;

            lock (lockPreforce)
            {
                if (!m_getInfoDone) getInformation();

                string savePath = Directory.GetCurrentDirectory();
                if (m_clientRoot != "")
                    Directory.SetCurrentDirectory(m_clientRoot);

                string fullCommand = "p4 -p " + m_server + " " + _command;
                CreatePerforceProcess(fullCommand, _resultLineList, _input);
                Directory.SetCurrentDirectory(savePath);
            }

            return true;
        }

        //-----------------------------------------------------------------------------------------
        bool CreatePerforceProcess(string _command, List<string> _resultLineList, string _input)
        {
            //Declare and instantiate a new process component.
            Process process = new Process();

            process.StartInfo.FileName = "CMD.exe";
            process.StartInfo.Arguments = "/C " + _command;
            process.StartInfo.CreateNoWindow = true;

            Console.Write("{0} {1}\n", process.StartInfo.FileName, process.StartInfo.Arguments);

            //process.EnableRaisingEvents = false;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = (_input != null);

            process.Start();

            if (_input != null)
            {
                process.StandardInput.WriteLine(_input);
                process.StandardInput.Close();
            }

            while (!process.StandardOutput.EndOfStream)
                _resultLineList.Add(process.StandardOutput.ReadLine());

            process.WaitForExit();
            process.Close();

            return true;
        }
    }

}
