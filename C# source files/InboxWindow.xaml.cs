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
using OpenPop.Pop3;
using OpenPop.Mime;

namespace C_Mail_2._0
{
    /// <summary>
    /// Interaction logic for InboxWindow.xaml
    /// </summary>
    public partial class InboxWindow : Window
    {
        public InboxWindow()
        {
            InitializeComponent();
        }

        // Button _Click methods

        private void SendEmailButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of MainWindow class
            MainWindow SendMail = new MainWindow();
            
            // Show it
            SendMail.Show();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a list to store the messages in
            List<Message> AllMessages = new List<Message>();

            // Retrieve all the messages
            AllMessages = Program.RetrieveAllMessages("pop.gmail.com", 995, Program.FromAddress, Program.FromPass, true);

            try
            {
                // Show the sender of the last received email
                FromTextBox.Text = AllMessages[Program.MessageCount].Headers.Sender.ToString();

                // Show the subject of the last received email
                SubjectTextBox.Text = AllMessages[Program.MessageCount].Headers.Subject.ToString();

                // Show the body of the last received email
                EmailBodyTextBox.Text = AllMessages[Program.MessageCount].MessagePart.ToString();
            }
            catch (Exception exception)
            {
                // Create the error message
                string ErrorMessage = "ERROR 70001" + "\n" + exception.ToString();

                // Show the error message
                Program.ErrorPopupCall(ErrorMessage);

                // Stop executing this method
                return;
            }
        }

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
    }
}
