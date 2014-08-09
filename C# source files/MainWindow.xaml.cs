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
        public MainWindow()
        {
            InitializeComponent();
        }

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

            // Call the SendEmail method
            SendEmail(ToAddress, FromAddress, FromPassword, Subject, Body);
        }
        private void SendEmail(string ToAddress, string FromAddress, string FromPass, string subject, string body) // Sends an email
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(FromAddress, FromPass),
                Timeout = 20000
            };

            using (var message = new MailMessage(FromAddress, ToAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
            EmailIsSentPopupCall();
        }
        private void EmailIsSentPopupCall()
        {
            EmailIsSentPopup popup = new EmailIsSentPopup();
            popup.Show();
        }
    }
}
