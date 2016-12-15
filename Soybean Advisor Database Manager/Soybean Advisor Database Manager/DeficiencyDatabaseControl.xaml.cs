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
using Parse;
using System.Collections.ObjectModel;

namespace Soybean_Advisor_Database_Manager
{
    /// <summary>
    /// Interaction logic for DeficiencyDatabaseControl.xaml
    /// </summary>
    public partial class DeficiencyDatabaseControl : UserControl
    {
        ObservableCollection<Deficiency> Deficiencies = new ObservableCollection<Deficiency>();
        List<Picture> Pictures = new List<Picture>();

        public DeficiencyDatabaseControl()
        {
            InitializeComponent();
            HideEditor();
            FindDeficiencies();
        }

        public async void FindDeficiencies()
        {
            labelLoading.Visibility = Visibility.Visible;

            var defQuery = from def in new ParseQuery<Deficiency>()
                           orderby def.Name
                           select def;
            defQuery = defQuery.Limit(1000);
            Deficiencies = new ObservableCollection<Deficiency>(await defQuery.FindAsync());

            var pictureQuery = from picture in new ParseQuery<Picture>()
                               orderby picture.ItemId
                               select picture;
            Pictures = (await pictureQuery.FindAsync()).ToList();

            listDeficiencies.ItemsSource = Deficiencies;

            listDeficiencies.SelectionChanged += listDeficiencies_SelectionChanged;

            labelLoading.Visibility = Visibility.Hidden;

            lblDeficiencies.Content = "Deficiencies (" + Deficiencies.Count + ")";

        }

        void listDeficiencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listDeficiencies.SelectedIndex == -1) return;
            LoadEditor(Deficiencies.ElementAt(listDeficiencies.SelectedIndex));
            nameTextBox.TextChanged += nameTextBox_TextChanged;
        }

        private void LoadEditor(Deficiency deficiency)
        {
            nameTextBox.TextChanged -= nameTextBox_TextChanged;
            nameLabel.Visibility = Visibility.Visible;
            nameTextBox.Visibility = Visibility.Visible;
            btnManagePictures.Visibility = Visibility.Visible;
            descriptionLabel.Visibility = Visibility.Visible;
            descriptionTextBox.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Visible;

            nameTextBox.Text = deficiency.Name;
            descriptionTextBox.Text = deficiency.Description;
        }

        private void HideEditor()
        {
            nameLabel.Visibility = Visibility.Hidden;
            nameTextBox.Visibility = Visibility.Hidden;
            btnManagePictures.Visibility = Visibility.Hidden;
            descriptionLabel.Visibility = Visibility.Hidden;
            descriptionTextBox.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Hidden;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (listDeficiencies.SelectedIndex == -1) return;

            Deficiency d = Deficiencies.ElementAt(listDeficiencies.SelectedIndex);
            d.Name = nameTextBox.Text;
            d.Description = descriptionTextBox.Text;
            await d.SaveAsync();
            MessageBox.Show("Deficiency \"" + d.Name + "\" Saved.");
        }

        private void btnManagePictures_Click(object sender, RoutedEventArgs e)
        {
            Deficiency d = Deficiencies.ElementAt(listDeficiencies.SelectedIndex);
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
                await d.SaveAsync();

            };
            window.ShowDialog();
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listDeficiencies.SelectedIndex == -1) return;

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this deficiency?", "Are you sure?", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                int index = listDeficiencies.SelectedIndex;
                Deficiency d = Deficiencies.ElementAt(index);
                index--;
                if (index < 0) index = 0;
                Deficiencies.Remove(d);

                var pictureQuery = from pic in new ParseQuery<Picture>()
                                   where pic.ItemId == d.PictureId
                                   select pic;
                List<Picture> results = (await pictureQuery.FindAsync()).ToList();
                foreach (Picture p in results)
                {
                    await p.DeleteAsync();
                }

                if (Deficiencies.Count > 0)
                {
                    listDeficiencies.SelectedIndex = index;
                }

                lblDeficiencies.Content = "Deficiencies (" + Deficiencies.Count + ")";

                await d.DeleteAsync();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Deficiency d = new Deficiency()
            {
                DeficiencyId = GetFirstDeficiencyId(),
                PictureId = GetFirstPictureId()
            };
            Deficiencies.Add(d);
            listDeficiencies.SelectedIndex = Deficiencies.Count - 1;
            LoadEditor(d);
            nameTextBox.TextChanged += nameTextBox_TextChanged;
            lblDeficiencies.Content = "Deficiencies (" + Deficiencies.Count + ")";
            d.SaveAsync();
        }

        void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Deficiencies.ElementAt(listDeficiencies.SelectedIndex).Name = nameTextBox.Text;
        }

        private int GetFirstDeficiencyId()
        {
            int i = 1;
            bool found = true;
            while (found)
            {
                found = false;
                foreach (Deficiency d in Deficiencies)
                {
                    if (i == d.DeficiencyId)
                    {
                        found = true;
                        i++;
                        break;
                    }
                }
            }
            return i;
        }

        private int GetFirstPictureId() {
            int i = 1;
            bool found = true;
            while (found)
            {
                found = false;
                foreach (Picture p in Pictures)
                {
                    if (i == p.ItemId)
                    {
                        found = true;
                        i++;
                        break;
                    }
                }
            }
            return i;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            HideEditor();
            listDeficiencies.SelectionChanged -= listDeficiencies_SelectionChanged;
            listDeficiencies.ItemsSource = null;
            FindDeficiencies();
        }

    }
}
