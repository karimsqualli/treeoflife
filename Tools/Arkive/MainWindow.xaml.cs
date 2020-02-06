using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Arkive
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _VM;
        }

        MainViewModel _VM = new MainViewModel();
        Config Config = null;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Config = Config.Load();
            Config.MainWindowPlacementToUI(new WindowInteropHelper(this).Handle);
            _VM.OutputFolder = Config.OutputFolder;
            Helpers.VM = _VM;
            Helpers.ClearLog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Config.MainWindowPlacementFromUI(new WindowInteropHelper(this).Handle);
            Config.OutputFolder = _VM.OutputFolder;
            Config.Save();
        }

        private void ButtonOutputFolder_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    _VM.OutputFolder = dialog.SelectedPath;
            }
        }

        private void ButtonOpenOutputFolder_Click(object sender, RoutedEventArgs e)
        {
            VinceToolbox.exeFunctions.openFile(_VM.OutputFolder);
        }

        private void ButtonOpenLog_Click(object sender, RoutedEventArgs e)
        {
            if (!System.IO.File.Exists(_VM.LogFilename))
                MessageBox.Show(_VM.LogFilename + " not found!");
            else
                VinceToolbox.exeFunctions.openFile(_VM.LogFilename);
        }

        private void ButtonOpenSpecies_Click(object sender, RoutedEventArgs e)
        {
            if (!System.IO.File.Exists(_VM.SpeciesFilename))
                MessageBox.Show(_VM.SpeciesFilename + " not found!");
            else
                VinceToolbox.exeFunctions.openFile(_VM.SpeciesFilename);
        }

        private void ButtonOpenImages_Click(object sender, RoutedEventArgs e)
        {
            if (!System.IO.File.Exists(_VM.ImageUrlsFilename))
                MessageBox.Show(_VM.ImageUrlsFilename + " not found!");
            else
                VinceToolbox.exeFunctions.openFile(_VM.ImageUrlsFilename );
        }

        private void ButtonOpenImageCsv_Click(object sender, RoutedEventArgs e)
        {
            if (!System.IO.File.Exists(_VM.ImageCsvFilename))
                MessageBox.Show(_VM.ImageCsvFilename + " not found!");
            else
                VinceToolbox.exeFunctions.openFile(_VM.ImageCsvFilename);
        }

        private void ButtonGetSpecies_Click(object sender, RoutedEventArgs e)
        {
            _VM.Species = ArkiveSpecies.Get(_VM.UseSpeciesLocalFile, _VM.SpeciesFilename);
        }

        private void ButtonGetImageUrls_Click(object sender, RoutedEventArgs e)
        {
            _VM.ImagesUrls = ArkiveImageUrls.Get(_VM.Species, _VM.UseImageUrlsLocalFile, _VM.ImageUrlsFilename);
            ArkiveImageUrls.Normalize(_VM.ImagesUrls);
            Helpers.LogDicoResult(_VM.Species, _VM.ImagesUrls, null);
        }

        private void ButtonGenerateCsv_Click(object sender, RoutedEventArgs e)
        {
            ArkiveImageUrls.GenerateCsv(_VM.ImagesUrls, _VM.ImageCsvFilename);
        }

        private void ButtonGetLocalImages_Click(object sender, RoutedEventArgs e)
        {
            _VM.LocalImages = ArkiveLocalImages.Get(_VM.OutputFolder);
            Helpers.LogDicoResult(_VM.Species, null, _VM.LocalImages);
        }

        private void ButtonComputeDiff_Click(object sender, RoutedEventArgs e)
        {
            _VM.Diff = ArkiveImages.ComputeDiff(_VM.ImagesUrls, _VM.LocalImages);
            //Helpers.LogDiff(_VM.Diffs);
        }

        private void ButtonRemoveOld_Click(object sender, RoutedEventArgs e)
        {
            ArkiveImages.RemoveOld(_VM.Diff);
        }

        private void ButtonGetNew_Click(object sender, RoutedEventArgs e)
        {
            ArkiveImages.GetMissings(_VM.Diff);
        }
    }
}
