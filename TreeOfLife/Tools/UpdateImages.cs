using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using TreeOfLife.Controls;
using TreeOfLife.TaxonDialog;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TreeOfLife.Tools
{
    [Description("Update Images list on Espece and Sub espece taxon")]
    [DisplayName("Update images")]
    [Category("Tools/Images")]
    [PresentInMode(true, true)]
    class UpdateImages: ITool 
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            DateTime ts_Start = DateTime.Now;

            using (ProgressDialog progressDlg = new ProgressDialog())
            {
                progressDlg.StartPosition = FormStartPosition.CenterScreen;
                progressDlg.Show();

                // retrouve la liste de toutes les especes / sous especes
                ProgressItem piPrepare = progressDlg.Add("Prepare ...", "get species", 0, 4);
                List<TaxonTreeNode> Species = new List<TaxonTreeNode>();
                TaxonUtils.OriginalRoot.GetAllChildrenRecursively(Species, ClassicRankEnum.Espece);
                piPrepare.Update(1, "get sub species");
                TaxonUtils.OriginalRoot.GetAllChildrenRecursively(Species, ClassicRankEnum.SousEspece);
                piPrepare.Update(2, "init");

                int beforeWith = 0;
                int beforeImages = 0;
                int beforeWithout = 0;

                DateTime ts_StartDico = DateTime.Now;
                Dictionary<string, TaxonTreeNode> dico = new Dictionary<string, TaxonTreeNode>();
                foreach (TaxonTreeNode node in Species)
                {
                    if (node.Desc.Images != null && node.Desc.Images.Count != 0)
                    {
                        beforeWith++;
                        beforeImages += node.Desc.Images.Count;
                    }
                    else
                        beforeWithout++;

                    dico[node.Desc.RefMultiName.Main.ToLower()] = node;
                }

                piPrepare.Update(3, "clear");
                TaxonUtils.OriginalRoot.ParseNodeDesc((d) => { d.Images = null; });
                piPrepare.Update(4, "end");
                piPrepare.End();

                DateTime ts_StartParseCollection = DateTime.Now;

                List<string> badFormatFilenames = new List<string>();

                ProgressItem piCollections = progressDlg.Add("Parse collections ...", "", 0, TaxonImages.Manager.CollectionsEnumerable().Count());
                foreach (ImageCollection collection in TaxonImages.Manager.CollectionsEnumerable())
                {
                    piCollections.Update(piCollections.Current + 1, collection.Name);

                    if (!Directory.Exists(collection.Path)) continue;
                    IEnumerable<string> files = Directory.EnumerateFiles(collection.Path, "*.jpg");

                    int count = 10000;
                    Task getCountTask = Task.Factory.StartNew(() => { count = files.Count(); });

                    int counter = 0;
                    ProgressItem piPhotos = progressDlg.Add("Parse " + collection.Name + " photos ...", "", 0, count - 1);
                    foreach (string file in files)
                    {
                        string name = Path.GetFileNameWithoutExtension(file);

                        counter++;
                        if (counter == 10)
                        {
                            counter = 0;
                            if (getCountTask != null)
                            {
                                if (getCountTask.IsCompleted)
                                {
                                    piPhotos.Max = count - 1;
                                    getCountTask = null;
                                }
                                else if (piPhotos.Max < piPhotos.Current + 10)
                                    piPhotos.Max += 10000;
                            }
                            piPhotos.Update(piPhotos.Current + 10, name);
                        }

                        TaxonImages.SplitImageFilenameResult result = TaxonImages.Manager.SplitImageFilename(file, false, collection);
                        if (result == null)
                        {
                            badFormatFilenames.Add(file);
                            continue;
                        }

                        name = result.TaxonName.ToLower();
                        if (dico.ContainsKey(name))
                        {
                            if (dico[name].Desc.Images == null)
                                dico[name].Desc.Images = new List<TaxonImageDesc>();
                            dico[name].Desc.Images.Add(result.Desc);
                        }
                    }
                    piPhotos.End();

                    // treat xmls
                    foreach( var pair in collection.AllLinks)
                    {
                        ImagesLinks list = pair.Value;

                        for (int i = 0; i < list.Count; i++)
                        {
                            ImageLink link = list[i];
                            if (dico.TryGetValue(link.Key.ToLower(), out TaxonTreeNode node))
                            {
                                TaxonImageDesc desc = new TaxonImageDesc
                                {
                                    CollectionId = collection.Id,
                                    Secondary = false,
                                    LinksId = pair.Key,
                                    Index = i
                                };

                                if (node.Desc.Images == null)
                                    node.Desc.Images = new List<TaxonImageDesc>();
                                node.Desc.Images.Add(desc);
                            }
                        }
                    }

                    foreach (var reference in collection.DistantReferences)
                    {

                        if (dico.TryGetValue(reference.TaxonName.ToLower(), out TaxonTreeNode node))
                        {
                            TaxonImageDesc desc = new TaxonImageDesc
                            {
                                CollectionId = collection.Id,
                                Secondary = false,
                                LinksId = 0,
                                Index = reference.Index
                            };

                            if (node.Desc.Images == null)
                                node.Desc.Images = new List<TaxonImageDesc>();
                            node.Desc.Images.Add(desc);
                        }
                    }
                }

                DateTime ts_StartFinalise = DateTime.Now;

                int afterWith = 0;
                int afterImages = 0;
                int afterWithout = 0;

                foreach (TaxonTreeNode node in Species)
                {
                    if (node.Desc.Images != null)
                    {
                        afterWith++;
                        afterImages += node.Desc.Images.Count;
                    }
                    else
                        afterWithout++;
                }

                DateTime ts_End = DateTime.Now;

                string message = "Update image flag : \n\n";
                message += String.Format("    Total with image     : {0} referencing {1} images, (before: {2} referencing {3} images)\n", afterWith, afterImages, beforeWith, beforeImages);
                message += String.Format("    Total without image  : {0}, (before: {1})\n\n", afterWithout, beforeWithout);
                message += String.Format("    Badly formatted files: {0}\n", badFormatFilenames.Count);
                message += String.Format("    {0} ms to get taxons\n", (ts_StartDico - ts_Start).Milliseconds);
                message += String.Format("    {0} ms to fill dico\n", (ts_StartParseCollection - ts_StartDico).Milliseconds);
                message += String.Format("    {0} ms to parse collections\n", (ts_StartFinalise - ts_StartParseCollection).Milliseconds);
                message += String.Format("    {0} ms to finalize taxons\n", (ts_End - ts_StartFinalise).Milliseconds);
                message += String.Format("Dumped in UpdateImages.log file");
                Loggers.WriteInformation(LogTags.Image, message);

                string logfile = Path.Combine(TaxonUtils.GetLogPath(), "UpdateImages.log");
                if (File.Exists(logfile)) File.Delete(logfile);
                using (StreamWriter log = new StreamWriter(logfile))
                {
                    log.WriteLine("UpdateImages result ( " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " )\n");
                    log.WriteLine(message);
                    log.WriteLine("");
                    log.WriteLine("Files with bad format names detected:\n");
                    foreach (string filename in badFormatFilenames)
                        log.WriteLine("    " + filename);
                }
            }

            TaxonUtils.OriginalRoot.UpdateAvailableImages();
            TaxonControlList.OnAvailableImagesChanged();
        }
    }
}
