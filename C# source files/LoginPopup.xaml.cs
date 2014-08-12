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
using System.IO;

namespace C_Mail_2._0
{
    /// <summary>
    /// Interaction logic for LoginPopup.xaml
    /// </summary>
    public partial class LoginPopup : Window
    {
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
            // Assign the variables
            Program.FromAddress = FromAddressTextBox.Text;
            Program.FromPass = FromPasswordPasswordBox.Password;
            string EncryptionPassword = EncryptionPasswordPasswordBox.Password;

            // Save the details if the user wants so
            if (RememberDetailsCheckBoxState == true)
            {
                try
                {
                    Program.WriteCredentialsToFile(Program.FromAddress, Program.FromPass, "Credentials", EncryptionPassword);
                }
                catch(Exception exception)
                {
                    // Create the ErrorMessage
                    string ErrorMessage = "ERROR 50001:" + "\n" + exception.ToString();

                    // Show the ErrorMessage to the user
                    Program.ErrorPopupCall(ErrorMessage);

                    // Stop executing this method
                    return;
                }
            }

            // If the login is succesfull, show the popup
            if (!(string.IsNullOrEmpty(Program.FromAddress)) && !(string.IsNullOrEmpty(Program.FromPass)))
            {
                Program.LoggedInPopupCall();
            }

            // Close the window
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
