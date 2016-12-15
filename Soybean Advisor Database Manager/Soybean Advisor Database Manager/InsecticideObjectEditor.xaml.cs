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
    /// Interaction logic for InsecticideObjectEditor.xaml
    /// </summary>
    public partial class InsecticideObjectEditor : Window
    {
        Insecticide CurrentInsecticide;

        public InsecticideObjectEditor()
        {
            InitializeComponent();
        }

        public InsecticideObjectEditor(Insecticide i)
        {
            InitializeComponent();
            CurrentInsecticide = i;

            textBoxName.Text = i.Name;
            textBoxName.TextChanged += delegate
            {
                CurrentInsecticide.Name = textBoxName.Text;
            };

            textBoxOtherName.Text = i.OtherName;
            textBoxOtherName.TextChanged += delegate
            {
                CurrentInsecticide.OtherName = textBoxOtherName.Text;
            };

            textBoxFormulation.Text = i.Formulation;
            textBoxFormulation.TextChanged += delegate
            {
                CurrentInsecticide.Formulation = textBoxFormulation.Text;
            };

            textBoxActive.Text = i.Active;
            textBoxActive.TextChanged += delegate
            {
                CurrentInsecticide.Active = textBoxActive.Text;
            };

            textBoxAcres.Text = i.Acres;
            textBoxAcres.TextChanged += delegate
            {
                CurrentInsecticide.Acres = textBoxAcres.Text;
            };

            textBoxMinDays.Text = (i.MinimumDays <= 0) ? "" : "" + i.MinimumDays;
            textBoxMinDays.TextChanged += delegate
            {
                try
                {
                    if (!textBoxMinDays.Text.Equals(""))
                        CurrentInsecticide.MinimumDays = int.Parse(textBoxMinDays.Text);
                    else
                        CurrentInsecticide.MinimumDays = -1;
                }
                catch
                {
                    textBoxMinDays.Text = (i.MinimumDays <= 0) ? "" : "" + i.MinimumDays;
                }
            };

            textBoxPerformance.Text = (i.Performance <= 0) ? "" : "" + i.Performance;
            textBoxPerformance.TextChanged += delegate
            {
                try
                {
                    if (!textBoxPerformance.Text.Equals(""))
                        CurrentInsecticide.Performance = int.Parse(textBoxPerformance.Text);
                    else
                        CurrentInsecticide.Performance = -1;
                }
                catch
                {
                    textBoxPerformance.Text = (i.Performance <= 0) ? "" : "" + i.Performance;
                }
            };

            textBoxRestrictedInterval.Text = (i.EntryInterval <= 0) ? "" : "" + i.EntryInterval;
            textBoxRestrictedInterval.TextChanged += delegate
            {
                try
                {
                    if (!textBoxRestrictedInterval.Text.Equals(""))
                        CurrentInsecticide.EntryInterval = int.Parse(textBoxRestrictedInterval.Text);
                    else
                        CurrentInsecticide.EntryInterval = -1;
                }
                catch
                {
                    textBoxRestrictedInterval.Text = (i.EntryInterval <= 0) ? "" : "" + i.EntryInterval;
                }
            };

            textBoxComments.Text = i.Comments;
            textBoxComments.TextChanged += delegate
            {
                CurrentInsecticide.Comments = textBoxComments.Text;
            };

            comboBoxRestricted.SelectedIndex = (i.Restricted) ? 0 : 1;
            Console.WriteLine("Items in combo box: " + comboBoxRestricted.Items.Count);
            comboBoxRestricted.SelectionChanged += delegate
            {
                CurrentInsecticide.Restricted = (comboBoxRestricted.SelectedIndex == 0);
                Console.WriteLine("Restricted = " + CurrentInsecticide.Restricted);
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
