using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace TreeOfLife.TaxonDialog
{
    public partial class ExportDialog : Localization.Form
    {
        public ExportDialog(List<TaxonTreeNode> _list, string _filename)
        {
            _List = _list;
            InitializeComponent();

            textBoxFilename.Text = _filename;
            UpdateCheckBoxFormat(".txt");
        }

        List<TaxonTreeNode> _List;
        string _CurrentExtention;
        string _CurrentSaveFileFilter
        {
            get
            {
                if (_CurrentExtention == ".txt") return "Text Files | *.txt";
                if (_CurrentExtention == ".csv") return "Csv Excel Files | *.csv";
                if (_CurrentExtention == ".html") return "Html Files | *.html";
                return "";
            }
        }

        static bool UpdateCheckBoxFormatInProgress = false;
        private void UpdateCheckBoxFormat(string _format)
        {
            if (UpdateCheckBoxFormatInProgress) return;
            UpdateCheckBoxFormatInProgress = true;
            _CurrentExtention = _format;
            checkBoxFormatTxt.Checked = _CurrentExtention == ".txt";
            checkBoxFormatCsv.Checked = _CurrentExtention == ".csv";
            checkBoxFormatHtml.Checked = _CurrentExtention == ".html";
            textBoxFilename.Text = Path.ChangeExtension(textBoxFilename.Text, _CurrentExtention);
            checkBoxImages.Visible = checkBoxFormatHtml.Checked;
            UpdateCheckBoxFormatInProgress = false;
        }

        private void checkBoxFormatTxt_CheckedChanged(object sender, EventArgs e) { UpdateCheckBoxFormat(".txt"); }

        private void checkBoxFormatCsv_CheckedChanged(object sender, EventArgs e) { UpdateCheckBoxFormat(".csv"); }

        private void checkBoxFormatHtml_CheckedChanged(object sender, EventArgs e) { UpdateCheckBoxFormat(".html"); }

        private void buttonBrowseFilename_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = _CurrentSaveFileFilter;
            saveFileDialog.DefaultExt = _CurrentExtention.Trim('.');
            saveFileDialog.FileName = textBoxFilename.Text;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                textBoxFilename.Text = saveFileDialog.FileName;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        bool ExportLatin { get { return checkBoxLatin.Checked; } }
        bool ExportFrench { get { return checkBoxFrench.Checked; } }
        bool ExportClassicRank { get { return checkBoxClassicRank.Checked; } }
        bool ExportAscendants { get { return checkBoxAscendants.Checked; } }
        bool ExportImage { get { return checkBoxImages.Visible && checkBoxImages.Checked; } }
        bool ExportSomeText { get { return ExportLatin || ExportFrench || ExportClassicRank || ExportAscendants; } }
        bool ExportSomething { get { return ExportSomeText || ExportImage; } }


        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (!ExportSomething)
                return;
            
            string filename = textBoxFilename.Text;

            if (_CurrentExtention == ".html")
                filename = Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar + NormalizeStringForUrl(Path.GetFileNameWithoutExtension(filename)) + ".html";

            if (Path.GetExtension(filename).ToLower() != _CurrentExtention)
                filename += _CurrentExtention;

            if (File.Exists(filename))
            {
                string message = "File: " + filename;
                message += "\r\nalready exists !";
                message += "\r\n\r\nOverwrite it ?";
                if (MessageBox.Show(message, "Export", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    return;
                File.Delete(filename);
            }

            //Encoding utf8WithoutBom = new UTF8Encoding(false);
            Encoding isoLatin1Encoding = Encoding.GetEncoding("ISO-8859-1");
            using (StreamWriter sw = new StreamWriter(filename, false, isoLatin1Encoding))
            {
                if (_CurrentExtention == ".txt") WriteText(sw);
                else if (_CurrentExtention == ".csv") WriteCsv(sw);
                else if (_CurrentExtention == ".html") WriteHtml(sw, filename);
            }

            System.Diagnostics.Process.Start(filename);
            Close();
        }

        private void WriteText( StreamWriter sw )
        {
            _List.ForEach(n => WriteTextTaxon(sw, n, ","));
        }

        private void WriteTextTaxon( StreamWriter sw, TaxonTreeNode n, string separator )
        {
            List<string> data = new List<string>();
            if (ExportLatin) data.Add(n.Desc.RefAllNames);
            if (ExportFrench) data.Add(n.Desc.FrenchAllNames);
            if (ExportClassicRank) data.Add(n.Desc.ClassicRank.ToString());
            if (ExportAscendants) data.Add(n.GetHierarchicalName());
            sw.WriteLine(string.Join(separator, data));
        }

        private void WriteCsv(StreamWriter sw)
        {
            _List.ForEach(n => WriteTextTaxon(sw, n, ";"));
        }

        const string _htmlHeader = "<html> <head> <meta charset=\"ISO-8859-1\" /> <title>[TITLE]</title> </head> <body>";
        const string _htmlBlocStartOdd = "<table bgcolor=\"LightGray\" width=\"100%\" > <tr>";
        const string _htmlBlocStartEven = "<table> <tr>";
        const string _htmlBlocColumnWithImage = "<td width=\"100\"> <a href=\"[IMAGE]\"><img width=\"100\" src=\"[IMAGE]\"></a></td>";
        const string _htmlBlocColumnNoImage = "<td width=\"100\"> </td>";

        const string _htmlBlocColumnInfoStart = "<td>";
        const string _htmlBlocColumnInfoLatin = "<p><b>[LATIN]</b></p>";
        const string _htmlBlocColumnInfoFrench = "<p>&nbsp&nbsp[FRENCH]</p>";
        const string _htmlBlocColumnInfoRank = "<p>&nbsp&nbsp<i>[RANK]</i></p>";
        const string _htmlBlocColumnInfoAscendants = "<p>&nbsp&nbsp<font size=-1><i>[ASCENDANTS]</i></font></p>";
        const string _htmlBlocColumnInfoEnd = "</td>";

        const string _htmlBlocEnd = "</tr></table>";
        const string _htmlEnd = "</body> </html>";

        const string _htmlOneImage = "<a href =\"[IMAGE]\"><img width=\"100\" src=\"[IMAGE]\"></a>";

        private void WriteHtml(StreamWriter sw, string filename)
        {
            string path = Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar;
            int rootPathLength = path.Length;
            path += Path.GetFileNameWithoutExtension(filename);
            path += "_files";
            Directory.CreateDirectory(path);

            sw.WriteLine(_htmlHeader.Replace("[TITLE]", Path.GetFileNameWithoutExtension(filename)));
            if (!ExportSomeText && ExportImage)
            {
                sw.WriteLine("<div>");
                _List.ForEach(n => WriteHtmlTaxonImage(sw, n, path, rootPathLength));
                sw.WriteLine("</div>");
            }
            else
            {
                int index = 0;
                _List.ForEach(n => WriteHtmlTaxon(sw, n, path, rootPathLength, ++index));
            }
            sw.WriteLine(_htmlEnd);
        }

        private string WriteHtmlGetImage(TaxonTreeNode node, string imageFolder)
        {
            string image = null;
            if (node.Desc.HasImage)
            {
                List<TaxonImageDesc> available = node.Desc.AvailableImages;
                if (available != null && available.Count > 0)
                {
                    string src = available[0].GetPath(node.Desc);
                    if (src != null)
                    {
                        string tgt = imageFolder + Path.DirectorySeparatorChar + Path.GetFileName(src);
                        if (File.Exists(src))
                        {
                            if (!File.Exists(tgt))
                                File.Copy(src, tgt);
                            image = tgt;
                        }
                    }
                }
            }
            return image;
        }

        private void WriteHtmlTaxonImage(StreamWriter sw, TaxonTreeNode node, string imageFolder, int rootPathLength)
        {
            string image = WriteHtmlGetImage(node, imageFolder);
            if (image != null)
                sw.WriteLine(_htmlOneImage.Replace("[IMAGE]", image.Substring(rootPathLength)));
        }
        
        private void WriteHtmlTaxon( StreamWriter sw, TaxonTreeNode node, string imageFolder, int rootPathLength, int index )
        {
            sw.WriteLine((index % 2 == 1) ? _htmlBlocStartOdd : _htmlBlocStartEven);

            if (ExportImage)
            {
                string image = WriteHtmlGetImage(node, imageFolder);
                if (image != null)
                    sw.WriteLine(_htmlBlocColumnWithImage.Replace("[IMAGE]", image.Substring(rootPathLength)));
                else
                    sw.WriteLine(_htmlBlocColumnNoImage);
            }

            sw.WriteLine(_htmlBlocColumnInfoStart);

            if (ExportLatin)
                sw.WriteLine(_htmlBlocColumnInfoLatin.Replace("[LATIN]", node.Desc.RefMainName));
            if (ExportFrench && node.Desc.HasFrenchName)
                sw.WriteLine(_htmlBlocColumnInfoFrench.Replace("[FRENCH]", node.Desc.FrenchMultiName.Main));
            if (ExportClassicRank)
                sw.WriteLine(_htmlBlocColumnInfoRank.Replace("[RANK]", node.Desc.ClassicRank.ToString()));
            if (ExportAscendants)
                sw.WriteLine(_htmlBlocColumnInfoAscendants.Replace("[ASCENDANTS]", node.GetHierarchicalName()));

            sw.WriteLine(_htmlBlocColumnInfoEnd);

            sw.WriteLine(_htmlBlocEnd);
        }

        public static string NormalizeStringForUrl(string name)
        {
            String normalizedString = name.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                switch (CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        stringBuilder.Append(c);
                        break;
                    case UnicodeCategory.SpaceSeparator:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.DashPunctuation:
                        stringBuilder.Append('_');
                        break;
                }
            }
            string result = stringBuilder.ToString();
            return String.Join("_", result.Split(new char[] { '_' }
                , StringSplitOptions.RemoveEmptyEntries)); // remove duplicate underscores
        }

    }
}
