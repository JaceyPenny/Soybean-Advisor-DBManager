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

namespace Soybean_Advisor_Database_Manager
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public string DialogText
        {
            get
            {
                return txtDialog.Text;
            }

            set
            {
                txtDialog.Text = value;
            }
        }

        public InputDialog()
        {
            InitializeComponent();
        }

        public InputDialog(string title, string prompt)
        {
            InitializeComponent();
            this.Title = title;
            lblPrompt.Content = prompt;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.ToString().Equals("OK"))
            {
                this.DialogResult = true;
            }
            else
            {
                this.DialogResult = false;
            }
        }
    }
}
