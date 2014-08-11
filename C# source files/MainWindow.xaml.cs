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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace C_Mail_2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
        public MainWindow()
        {
            InitializeComponent();
        }

        // Button and Textbox methods

        private void ToAddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SubjectTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BodyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Exits the program when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// When the AddAttachmentButton is clicked, show the AddAttachmentpopup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the AddAttachmentPopup
            AddAttachmentPopup popup = new AddAttachmentPopup();

            // Show the popup
            popup.Show();
        }

        /// <summary>
        /// Logs in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // If the credentials are saved, open those, else open the login popup
            if (File.Exists("Credentials") == true)
            {
                // Ask the Encryption password using the EncryptionPasswordPopup
                EncryptionPasswordPopup popup = new EncryptionPasswordPopup();
                
                //Show the popup
                popup.Show();
            }
            else
            {
                // Create a new instance of the LoginPopup class
                LoginPopup loginPopup = new LoginPopup();

                // Show the popup
                loginPopup.Show();
            }
        }

        /// <summary>
        /// Prepares variables to call SendEmail to and the email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string ToAddress, Subject, Body, CC, S_Attachment;

            // Assign the variables
            ToAddress = ToAddressTextBox.Text;
            Subject = SubjectTextBox.Text;
            Body = BodyTextBox.Text;
            CC = CCAddressTextBox.Text;
            S_Attachment = AddAttachmentPopup.AttachmentPath;

            // Call SendEmail to send the email
            Program.SendEmail(ToAddress, Program.FromAddress, Program.FromPass, Subject, Body, CC, S_Attachment);
        }
    }
}
