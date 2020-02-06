using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using System.Text.RegularExpressions;

namespace ConvertDocXToHtml
{
    public partial class FormConvertDocX2Html : Form
    {
        public FormConvertDocX2Html()
        {
            InitializeComponent();
            textBoxBrowseSource.Text = "E:\\Project TreeOfLife\\Datas\\CommentairesDocX\\Actinistia.docx";
            textBoxBrowseTarget.Text = "E:\\Project TreeOfLife\\Datas\\Commentaires";
        }

        Config Config = null;
        private void FormConvertDocX2Html_Load(object sender, EventArgs e)
        {
            Config = Config.Load();
            Config.MainWindowPlacementToUI(Handle);
            textBoxBrowseSource.Text = Config.SourceFolder;
            textBoxBrowseTarget.Text = Config.TargetFolder;
            radioButtonSourceFile.Checked = !Config.SourceIsFolder;
            radioButtonSourceFolder.Checked = Config.SourceIsFolder;
        }

        private void FormConvertDocX2Html_FormClosing(object sender, FormClosingEventArgs e)
        {
            Config.MainWindowPlacementFromUI(Handle);
            Config.SourceFolder = textBoxBrowseSource.Text;
            Config.TargetFolder = textBoxBrowseTarget.Text;
            Config.Save();
        }

        private void radioButtonSourceFolder_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonSourceFolder.Checked) return;
            Config.SourceIsFolder = true;
            textBoxBrowseSource.Text = Config.SourceFolder;
        }

        private void radioButtonSourceFile_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonSourceFile.Checked) return;
            Config.SourceIsFolder = false;
            textBoxBrowseSource.Text = Config.SourceFilename;
        }

        private void buttonBrouseSource_Click(object sender, EventArgs e)
        {
            if (Config.SourceIsFolder)
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    textBoxBrowseSource.Text = dlg.SelectedPath;
                    Config.SourceFolder = dlg.SelectedPath;
                }
            }
            else
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "docx files (*.docx)|*.docx";
                dlg.Multiselect = false;
                dlg.AddExtension = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    textBoxBrowseSource.Text = dlg.FileName;
                    Config.SourceFilename = dlg.FileName;
                }
            }

        }

        private void buttonBrowseTarget_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBoxBrowseTarget.Text = dlg.SelectedPath;
                Config.TargetFolder = dlg.SelectedPath;
            }
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            logClear();

            _TargetFolder = textBoxBrowseTarget.Text;
            try
            {
                if (!Directory.Exists(_TargetFolder))
                    Directory.CreateDirectory(_TargetFolder);
            }
            catch
            {
                MessageBox.Show("Cannot create target directory :\n" + _TargetFolder, "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> docxFiles = null;
            _Source = textBoxBrowseSource.Text;
            if (File.Exists(_Source))
            {
                docxFiles = new List<string>() { _Source };
            }
            else if (Directory.Exists(_Source))
            {
                docxFiles = Directory.GetFiles(_Source, "*.docx").ToList();
            }
            else
            {
                MessageBox.Show(_Source + "\ndoesn't exist !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            log ("Found " + docxFiles.Count + " docx files" );

            foreach (string file in docxFiles)
            {
                HtmlConverterInit(file);
                log("    " + _SourceFile + " ==> " + _TargetFile ); 
                using (WordprocessingDocument doc = WordprocessingDocument.Open(_SourceFile, true))
                {
                    OpenXmlPowerTools.HtmlConverterSettings settings = new OpenXmlPowerTools.HtmlConverterSettings();
                    settings.PageTitle = _Name;
                    settings.ImageHandler = HtmlConverterImageHandler;
                    XElement html = OpenXmlPowerTools.HtmlConverter.ConvertToHtml(doc, settings);

                    string htmlContent = html.ToString();
                    htmlContent = CleanHtml(htmlContent);
                    
                    File.WriteAllText(_TargetFile, htmlContent);
                }
            }

            log("Finished"); 
        }

        string _Source;
        string _TargetFolder;
        int _ImageCounter = 0;
        string _Name;
        string _SourceFile;
        string _TargetFile;
        string _TargetFileWithoutExtension;
        string _ImagePath;
        string _RelativeImagePath;

        void HtmlConverterInit(string _file)
        {
            _ImageCounter = 0;
            _SourceFile = _file;
            _Name = Path.GetFileNameWithoutExtension(_file);
            _ImagePath = _TargetFolder + "\\" + _Name;
            _RelativeImagePath = _Name;
            _TargetFileWithoutExtension = _TargetFolder + "\\" + _Name;
            _TargetFile = _TargetFolder + "\\" + _Name + ".html";
        }

        XElement HtmlConverterImageHandler(OpenXmlPowerTools.ImageInfo imageInfo)
        {
            if (_ImageCounter == 0)
                Directory.CreateDirectory(_ImagePath);

            ++_ImageCounter;
            string extension = imageInfo.ContentType.Split('/')[1].ToLower();
            ImageFormat imageFormat = null;
            if (extension == "png")
            {
                // Convert the .png file to a .jpeg file.
                extension = "jpeg";
                imageFormat = ImageFormat.Jpeg;
            }
            else if (extension == "bmp")
                imageFormat = ImageFormat.Bmp;
            else if (extension == "jpeg")
                imageFormat = ImageFormat.Jpeg;
            else if (extension == "tiff")
                imageFormat = ImageFormat.Tiff;

            // If the image format is not one that you expect, ignore it,
            // and do not return markup for the link.
            if (imageFormat == null)
                return null;

            string relativeImageFileName = _Name + "\\image" + _ImageCounter.ToString() + "." + extension;
            string imageFileName = _ImagePath + "\\image" + _ImageCounter.ToString() + "." + extension;

            try
            {
                imageInfo.Bitmap.Save(imageFileName, imageFormat);
            }
            catch (System.Runtime.InteropServices.ExternalException)
            {
                return null;
            }
            XElement img = new XElement("img", new XAttribute("src", relativeImageFileName));
            return img;
        }

        private void logClear()
        {
            listBoxResult.Items.Clear();
        }

        private void log(string _line)
        {
            listBoxResult.Items.Add(_line);
        }

        string CleanHtml( string _html )
        {
            _html = CleanParagraph(_html);
            _html = CleanUselessSpan(_html);
            _html = CleanFont(_html);
            _html = CleanMisc(_html);
            _html = CleanUnorderedList(_html);
            return _html;
        }

        string CleanParagraph(string _html)
        {
            string patternParagraph = @"(?<para><p[^>]*>)";
            Regex rgx = new Regex(patternParagraph);
            _html = rgx.Replace(_html, "<p>");

            string patternParagraphStyle = @"(?<para>p.pt-Normal[^}]*})";
            rgx = new Regex(patternParagraphStyle);
            _html = rgx.Replace(_html, "");

            return _html;
        }

        string CleanUselessSpan(string _html)
        {
            Regex rgx = new Regex("<span[^>]*/>");
            _html = rgx.Replace(_html, "");

            Regex rgxSpanLink = new Regex("<span[^>]*hypertexte[^>]*>");
            Match matchSpan = rgxSpanLink.Match(_html);
            while (matchSpan.Success)
            {
                _html = _html.Remove(matchSpan.Index, matchSpan.Length);
                int index = _html.IndexOf("</span>", matchSpan.Index);
                _html = _html.Remove(index, "</span>".Length);
                matchSpan = rgxSpanLink.Match(_html);
            }

            return _html;
        }

        string CleanMisc(string _html)
        {
            Regex rgxMeta = new Regex("<meta[^>]*powertools[^>]*>", RegexOptions.IgnoreCase);
            _html = rgxMeta.Replace(_html, "");

            Regex rgxStyle = new Regex("<style>.*<\\/style>", RegexOptions.Singleline);
            _html = rgxStyle.Replace(_html, "<link rel=\"stylesheet\" href=\"_Styles/style.css\" />");

            Regex rgxHtml = new Regex("<html[^>]*>");
            _html = rgxHtml.Replace(_html, "<html>");

            Regex rgxEmptyLine = new Regex("\\n\\s*\\n");
            _html = rgxEmptyLine.Replace(_html, "\n");

            return _html;
        }


        string CleanFont(string _html)
        {
            string pattern = @"(?<font>span.pt-Policepardfaut[^}]*})";
            Regex rgx = new Regex(pattern);
            MatchCollection matches = rgx.Matches(_html);
            List<Tuple<string, string, string>> replacementList = new List<Tuple<string, string,string>>();

            foreach (Match match in matches)
            {
                Group grp = match.Groups["font"];

                Regex rgxName = new Regex(@"span\.(?<name>pt-Policepardfaut\S*)");
                string name = rgxName.Match(grp.Value).Groups["name"].Value.Trim();

                Regex rgxColor = new Regex(@"(color:(?<color>[^;]*))");
                string colorstring = rgxColor.Match(grp.Value).Groups["color"].Value.Trim();
                Color color = ColorTranslator.FromHtml(colorstring);
                int colorValue = Color.FromArgb(0, color).ToArgb();

                Regex rgxFontStyle = new Regex(@"(font-style:(?<style>[^;]*))");
                string style = rgxFontStyle.Match(grp.Value).Groups["style"].Value.Trim();

                Regex rgxFontWeight = new Regex(@"(font-weight:(?<weight>[^;]*))");
                string weight = rgxFontWeight.Match(grp.Value).Groups["weight"].Value.Trim();

                if (colorValue != 0)
                    replacementList.Add( new Tuple<string,string,string>(name, "<span class=\"figureRef\">", "</span>" ));
                else if (style == "italic")
                    replacementList.Add( new Tuple<string,string,string>(name, "<em>", "</em>" ));
                else if (weight == "bold")
                    replacementList.Add(new Tuple<string, string, string>(name, "<strong>", "</strong>"));
                else
                    replacementList.Add( new Tuple<string,string,string>(name, "", "" ));
            }
            
            _html = rgx.Replace(_html, "");

            foreach (Tuple<string, string, string> rep in replacementList )
            {
                string patternSpan = "<span[^>]*class=\"" + rep.Item1 + "\"[^>]*>";
                Regex rgxSpan = new Regex(patternSpan);
                
                Match matchSpan = rgxSpan.Match(_html);
                while (matchSpan.Success)
                {
                    _html = _html.Remove(matchSpan.Index, matchSpan.Length);
                    int index = _html.IndexOf("</span>", matchSpan.Index);
                    _html = _html.Remove(index, "</span>".Length);
                    if (index != matchSpan.Index && rep.Item2 != "")
                    {
                        _html = _html.Insert(index, rep.Item3);
                        _html = _html.Insert(matchSpan.Index, rep.Item2);
                    }
                    matchSpan = rgxSpan.Match(_html);
                }
            }

            
            return _html;
        }

        string CleanUnorderedList(string _html)
        {
            // special code to inverse - char when in bold text
            Regex rgxWrongHyphen = new Regex("<p>\\s*<strong>\\s*-");
            _html = rgxWrongHyphen.Replace(_html, "<p>-<strong>");

            Regex para = new Regex("<p>\\s*-");
            Match matchSpan = para.Match(_html);
            while (matchSpan.Success)
            {
                _html = _html.Remove(matchSpan.Index, matchSpan.Length);
                _html = _html.Insert(matchSpan.Index, "<ul><li>");
                int index = _html.IndexOf("</p>", matchSpan.Index);
                _html = _html.Remove(index, "</p>".Length);
                _html = _html.Insert(index, "</li></ul>");
                matchSpan = para.Match(_html);
            }
            return _html;
        }
    }
}

