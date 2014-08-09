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

namespace C_Mail_2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string Host = "";
        public static int Port = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Button methods

        private void ToAddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SubjectTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BodyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPopup loginPopup = new LoginPopup();
            loginPopup.Show();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string ToAddress, Subject, Body, FromAddress, FromPassword;

            // Assign the variables
            ToAddress = ToAddressTextBox.Text;
            Subject = SubjectTextBox.Text;
            Body = BodyTextBox.Text;
            FromAddress = LoginPopup.LoginFromAddress;
            FromPassword = LoginPopup.LoginFromPassword;

            // Call SendEmail to send the email
            SendEmail(ToAddress, FromAddress, FromPassword, Subject, Body);
        }

        // Popup call methods

        /// <summary>
        /// Shows the EmailIsSentPopup popup
        /// </summary>
        private void EmailIsSentPopupCall()
        {
            // Create a new instance of the class
            EmailIsSentPopup popup = new EmailIsSentPopup();
            // Show the popup
            popup.Show();
        }

        /// <summary>
        /// Shows an error popup, not the best thing to see...
        /// </summary>
        /// <param name="ErrorMessage">the error message</param>
        private void ErrorPopupCall(string ErrorMessage)
        {
            // Create a new instance of the ErrorPopup class
            ErrorPopup Error = new ErrorPopup();

            // Add the content of the ErrorLabel
            Error.ErrorLabel.Content = ErrorMessage;

            // Show the error
            Error.Show();
        }

        // Email methods

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="ToAddress">The address of the recipient of the email</param>
        /// <param name="FromAddress">The address of the sender of the email</param>
        /// <param name="FromPass">The password of the sender of the email</param>
        /// <param name="subject">The subject of the email</param>
        /// <param name="body">The body of the email</param>
        private void SendEmail(string ToAddress, string FromAddress, string FromPass, string subject, string body)
        {
            // First check the host and if the enterd FromAddress is actually an address
            if (CheckEmailHost(FromAddress) == true)
            {
                // Create a new instance of the SmtpClient class
                SmtpClient smtpClient = new SmtpClient
                {
                    Host = Host,
                    Port = Port,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(FromAddress, FromPass),
                    Timeout = 20000
                };

                // Create a new MailMessage, called Message, and add the properties
                MailMessage Message = new MailMessage(FromAddress, ToAddress, subject, body);

                // Send the message
                smtpClient.Send(Message);

                // Cleanup
                Message.Dispose();

                // Call this method to notify the user that the message has been sent
                EmailIsSentPopupCall();
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Checks the host and if the enterd FromAddress is actually an address.
        /// </summary>
        /// <param name="FromAddress">The address of the sender of the email</param>
        private bool CheckEmailHost(string FromAddress) // Checks the host
        {
            // First split the FromAddress between the @
            string[] splitFromAddress = FromAddress.Split('@');

            // Then check if the splitFromAddress[1] exists
            if (splitFromAddress.Length == 2)
            {
                // This switch checks which host it is, and assigns the Host and Port variables to the corresponding Host and Port
                switch(splitFromAddress[1])
                {
                    default:
                        ErrorPopupCall("ERROR: 30002" + "\n" + "Description: reached default in switch(splitFromAddres[1])");
                        return false;
                    case "gmail.com":
                        Host = "smtp.gmail.com";
                        Port = 587;
                        return true;
                }
            }
            else
            {
                ErrorPopupCall("ERROR: 30001" + "\n" + "Description: splitFromAddress[1] does not exists.");
                return false;
            }

        }
    }
}
