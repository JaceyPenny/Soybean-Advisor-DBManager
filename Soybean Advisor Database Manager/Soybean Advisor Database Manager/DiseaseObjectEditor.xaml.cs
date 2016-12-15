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
using Parse;

namespace Soybean_Advisor_Database_Manager
{
    /// <summary>
    /// Interaction logic for DiseaseObjectEditor.xaml
    /// </summary>
    public partial class DiseaseObjectEditor : Window
    {
        Disease d;
        List<Picture> Pictures = new List<Picture>();

        public DiseaseObjectEditor()
        {
            InitializeComponent();
            this.Closed += delegate
            {
                d.SaveAsync();
            };
        }

        public DiseaseObjectEditor(Disease d)
        {
            InitializeComponent();
            this.d = d;

            txtName.Text = d.Name;
            txtCategory.Text = d.Category;
            txtSymptoms.Text = d.Symptoms;

            txtName.TextChanged += delegate
            {
                d.Name = txtName.Text;
            };
            txtCategory.TextChanged += delegate {
                d.Category = txtCategory.Text;
            };
            txtSymptoms.TextChanged += delegate
            {
                d.Symptoms = txtSymptoms.Text;
            };

            listManagement.ItemsSource = d.Management;
            listManagement.MouseDoubleClick += listManagement_MouseDoubleClick;

            GetPictures();
        }

        private async void GetPictures()
        {
            var pictureQuery = from pic in new ParseQuery<Picture>()
                               orderby pic.ItemId
                               select pic;
            pictureQuery = pictureQuery.Limit(1000);
            Pictures = (await pictureQuery.FindAsync()).ToList();
        }

        void listManagement_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listManagement.SelectedIndex == -1) return;
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this Management Item?", "Confirm Delete", MessageBoxButton.OKCancel);

            if (result.Equals(MessageBoxResult.OK))
            {
                List<string> m = d.Management.ToList();
                m.RemoveAt(listManagement.SelectedIndex);
                d.Management = m;
                d.SaveAsync();
            }

            listManagement.ItemsSource = null;
            listManagement.ItemsSource = d.Management;
        }

        private string MakeControlIdsText(IList<int> list)
        {
            string output = "";
            for (int i = 0; i < list.Count - 1; i++)
            {
                output += list.ElementAt(i) + ", ";
            }

            if (list.Count > 0)
                output += list.Last();
            return output;
        }

        private List<int> GetControlIdsList(string p)
        {
            List<int> output = new List<int>();

            string[] split = p.Replace(" ", "").Split(',');
            foreach (string s in split)
            {
                try
                {
                    output.Add(Int32.Parse(s));
                }
                catch (Exception e)
                {

                }
            }

            return output;
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter) && !txtSymptoms.IsFocused)
            {
                await d.SaveAsync();
                this.Close();
            }
        }

        private void ManagementButtonClicked(object sender, RoutedEventArgs e)
        {
            Button codeButton = (Button)sender;
            int code = Int32.Parse((string)codeButton.Tag);

            if (code != 0)
            {
                List<string> m = d.Management.ToList();
                m.Add(GetStringFromCode(code));
                d.Management = m;
            }
            else
            {
                string input = GetStringFromInputBox();

                if (input != null)
                {
                    List<string> m = d.Management.ToList();
                    m.Add(input);
                    d.Management = m;
                }
            }

            d.SaveAsync();
            listManagement.ItemsSource = null;
            listManagement.ItemsSource = d.Management;

            Console.WriteLine("Management length: " + d.Management.Count);
        }

        private string GetStringFromInputBox()
        {
            InputDialog dlg = new InputDialog("Management", "Enter custom Management Item");
            dlg.Topmost = true;
            dlg.ShowDialog();
            if (dlg.DialogResult == true)
            {
                return dlg.DialogText;
            }
            else
            {
                return null;
            }
        }

        private string GetStringFromCode(int code)
        {
            switch (code)
            {
                case 1:
                    return "Use resistant varieties.";
                case 2:
                    return "Plant the least susceptible varieties.";
                case 3:
                    return "Plant high-quality, disease-free seed.";
                case 4:
                    return "Use appropriate seed treatment fungicides";
                case 5:
                    return "Delay planting to escape early vegetative infection during rainy periods.";
                case 8:
                    return "Use tillage practices that lead to rapid decomposition of crop residue.";
                case 9:
                    return "Harvest soybeans promptly at maturity.";
                case 10:
                    return "Apply a foliar fungicide when weather conditions favoring disease development are forecast-between early pod development and initial seed formation, when yield potential is high and disease is present.";
                case 13:
                    return "Avoid continuous soybean production";
                case 14:
                    return "Avoid narrow row widths and high plant populations.";
            }

            return null;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (d.PictureId == -1)
            {
                d.PictureId = GetFirstPictureId();
            }
            PictureManager window = new PictureManager(d);
            window.Closing += async delegate
            {
                var pictureQuery = from picture in new ParseQuery<Picture>()
                                   where picture.ItemId == d.PictureId
                                   select picture;
                var result = await pictureQuery.FindAsync();
                if (result.Count() == 0)
                {
                    d.PictureId = -1;
                }

            };
            window.ShowDialog();
        }

        private int GetFirstPictureId()
        {
            int i = 1;
            bool found = true;
            while (found)
            {
                found = false;
                foreach (Picture p in Pictures)
                {
                    if (p.ItemId == i)
                    {
                        found = true;
                        i++;
                        break;
                    }
                }
            }
            return i;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (listManagement.SelectedIndex != -1)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this Management Item?", "Confirm Delete", MessageBoxButton.OKCancel);

                if (result.Equals(MessageBoxResult.OK))
                {
                    List<string> m = d.Management.ToList();
                    m.RemoveAt(listManagement.SelectedIndex);
                    d.Management = m;
                }
                d.SaveAsync();
                listManagement.ItemsSource = d.Management;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
