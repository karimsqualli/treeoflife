using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using VinceToolbox;

namespace TreeOfLife.TaxonDialog
{
    public partial class CollectionManagerDialog : Localization.Form
    {
        public CollectionManagerDialog()
        {
            InitializeComponent();
            CurrentImageCollection = null;
            FillCollections();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                if (winFunctions.winPlacementIsValid(TaxonUtils.MyConfig.CollectionWindowPlacement))
                    if (!winFunctions.winSetPlacement(Handle, TaxonUtils.MyConfig.CollectionWindowPlacement, 0.5))
                        CenterToParent();
            }
            catch
            {
                CenterToParent();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            bw_Clear();
            base.OnClosing(e);
            TaxonUtils.MyConfig.CollectionWindowPlacement = winFunctions.winGetPlacement(Handle);
        }

        private void FillCollections()
        {
            FillCollectionsImages();
            FillCollectionsComments();
        }

        private void FillCollectionsImages()
        {
            bw_Clear();
            dataGridViewImages.Rows.Clear();
            TaxonImages.Manager.RebuildCollections();
            foreach (ImageCollection collection in TaxonImages.Manager.CollectionsEnumerable())
            {
                string name = collection.Name;

                CollectionToEnumerate.Add(collection);

                bool useIt = collection.UseIt;
                bool isDefault = collection.IsDefault;

                dataGridViewImages.Rows.Add(new object[] { name, "...", useIt, isDefault });
                dataGridViewImages.Rows[dataGridViewImages.Rows.Count - 1].Tag = collection;
            }
            bw_Start();

            if (tabControlCollections.SelectedTab == tabPageImages)
                dataGridViewImages_SelectionChanged(dataGridViewImages, null);
        }

        BackgroundWorker bw = null;
        List<ImageCollection> CollectionToEnumerate = new List<ImageCollection>();

        private void bw_Start()
        {
            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerAsync();
        }

        private void bw_Clear()
        {
            if (bw != null)
            {
                bw.CancelAsync();
                while (bw.IsBusy)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(200);
                }
                bw = null;
            }
            CollectionToEnumerate.Clear();
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while (true)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                string folder = null;
                int count = 0;
                lock (CollectionToEnumerate)
                {
                    if (CollectionToEnumerate.Count > 0)
                    {
                        folder = CollectionToEnumerate[0].Path;
                        count += CollectionToEnumerate[0].NumberOfLinks();
                        CollectionToEnumerate.RemoveAt(0);
                    }
                }

                if (folder == null)
                {
                    System.Threading.Thread.Sleep(500);
                    continue;
                }

                /*string[] csvFiles = Directory.GetFiles(folder, "*.csv");
                foreach ( string csv in csvFiles )
                {
                    string xml = Path.ChangeExtension(csv, ".xml");
                    if (File.Exists(xml))
                    {
                        if (File.GetLastWriteTime(xml).CompareTo(File.GetLastWriteTime(csv)) >= 0)
                            continue;
                        File.Delete(xml);
                    }
                    ConvertCsvToXml(csv);
                }*/

                //int count = 0;

                /*string[] xmlFiles = Directory.GetFiles(folder, "*.xml");
                foreach( string xml in xmlFiles )
                { 
                    if (xml.ToLower().StartsWith("_infos"))
                        continue;

                    XmlSerializer deserializer = new XmlSerializer(typeof(ImagesLinks));
                    TextReader reader = new StreamReader(xml);
                    try
                    {
                        object obj = deserializer.Deserialize(reader);
                        reader.Close();
                        ImagesLinks list = obj as ImagesLinks;
                        count += list.Count;
                    }
                    catch { }

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                */

                IEnumerable<string> files = Directory.EnumerateFiles(folder, "*.jpg");
                foreach (string file in files)
                {
                    count++;
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                lock (dataGridViewImages.Rows)
                {
                    foreach (DataGridViewRow row in dataGridViewImages.Rows)
                    {
                        if (row == null || row.Tag == null) continue;
                        if (!(row.Tag is ImageCollection)) continue;
                        if ((row.Tag as ImageCollection).Path != folder) continue;
                        row.Cells[1].Value = count.ToString();
                        break;
                    }
                }
            }
        }

        

        /*private void ConvertCsvToXml(string _path)
        {
            ImagesLinks datas = new ImagesLinks();

            using (StreamReader tr = new StreamReader(_path))
            {
                string line;
                while ((line = tr.ReadLine()) != null)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1])) continue;
                    if (parts[1].StartsWith("\""))
                    {
                        parts[1] = parts[1].Remove(0, 1);
                        int index = parts[1].IndexOf("\"");
                        if (index != -1) parts[1] = parts[1].Remove(index);
                    }
                    datas.Add(new ImageLink() { Key = parts[0], Link = parts[1] });
                }
            }

            string xml = Path.ChangeExtension(_path, ".xml");
            XmlSerializer serializer = new XmlSerializer(typeof(ImagesLinks));
            using (TextWriter writer = new StreamWriter(xml))
            {
                serializer.Serialize(writer, datas);
            }
        }*/

        private void FillCollectionsComments()
        {
            DataGridViewRow toSelect = null;
            string toSelectPath = null;
            if (dataGridViewComments.SelectedRows.Count > 0 && dataGridViewComments.SelectedRows[0] != null)
                toSelectPath= (dataGridViewComments.SelectedRows[0].Tag as CommentsCollection).Path;

            dataGridViewComments.Rows.Clear();

            foreach (Image image in CreatedImages)
                image.Dispose();
            CreatedImages.Clear();

            TaxonComments.Manager.RebuildCollections();
            int count = TaxonComments.Manager.Collections.Count;
            foreach (CommentsCollection collection in TaxonComments.Manager.Collections)
            {
                bool up = TaxonComments.Manager.CanMoveUp(collection);
                bool down = TaxonComments.Manager.CanMoveDown(collection);
                string name = collection.Name;
                string[] files = null;
                try { files = System.IO.Directory.GetFiles(collection.Path, "*.html"); }
                catch { continue; }

                int fileNumber = files.Where(x => !x.StartsWith("_") && !x.StartsWith("~")).Count();
                bool useIt = collection.UseIt;
                Image image = CommentsPrioImage(collection.PriorityOrder, up, down);

                dataGridViewComments.Rows.Add(new object[] { name, fileNumber, useIt, image });
                dataGridViewComments.Rows[dataGridViewComments.Rows.Count - 1].Tag = collection;
                if (collection.Path == toSelectPath)
                    toSelect = dataGridViewComments.Rows[dataGridViewComments.Rows.Count - 1];
            }
            if (tabControlCollections.SelectedTab == tabPageComments)
            {
                if (toSelect != null)
                    toSelect.Selected = true;
                dataGridViewComments_SelectionChanged(dataGridViewComments, null);
            }
        }

        Image _CommentsNoPrioImage = null;
        private Image CommentsNoPrioImage()
        {
            if (_CommentsNoPrioImage == null)
                _CommentsNoPrioImage = new Bitmap(64, 20);
            return _CommentsNoPrioImage;
        }

        List<Image> CreatedImages = new List<Image>();
        private Image CommentsPrioImage(int _prio, bool _up, bool _down )
        {
            if (_prio == -1) return CommentsNoPrioImage();

            Image image = new Bitmap(64, 20);
            RectangleF bounds = new RectangleF(0,0,64,20);
            StringFormat stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            using (Graphics g = Graphics.FromImage(image))
            {
                g.DrawString(_prio.ToString(), dataGridViewComments.Font, Brushes.Black, bounds, stringFormat);
                if (_up)
                    g.DrawImage(global::TreeOfLife.Properties.Resources.lineup_16x16, new Rectangle(2, 2, 16, 16));
                if (_down)
                    g.DrawImage(global::TreeOfLife.Properties.Resources.linedown_16x16, new Rectangle(46, 2,16,16));
            }
            CreatedImages.Add(image);
            return image;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string message = "Are you sure ?";
            message += "\n\nCollection directory will be completely deleted\n\n";
            message += "Still sure ?";
            message = Localization.Manager.Get("_DeleteCollectionMessage", message);
            DialogResult result = MessageBox.Show(message, "Delete collection", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK) return;

            if (tabControlCollections.SelectedTab == tabPageImages)
            {
                System.IO.Directory.Delete(CurrentImageCollection.Path, true);
                FillCollectionsImages();
            }
            else
            {
                System.IO.Directory.Delete(CurrentCommentsCollection.Path, true);
                FillCollectionsComments();
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            TaxonImages.Manager.SaveCollections();
            TaxonComments.Manager.SaveCollections();
            TaxonComments.Manager.CleanCommentInMemory(0);
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        { 
            string fileDesc;
            if (tabControlCollections.SelectedTab == tabPageImages)
                fileDesc = CurrentImageCollection.Path + "\\_description.rtf";
            else
                fileDesc = CurrentCommentsCollection.Path + "\\_description.rtf";
            richTextBox1.SaveFile(fileDesc);
        }

        private void dataGridViewImages_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewImages.SelectedRows.Count != 1)
                CurrentImageCollection = null;
            else
                CurrentImageCollection = dataGridViewImages.SelectedRows[0].Tag as ImageCollection;
        }

        private void dataGridViewComments_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewComments.SelectedRows.Count != 1)
                CurrentCommentsCollection = null;
            else
                CurrentCommentsCollection = dataGridViewComments.SelectedRows[0].Tag as CommentsCollection;
        }

        ImageCollection _CurrentImageCollection = null;
        ImageCollection CurrentImageCollection
        {
            get { return _CurrentImageCollection; }
            set
            {
                _CurrentImageCollection = value;

                if (_CurrentImageCollection == null)
                {
                    richTextBox1.Enabled = false;
                    richTextBox1.Text = "";
                    buttonSave.Enabled = false;
                    buttonDelete.Enabled = false;
                    return;
                }

                string fileDesc = CurrentImageCollection.Path + "\\_description.rtf";
                if (System.IO.File.Exists(fileDesc))
                    richTextBox1.LoadFile(fileDesc);
                else
                    richTextBox1.Text = "";

                richTextBox1.Enabled = true;
                buttonSave.Enabled = true;
                buttonDelete.Enabled = true;
            }
        }

        CommentsCollection _CurrentCommentsCollection = null;
        CommentsCollection CurrentCommentsCollection
        {
            get { return _CurrentCommentsCollection; }
            set
            {
                _CurrentCommentsCollection = value;

                if (_CurrentCommentsCollection == null)
                {
                    richTextBox1.Enabled = false;
                    richTextBox1.Text = "";
                    buttonSave.Enabled = false;
                    buttonDelete.Enabled = false;
                    return;
                }

                string fileDesc = _CurrentCommentsCollection.Path + "\\_description.rtf";
                if (System.IO.File.Exists(fileDesc))
                    richTextBox1.LoadFile(fileDesc);
                else
                    richTextBox1.Text = "";

                richTextBox1.Enabled = true;
                buttonSave.Enabled = true;
                buttonDelete.Enabled = true;
            }
        }

        private void textBoxNew_TextChanged(object sender, EventArgs e)
        {
            bool acceptable = !String.IsNullOrWhiteSpace(textBoxNew.Text);
            acceptable &= !textBoxNew.Text.Contains('/');
            acceptable &= !textBoxNew.Text.Contains('\\');
            buttonAddNew.Enabled = acceptable;
        }

        private void buttonAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControlCollections.SelectedTab == tabPageImages)
                {
                    string path = TaxonImages.Manager.Path + "//" + textBoxNew.Text;
                    System.IO.Directory.CreateDirectory(path);
                    FillCollectionsImages();
                }
                if (tabControlCollections.SelectedTab == tabPageComments)
                {
                    string path = TaxonComments.Manager.Path + "//" + textBoxNew.Text;
                    System.IO.Directory.CreateDirectory(path);
                    FillCollectionsComments();
                }
            }
            catch (Exception ex)
            {
                string message = "Cannot create new collection " + textBoxNew.Text + "\n\n";
                message += "Exception: " + ex.Message;
                Loggers.WriteError(LogTags.Collection, message);
            }

        }

        private void dataGridViewImages_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                if (dataGridViewImages.Columns[e.ColumnIndex] == ColumnImageUseIt)
                {
                    ContextMenu cm = new ContextMenu();
                    cm.MenuItems.Add(Localization.Manager.Get("_CheckAll", "Check all"), new EventHandler((o, e1) =>
                       {
                           foreach (DataGridViewRow row in dataGridViewImages.Rows)
                           {
                               if (!(row.Tag is ImageCollection)) continue;
                               (row.Tag as ImageCollection).UseIt = true;
                               row.Cells["ColumnImageUseIt"].Value = true;
                           }
                       }));
                    cm.MenuItems.Add(Localization.Manager.Get("_UncheckAll", "Uncheck all"), new EventHandler((o, e1) =>
                    {
                        foreach (DataGridViewRow row in dataGridViewImages.Rows)
                        {
                            if (!(row.Tag is ImageCollection)) continue;
                            (row.Tag as ImageCollection).UseIt = false;
                            row.Cells["ColumnImageUseIt"].Value = false;
                        }
                    })) ;
                    cm.Show(this, this.PointToClient(Cursor.Position));
                }
                return;
            }

            if (dataGridViewImages.Columns[e.ColumnIndex] == ColumnImageUseIt)
            {
                ImageCollection collection = dataGridViewImages.Rows[e.RowIndex].Tag as ImageCollection;
                if (collection != null)
                {
                    collection.UseIt = !collection.UseIt;
                    dataGridViewImages.Rows[e.RowIndex].Cells["ColumnImageUseIt"].Value = collection.UseIt;
                }
            }
            else if (dataGridViewImages.Columns[e.ColumnIndex] == ColumnImageDefault)
            {
                ImageCollection defaultCollection = dataGridViewImages.Rows[e.RowIndex].Tag as ImageCollection;
                SetDefaultCollection(defaultCollection);
            }
        }

        private void dataGridViewComments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                if (dataGridViewComments.Columns[e.ColumnIndex] == ColumnCommentsUseIt)
                {
                    ContextMenu cm = new ContextMenu();
                    cm.MenuItems.Add(Localization.Manager.Get("_CheckAll", "Check all"), new EventHandler((o, e1) =>
                    {
                        foreach (DataGridViewRow row in dataGridViewComments.Rows)
                            (row.Tag as CommentsCollection).UseIt = true;
                        TaxonComments.Manager.SaveCollections();
                        FillCollectionsComments();
                    }));
                    cm.MenuItems.Add(Localization.Manager.Get("_UncheckAll", "Uncheck all"), new EventHandler((o, e1) =>
                    {
                        foreach (DataGridViewRow row in dataGridViewComments.Rows)
                            (row.Tag as CommentsCollection).UseIt = false;
                        TaxonComments.Manager.SaveCollections();
                        FillCollectionsComments();
                    }));
                    cm.Show(this, this.PointToClient(Cursor.Position));
                }
                return;
            }

            if (dataGridViewComments.Columns[e.ColumnIndex] == ColumnCommentsUseIt)
            {
                if (e.RowIndex < 0 || e.RowIndex >= dataGridViewComments.Rows.Count) return;
                CommentsCollection collection = dataGridViewComments.Rows[e.RowIndex].Tag as CommentsCollection;
                if (collection != null)
                {
                    collection.UseIt = !collection.UseIt;
                    TaxonComments.Manager.SaveCollections();
                    FillCollectionsComments();
                }
            }
            if (dataGridViewComments.Columns[e.ColumnIndex] == ColumnCommentsRank)
            {
                CommentsCollection collection = dataGridViewComments.Rows[e.RowIndex].Tag as CommentsCollection;
                Rectangle r = dataGridViewComments.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                Point pt = Cursor.Position;
                pt = dataGridViewComments.PointToClient(  pt );
                if (pt.Y > r.Top && pt.Y < r.Bottom)
                {
                    if (pt.X > r.Left && pt.X < r.Left + 20)
                    {
                        TaxonComments.Manager.MoveUp(dataGridViewComments.Rows[e.RowIndex].Tag as CommentsCollection);
                        TaxonComments.Manager.SaveCollections();
                        FillCollectionsComments();
                    }
                    if (pt.X > r.Right - 20 && pt.X < r.Right)
                    {
                        TaxonComments.Manager.MoveDown(dataGridViewComments.Rows[e.RowIndex].Tag as CommentsCollection);
                        TaxonComments.Manager.SaveCollections();
                        FillCollectionsComments();
                    }
                }
            }
        }

        bool _SuspendCellValueChanged = false;
        private void dataGridViewImages_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_SuspendCellValueChanged) return;
            if (e.RowIndex < 0 || e.RowIndex >= dataGridViewImages.Rows.Count) return;

            if (dataGridViewImages.Columns[e.ColumnIndex] == ColumnImageDefault)
            {
                ImageCollection defaultCollection = dataGridViewImages.Rows[e.RowIndex].Tag as ImageCollection;
                SetDefaultCollection(defaultCollection);
            }
        }

        private void dataGridViewComments_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }

        

        private void SetDefaultCollection(ImageCollection _collection)
        {
            _SuspendCellValueChanged = true;
            foreach (DataGridViewRow row in dataGridViewImages.Rows)
            {
                ImageCollection collection = row.Tag as ImageCollection;
                collection.IsDefault = collection == _collection;
                row.Cells["ColumnImageDefault"].Value = collection.IsDefault;
            }
            _SuspendCellValueChanged = false;
        }

        private void tabControlCollections_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlCollections.SelectedTab == tabPageComments)
                dataGridViewComments_SelectionChanged(dataGridViewComments, null);
            if (tabControlCollections.SelectedTab == tabPageImages)
                dataGridViewImages_SelectionChanged(dataGridViewComments, null);

        }
    }
}
