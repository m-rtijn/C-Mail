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
        private static string Host = "";
        private static int Port = 0;
        public static string FromAddress, FromPass;
        
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ReportAnIssueButton_Click(object sender, RoutedEventArgs e)
        {
            // Opens a new tab in your default browser where you can report your issue.
            Process.Start("https://github.com/Tijndagamer/C-Mail/issues/new");
        }

        private void AddAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            AddAttachmentPopup popup = new AddAttachmentPopup();
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
            SendEmail(ToAddress, FromAddress, FromPass, Subject, Body, CC, S_Attachment);
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
            popup.Activate();
        }

        /// <summary>
        /// Shows an error popup, not the best thing to see...
        /// </summary>
        /// <param name="ErrorMessage">The error message</param>
        public static void ErrorPopupCall(string ErrorMessage)
        {
            // Create a new instance of the ErrorPopup class
            ErrorPopup Error = new ErrorPopup();

            // Add the content of the ErrorLabel
            Error.ErrorLabel.Content = ErrorMessage;

            // Show the error
            Error.Show();
            Error.Activate();
        }

        // Email methods

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="ToAddress">The address of the recipient of the email</param>
        /// <param name="FromAddress">The address of the sender of the email</param>
        /// <param name="FromPass">The password of the sender of the email</param>
        /// <param name="Subject">The subject of the email</param>
        /// <param name="Body">The body of the email</param>
        private void SendEmail(string ToAddress, string FromAddress, string FromPass, string Subject, string Body, string CC, string S_Attachment)
        {
            // First check if all the TextBoxes are filled in and then check the host.
            if (CheckArguments(ToAddress, FromAddress, FromPass, Subject, Body) == true && CheckEmailHost(FromAddress) == true)
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
                MailMessage Message = new MailMessage(FromAddress, ToAddress, Subject, Body);

                // If there's something in the CC tab, add it to Message
                if (!(string.IsNullOrEmpty(CC)))
                {
                    // First convert string CC to MailAddress CC
                    MailAddress Copy = new MailAddress(CC);

                    // Add the Copy to the message
                    Message.CC.Add(Copy);
                }

                // If there's something in the S_Attachment, add it to Message
                if (!(string.IsNullOrEmpty(S_Attachment)))
                {
                    // Create a new Attachment
                    Attachment File = new Attachment(S_Attachment);

                    // Add it to Message
                    Message.Attachments.Add(File);
                }

                // Send the message
                smtpClient.Send(Message);

                // Cleanup
                Message.Dispose();

                // Call this method to notify the user that the message has been sent
                EmailIsSentPopupCall();
            }
            else
            {
                // If not, stop the execution of this method.
                return;
            }
        }

        // Check methods

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
                    case "gmail.com":
                        Host = "smtp.gmail.com";
                        Port = 587;
                        return true;
                    case "yahoo.com":
                        Host = "smtp.mail.yahoo.com";
                        Port = 465;
                        return true;
                    default:
                        ErrorPopupCall("ERROR 30002" + "\n" + "Description: reached default in switch(splitFromAddres[1])");
                        return false;
                }
            }
            else
            {
                ErrorPopupCall("ERROR 30001" + "\n" + "Description: splitFromAddress[1] does not exists.");
                return false;
            }

        }

        /// <summary>
        /// Checks if all the arguments are not null nor empty
        /// </summary>
        /// <param name="ToAddress"></param>
        /// <param name="FromAddress"></param>
        /// <param name="FromPass"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private bool CheckArguments(string ToAddress, string FromAddress, string FromPass, string subject, string body)
        {
            // Check for all arguments if they're null or empty, of they all aren't null or empty return true else return false and an error message
            if (!(string.IsNullOrEmpty(FromAddress)) && !(string.IsNullOrEmpty(ToAddress)) && !(string.IsNullOrEmpty(FromPass)) && !(string.IsNullOrEmpty(subject)) && !(string.IsNullOrEmpty(body)))
            {
                return true;
            }
            else
            {
                ErrorPopupCall("ERROR 30003" + "\n" + "Description: one of the given arguments is null or empty.");
                return false;
            }
        }

        // Writing and Reading methods

        /// <summary>
        /// First encrypts, and then saves the login credentials to a file
        /// </summary>
        /// <param name="FromAddress">The FromAddress to be encrypted</param>
        /// <param name="FromPass">The FromPass to be encrypted</param>
        /// <param name="Path">The path of the file</param>
        /// <param name="EncryptionPassword">The password used for the encryption</param>
        public static void WriteCredentialsToFile(string FromAddress, string FromPass, string Path, string EncryptionPassword)
        {
            // Create a new instance of the FileStream class
            FileStream fileStream = File.OpenWrite(Path);

            // Create a new instance of he BinaryWriter class
            BinaryWriter writer = new BinaryWriter(fileStream);

            // Encrypt the FromAddress and Frompass
            string EncryptedFromAddress = EncryptionClass.Encrypt(FromAddress, EncryptionPassword);
            string EncryptedFromPass = EncryptionClass.Encrypt(FromPass, EncryptionPassword);

            // Write the credentials to the file
            writer.Write(EncryptedFromAddress);
            writer.Write(EncryptedFromPass);

            // Close the BinaryWriter
            writer.Close();
        }

        /// <summary>
        /// Reads and decrypts the FromAddress and Frompass from the file where they're saved
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="EncryptionPassword">The password used for the encryption</param>
        public static void ReadCredentialsFromFile(string Path, string EncryptionPassword)
        {
            // Create a new instance of the FileStream class
            FileStream fileStream = File.OpenRead(Path);

            // Create a new instance of the BinaryReader class
            BinaryReader reader = new BinaryReader(fileStream);

            // Read the file
            string EncryptedFromAddress = reader.ReadString();
            string EncryptedFromPass = reader.ReadString();

            // Create an array to store the decrypted data in
            string[] DecryptedData = new string[2];

            // Decrypt the FromAddress and FromPass
            DecryptedData[0] = EncryptionClass.Decrypt(EncryptedFromAddress, EncryptionPassword);
            DecryptedData[1] = EncryptionClass.Decrypt(EncryptedFromPass, EncryptionPassword);

            // Assign the variables
            FromAddress = DecryptedData[0];
            FromPass = DecryptedData[1];
        }


    }
}
