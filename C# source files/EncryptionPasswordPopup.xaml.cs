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
        public EncryptionPasswordPopup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Closes this window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Reads and decrypts stored login credentials using ReadCredentialsFromFile(string Path, string EncryptionPassword)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Assign EncryptionPassword
            string EncryptionPassword = EncryptionPasswordPasswordBox.Password;

            // Decrypt and read the data
            try
            {
                Program.ReadCredentialsFromFile("Credentials", EncryptionPassword);
            }
            catch(Exception exception)
            {
                // Create the ErrorMessage
                string ErrorMessage = "ERROR 50002:" + "\n" + exception.ToString();

                // Show the ErrorMessage to the user
                Program.ErrorPopupCall(ErrorMessage);

                // Stop executing this method
                return;
            }

            // Close this window
            Close();
        }
    }
}
