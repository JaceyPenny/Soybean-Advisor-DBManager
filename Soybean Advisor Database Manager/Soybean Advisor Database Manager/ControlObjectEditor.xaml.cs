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
    /// Interaction logic for ControlObjectEditor.xaml
    /// </summary>
    public partial class ControlObjectEditor : Window
    {
        public Control CurrentControl;

        public ControlObjectEditor()
        {
            InitializeComponent();
            CurrentControl = new Control();
        }

        public ControlObjectEditor(Control c)
        {
            InitializeComponent();
            CurrentControl = c;

            txtName.Text = c.Name;
            txtActive.Text = c.Active;
            txtFRAC.Text = c.FRAC;
            txtRate.Text = c.Rate;
            txtHarvest.Text = c.Harvest;
            txtComments.Text = c.Comments;

            txtName.TextChanged += textChanged;
            txtActive.TextChanged += textChanged;
            txtFRAC.TextChanged += textChanged;
            txtRate.TextChanged += textChanged;
            txtHarvest.TextChanged += textChanged;
            txtComments.TextChanged += textChanged;
        }

        private void textChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Equals(txtName))
            {
                CurrentControl.Name = txtName.Text;
            }
            else if (((TextBox)sender).Equals(txtActive))
            {
                CurrentControl.Active = txtActive.Text;
            }
            else if (((TextBox)sender).Equals(txtFRAC))
            {
                CurrentControl.FRAC = txtFRAC.Text;
            }
            else if (((TextBox)sender).Equals(txtRate))
            {
                CurrentControl.Rate = txtRate.Text;
            }
            else if (((TextBox)sender).Equals(txtHarvest))
            {
                CurrentControl.Harvest = txtHarvest.Text;
            }
            else if (((TextBox)sender).Equals(txtComments))
            {
                CurrentControl.Comments = txtComments.Text;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
