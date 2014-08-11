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

            // Apperantly MessageCount is 0. fml.
            EmailBodyTextBox.Text = Program.MessageCount.ToString();
            
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of LoginPopup
            LoginPopup popup = new LoginPopup();

            // Shot it
            popup.Show();
        }
    }
}
