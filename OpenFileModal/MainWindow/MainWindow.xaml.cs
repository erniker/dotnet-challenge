using System;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using Refactoring.FraudDetection;
using System.Text;
using System.Windows.Media;

namespace OpenFileModal.MainWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string contentFile;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            // Restore normal color
            txtContent.Foreground = Brushes.Black;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
                txtContent.Text = File.ReadAllText(openFileDialog.FileName);

            contentFile = openFileDialog.FileName;
        }

        private void btnCheckFile_Click(object sender, RoutedEventArgs e)
        {
            // Call to FraudRadar
            var resultsList = new FraudRadar().Check(contentFile);

            string initString = string.Format("The following registers are fraudulent:{0}", Environment.NewLine);

            StringBuilder fraudResults = new StringBuilder(initString);

            foreach (var row in resultsList)
            {
                string line = string.Format("OrderID: {0} {1}", row.OrderId, Environment.NewLine);
                fraudResults.Append(line);
            }

            // Set red color to result
            txtContent.Foreground = Brushes.Red;

            // Show fraud results
            txtContent.Text = fraudResults.ToString();

        }
    }
}