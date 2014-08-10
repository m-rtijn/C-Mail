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
using System.Security;

namespace C_Mail_2._0
{
    /// <summary>
    /// Interaction logic for LoginPopup.xaml
    /// </summary>
    public partial class LoginPopup : Window
    {
        public static string LoginFromAddress, LoginFromPassword, EncryptionPassword;
        public static bool RememberDetailsCheckBoxState = false;
        public LoginPopup()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginFromAddress = FromAddressTextBox.Text;
            LoginFromPassword = FromPasswordPasswordBox.Password;
            EncryptionPassword = EncryptionPasswordPasswordBox.Password;
            Close();
        }
        private void HandleCheck(object sender, RoutedEventArgs e)
        {
            RememberDetailsCheckBoxState = true;

            // Show the EncryptionPasswordLabel and PasswordBox
            EncryptionPasswordLabel.Visibility = Visibility.Visible;
            EncryptionPasswordPasswordBox.Visibility = Visibility.Visible;
        }
        private void HandleUncheck(object sender, RoutedEventArgs e)
        {
            RememberDetailsCheckBoxState = false;

            // Hide the EncryptionPasswordLabel and PasswordBox
            EncryptionPasswordLabel.Visibility = Visibility.Hidden;
            EncryptionPasswordPasswordBox.Visibility = Visibility.Hidden;
        }
        private void FromAddressTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
