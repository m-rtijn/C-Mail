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
    /// Interaction logic for AddAttachmentPopup.xaml
    /// </summary>
    public partial class AddAttachmentPopup : Window
    {
        public static string AttachmentPath;
        public AddAttachmentPopup()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            AttachmentPath = AttachmentFilepathTextBox.Text;
            Close();
        }
    }
}
