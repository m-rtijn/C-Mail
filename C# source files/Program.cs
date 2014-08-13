using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using S22.Imap;

// This is where the magic happens

namespace C_Mail_2._0
{
    class Program
    {
        // Global variables
        private static string Host = "";
        private static int Port = 0;
        public static string FromAddress, FromPass;

        // Popup call methods

        /// <summary>
        /// Shows the EmailIsSentPopup popup
        /// </summary>
        public static void EmailIsSentPopupCall()
        {
            // Create a new instance of the class
            EmailIsSentPopup popup = new EmailIsSentPopup();

            // Show the popup
            popup.Show();

            // Make the popup active
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

            // Make the error popup active
            Error.Activate();
        }

        public static void LoggedInPopupCall()
        {
            // Create a new instance of LoginConfirmedPopup 
            LoginConfirmedPopup popup = new LoginConfirmedPopup();

            // Show the popup
            popup.Show();
        }

        // Send email methods

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="ToAddress">The address of the recipient of the email</param>
        /// <param name="FromAddress">The address of the sender of the email</param>
        /// <param name="FromPass">The password of the sender of the email</param>
        /// <param name="Subject">The subject of the email</param>
        /// <param name="Body">The body of the email</param>
        public static void SendEmail(string ToAddress, string FromAddress, string FromPass, string Subject, string Body, string CC, string S_Attachment)
        {
            // First check if all the TextBoxes are filled in and then check the host.
            if (CheckArguments(ToAddress, FromAddress, FromPass, Subject, Body) == true && CheckEmailHost(FromAddress) == true)
            {
                // Create a new instance of the SmtpClient class
                SmtpClient smtpClient = new SmtpClient
                {
                    // Set the host
                    Host = Host,

                    // Set the port
                    Port = Port,

                    // Enable SSL
                    EnableSsl = true,

                    // Assign the delivery method
                    DeliveryMethod = SmtpDeliveryMethod.Network,

                    // Assign the credentials
                    Credentials = new NetworkCredential(FromAddress, FromPass),

                    // Set the timeout
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
                try
                {
                    smtpClient.Send(Message);
                }
                catch (Exception exception)
                {
                    // Create the ErrorMessage
                    string ErrorMessage = "ERROR 20001:" + "\n" + exception.ToString();

                    // Show the ErrorMessage to the user
                    ErrorPopupCall(ErrorMessage);

                    // Cleanup
                    Message.Dispose();

                    // Stop executing this method
                    return;
                }

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

        /// <summary>
        /// Checks the host and if the enterd FromAddress is actually an address.
        /// </summary>
        /// <param name="FromAddress">The address of the sender of the email</param>
        private static bool CheckEmailHost(string FromAddress)
        {
            // First split the FromAddress between the @
            string[] splitFromAddress = FromAddress.Split('@');

            // Then check if the splitFromAddress[1] exists
            if (splitFromAddress.Length == 2)
            {
                // This switch checks which host it is, and assigns the Host and Port variables to the corresponding Host and Port
                switch (splitFromAddress[1])
                {
                    case "gmail.com":
                        Host = "smtp.gmail.com";
                        Port = 587;
                        return true;
                    case "yahoo.com":
                        Host = "smtp.mail.yahoo.com";
                        Port = 465;
                        return true;
                    case "hotmail.com":
                        Host = "smtp.live.com";
                        Port = 587;
                        return true;
                    case "hotmail.nl":
                        Host = "smtp.live.com";
                        Port = 587;
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
        private static bool CheckArguments(string ToAddress, string FromAddress, string FromPass, string subject, string body)
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

        // Receive email methods

        /// <summary>
        /// Retrieves all the unseen messages from the server
        /// </summary>
        /// <param name="FromAddress">User's email address</param>
        /// <param name="FromPass">User's password</param>
        /// <returns>All the retrieved messages</returns>
        public static List<MailMessage> GetAllUnseenMessages(string FromAddress, string FromPass)
        {
            // Check the host
            if (CheckEmailHostIMAP(FromAddress) == true && !(string.IsNullOrEmpty(FromPass)))
            {
                try
                {
                    // Create a new ImapClient
                    ImapClient client = new ImapClient(Host, Port, FromAddress, FromPass, AuthMethod.Login, true);

                    // Get the uids
                    IEnumerable<uint> uids = client.Search(SearchCondition.Unseen());

                    // Get the messages
                    IEnumerable<MailMessage> Messages = client.GetMessages(uids);

                    // Convert to list
                    List<MailMessage> MessagesList = Messages.ToList<MailMessage>();

                    // Return them
                    return MessagesList;
                }
                catch (Exception exception)
                {
                    // Create the error message
                    string ErrorMessage = "ERROR 60003" + "\n" + exception.ToString();

                    // Show the error message
                    Program.ErrorPopupCall(ErrorMessage);

                    // Make an empty list to return
                    List<MailMessage> Stop = new List<MailMessage>();

                    // Stop executing this method
                    return Stop;
                }
            }
            else
            {
                // Create the error message
                string ErrorMessage = "ERROR 60003" + "\n" + "EmailHostIMAP(FromAddress) returned false";

                // Show the error message
                Program.ErrorPopupCall(ErrorMessage);

                // Make an empty list to return
                List<MailMessage> Stop = new List<MailMessage>();

                // Stop executing this method
                return Stop;
            }
        }

        /// <summary>
        /// Checks the host for the POP3 protocol used for the inbox
        /// </summary>
        /// <param name="FromAddress"></param>
        private static bool CheckEmailHostIMAP(string FromAddress)
        {
            // First split the FromAddress between the @
            string[] splitFromAddress = FromAddress.Split('@');

            // Then check if the splitFromAddress[1] exists
            if (splitFromAddress.Length == 2)
            {
                // This switch checks which host it is, and assigns the Host and Port variables to the corresponding Host and Port
                switch (splitFromAddress[1])
                {
                    case "gmail.com":
                        Host = "imap.gmail.com";
                        Port = 993;
                        return true;
                    case "yahoo.com":
                        Host = "-.mail.yahoo.com";
                        Port = 993;
                        return true;
                    case "hotmail.com":
                        Host = "-.live.com";
                        Port = 993;
                        return true;
                    case "hotmail.nl":
                        Host = "-.live.com";
                        Port = 993;
                        return true;
                    default:
                        ErrorPopupCall("ERROR 60004" + "\n" + "Description: reached default in switch(splitFromAddres[1])");
                        return false;
                }
            }
            else
            {
                ErrorPopupCall("ERROR 60005" + "\n" + "Description: splitFromAddress[1] does not exists.");
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
            // Declare variables
            string EncryptedFromAddress, EncryptedFromPass;

            // Create a new instance of the FileStream class
            FileStream fileStream = File.OpenWrite(Path);

            // Create a new instance of he BinaryWriter class
            BinaryWriter writer = new BinaryWriter(fileStream);

            // Encrypt the FromAddress and Frompass
            EncryptedFromAddress = EncryptionClass.Encrypt(FromAddress, EncryptionPassword);
            EncryptedFromPass = EncryptionClass.Encrypt(FromPass, EncryptionPassword);

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
