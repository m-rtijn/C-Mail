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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace C_Mail_2._0
{
    /// <summary>
    /// Interaction logic for EncryptionPasswordPopup.xaml
    /// </summary>
    public partial class EncryptionPasswordPopup : Window
    {
        public static string EncryptionPassword;
        public EncryptionPasswordPopup()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            EncryptionPassword = EncryptionPasswordPasswordBox.Password;

            // Decrypt and read the data
            MainWindow.ReadCredentialsFromFile("Credentials", EncryptionPassword);

            // Close this window
            Close();
        }
    }
}
