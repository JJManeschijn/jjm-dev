using System.Windows;
using res = OutsuranceTask1.Resources.Resources;

namespace OutsuranceTask1
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        #region variables

        ProcessFile _ProcessFile = null;
        bool _IsNamesSaved = false;
        bool _IsAddressesSaved = false;

        #endregion

        public Main()
        {
            InitializeComponent();
        }

        void InitializeProcessFile()
        {
            if (_ProcessFile == null)
            {
                _ProcessFile = new ProcessFile();
            }
        }

        bool ValidateCSV()
        {
            bool isValid = true;
            InitializeProcessFile();
            string result = _ProcessFile.ValidateCSV(txtInputFile.Text);
            if (!string.IsNullOrEmpty(result))
            {
                MessageBox.Show(result, res.OUTsuranceAssessment, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                isValid = false;
            }
            return isValid;
        }

        bool VaildateFileHeader()
        {
            bool isValid = true;
            InitializeProcessFile();
            string result = _ProcessFile.ValidateCSVHeader(txtInputFile.Text);
            if (!string.IsNullOrEmpty(result))
            {
                if (result == "True")
                {
                    chkHasHeader.IsChecked = true;
                }
                else
                {
                    MessageBox.Show(result, res.OUTsuranceAssessment, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    isValid = false;
                }
            }
            return isValid;
        }

        bool ProcessCSV()
        {
            _IsNamesSaved = false;
            _IsAddressesSaved = false;
            bool isSuccess = true;
            string result = string.Empty;
            InitializeProcessFile();
            result = _ProcessFile.ProcessCSV(txtInputFile.Text, (bool)chkHasHeader.IsChecked);
            if (!string.IsNullOrEmpty(result))
            {
                MessageBox.Show(result, res.OUTsuranceAssessment, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return isSuccess;
        }


        bool CanClose()
        {
            bool result = true;
            if ((!_IsNamesSaved) | (!_IsAddressesSaved))
            {
                if (MessageBox.Show(res.SaveWarning, res.OUTsuranceAssessment, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    result = false;
                }
            }
            return result;
        }

        void CloseApplication()
        {
            Application.Current.MainWindow.Close();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex++;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            //create openfile dialog
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            //set filter & initial path
            openFileDialog.Filter = res.CSVFormat;
            openFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            //show dialog
            bool? dialogResult = openFileDialog.ShowDialog();

            //get selected filename
            if (dialogResult == true)
            {
                txtInputFile.Text = openFileDialog.FileName;
            }

            //check if entered or selected file is valid
            if (ValidateCSV())
            {
                //check if it contains a header and set check box
                if (VaildateFileHeader())
                {
                    tbBody1_1.Visibility = Visibility.Visible;
                    chkHasHeader.Visibility = Visibility.Visible;
                }
            }
        }


        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessCSV())
            {
                MainTabControl.SelectedIndex++;
            }
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex--;
        }

        private void btnSaveNames_Click(object sender, RoutedEventArgs e)
        {
            //create openfile dialog
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();

            //set filter & initial path
            saveFileDialog.Filter = res.TXTFormat;
            saveFileDialog.Title = res.SaveNames;
            saveFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            //show dialog
            bool? dialogResult = saveFileDialog.ShowDialog();

            //get selected filename
            if (dialogResult == true)
            {
                string result = _ProcessFile.ExportNames(saveFileDialog.FileName);
                if (!string.IsNullOrEmpty(result))
                {
                    MessageBox.Show(result, res.OUTsuranceAssessment, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _IsNamesSaved = true;
                }
            }
        }

        private void btnSaveAddresses_Click(object sender, RoutedEventArgs e)
        {
            //create openfile dialog
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();

            //set filter & initial path
            saveFileDialog.Filter = res.TXTFormat;
            saveFileDialog.Title = res.SaveAddresses;
            saveFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            //show dialog
            bool? dialogResult = saveFileDialog.ShowDialog();

            //get selected filename
            if (dialogResult == true)
            {
                string result = _ProcessFile.ExportAddresses(saveFileDialog.FileName);
                if (!string.IsNullOrEmpty(result))
                {
                    MessageBox.Show(result, res.OUTsuranceAssessment, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _IsAddressesSaved = true;
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (CanClose())
            {
                CloseApplication();
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            CloseApplication();
        }
    }
}
