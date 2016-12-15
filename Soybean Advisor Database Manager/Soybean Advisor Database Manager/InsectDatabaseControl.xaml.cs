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
    /// Interaction logic for InsectDatabaseControl.xaml
    /// </summary>
    public partial class InsectDatabaseControl : UserControl
    {
        bool loading = false;

        ObservableCollection<Insect> Insects = new ObservableCollection<Insect>();
        ObservableCollection<Insecticide> Insecticides = new ObservableCollection<Insecticide>();
        ObservableCollection<Insecticide> LoadedInsecticides = new ObservableCollection<Insecticide>();

        public InsectDatabaseControl()
        {
            InitializeComponent();
            FindInsects();
        }

        public async void FindInsects()
        {
            loading = true;
            labelInsectsLoading.Visibility = Visibility.Visible;
            labelInsecticides.Content = "Insecticides";
            labelInsects.Content = "Insects";
            var insectQuery = from insect in new ParseQuery<Insect>()
                              orderby insect.Name
                              select insect;
            insectQuery = insectQuery.Limit(1000);
            Insects = new ObservableCollection<Insect>(await insectQuery.FindAsync());

            var insecticideQuery = from insecticide in new ParseQuery<Insecticide>()
                                   orderby insecticide.InsecticideId
                                   select insecticide;
            insecticideQuery = insecticideQuery.Limit(1000);
            Insecticides = new ObservableCollection<Insecticide>(await insecticideQuery.FindAsync());

            labelInsectsLoading.Visibility = Visibility.Hidden;

            listInsects.SelectionChanged += listInsects_SelectionChanged;
            labelInsects.Content = "Insects (" + Insects.Count + ")";

            listInsects.ItemsSource = Insects;
            listInsecticides.ItemsSource = LoadedInsecticides;

            listInsects.MouseDoubleClick += listInsects_MouseDoubleClick;

            listInsecticides.MouseDoubleClick += listInsecticides_MouseDoubleClick;
            loading = false;
        }

        void listInsecticides_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            InsecticideObjectEditor window = new InsecticideObjectEditor(LoadedInsecticides.ElementAt(listInsecticides.SelectedIndex));
            window.Closing += async delegate
            {
                await LoadedInsecticides.ElementAt(listInsecticides.SelectedIndex).SaveAsync();
            };
            window.ShowDialog();
        }

        void listInsects_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            InsectObjectEditor window = new InsectObjectEditor(Insects.ElementAt(listInsects.SelectedIndex));
            window.Closing += async delegate
            {
                await Insects.ElementAt(listInsects.SelectedIndex).SaveAsync();
            };
            window.ShowDialog();
        }

        void listInsects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listInsects.SelectedIndex == -1)
                return;
            LoadInsecticides(Insects.ElementAt(listInsects.SelectedIndex));
        }

        void LoadInsecticides(Insect insect)
        {
            LoadedInsecticides.Clear();

            List<int> insecticideIds = insect.InsecticideIds.ToList();

            foreach (Insecticide i in Insecticides)
            {
                if (insecticideIds.Contains(i.InsecticideId))
                {
                    LoadedInsecticides.Add(i);
                }
            }

            if (LoadedInsecticides.Count == 0)
                labelNoInsecticides.Visibility = Visibility.Visible;
            else
                labelNoInsecticides.Visibility = Visibility.Hidden;

            labelInsecticides.Content = insect.Name + ": Insecticides (" + LoadedInsecticides.Count + ")";
        }

        private bool Contains(IList<int> ints, int val)
        {
            foreach (int i in ints)
            {
                if (i == val) return true;
            }
            return false;
        }

        private async void buttonInsectDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listInsects.SelectedIndex == -1)
                return;

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this insect and its insecticides and pictures?", "Are you sure?", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                String outputText = "Insect deleted.";

                outputText = "Insect deleted along with " + LoadedInsecticides.Count + " insecticides.";
                foreach (Insecticide i in LoadedInsecticides)
                {
                    await i.DeleteAsync();
                    Insecticides.Remove(i);
                }

                Insect insect = Insects.ElementAt(listInsects.SelectedIndex);

                var pictureQuery = from pic in new ParseQuery<Picture>()
                                   where pic.ItemId == insect.PictureId
                                   select pic;
                List<Picture> results = (await pictureQuery.FindAsync()).ToList();
                foreach (Picture p in results)
                {
                    await p.DeleteAsync();
                }

                int newIndex = listInsects.SelectedIndex - 1;
                if (newIndex < 0) newIndex = 0;
                listInsects.SelectedIndex = newIndex;
                Insects.Remove(insect);
                await insect.DeleteAsync();
                labelInsects.Content = "Insects (" + Insects.Count + ")";
                MessageBox.Show(outputText);
            }
        }

        private async void buttonInsecticideDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listInsecticides.SelectedIndex == -1)
                return;

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this Insecticide?", "Are you sure?", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                Insect insect = Insects.ElementAt(listInsects.SelectedIndex);
                insect.InsecticideIds.RemoveAt(listInsecticides.SelectedIndex);
                await insect.SaveAsync();

                Insecticide i = LoadedInsecticides.ElementAt(listInsecticides.SelectedIndex);
                Insecticides.Remove(i);
                LoadedInsecticides.Remove(i);
                labelInsecticides.Content = insect.Name + ": Insecticides (" + LoadedInsecticides.Count + ")";
                if (LoadedInsecticides.Count == 0)
                {
                    labelNoInsecticides.Visibility = Visibility.Visible;
                }
                await i.DeleteAsync();
                MessageBox.Show("Insecticide deleted.");
            }
        }

        private void buttonInsectEdit_Click(object sender, RoutedEventArgs e)
        {
            if (listInsects.SelectedIndex != -1)
            {
                InsectObjectEditor window = new InsectObjectEditor(Insects.ElementAt(listInsects.SelectedIndex));
                window.Closing += async delegate
                {
                    await Insects.ElementAt(listInsects.SelectedIndex).SaveAsync();
                };
                window.ShowDialog();
            }
        }

        private void buttonInsecticideEdit_Click(object sender, RoutedEventArgs e)
        {
            if (listInsecticides.SelectedIndex != -1)
            {
                InsecticideObjectEditor window = new InsecticideObjectEditor(LoadedInsecticides.ElementAt(listInsecticides.SelectedIndex));
                window.Closing += async delegate
                {
                    await LoadedInsecticides.ElementAt(listInsecticides.SelectedIndex).SaveAsync();
                };
                window.ShowDialog();
            }
        }

        private async void buttonInsectAdd_Click(object sender, RoutedEventArgs e)
        {
            Insect i = new Insect()
            {
                InsectId = GetFirstInsectId(),
                PictureId = -1
            };
            await i.SaveAsync();
            Insects.Add(i);
            listInsects.SelectedIndex = Insects.Count - 1;
            InsectObjectEditor window = new InsectObjectEditor(i);
            window.Closing += async delegate
            {
                await i.SaveAsync();
                
                labelInsects.Content = "Insects (" + Insects.Count + ")";
            };
            window.ShowDialog();
        }

        private int GetFirstInsectId()
        {
            int i = 1;
            bool found = true;
            while (found)
            {
                found = false;
                foreach (Insect c in Insects)
                {
                    if (i == c.InsectId)
                    {
                        found = true;
                        i++;
                        break;
                    }
                }
            }
            return i;
        }

        private async void buttonInsecticideAdd_Click(object sender, RoutedEventArgs e)
        {
            Insecticide i = new Insecticide()
            {
                InsecticideId = GetFirstInsecticideId()
            };
            await i.SaveAsync();
            Insecticides.Add(i);
            LoadedInsecticides.Add(i);
            labelNoInsecticides.Visibility = Visibility.Hidden;
            InsecticideObjectEditor window = new InsecticideObjectEditor(i);
            window.Closing += async delegate
            {
                await i.SaveAsync();
                labelNoInsecticides.Visibility = Visibility.Hidden;

                if (Insects.ElementAt(listInsects.SelectedIndex).InsecticideIds.Count == 0)
                {
                    List<int> ids = new List<int>();
                    ids.Add(i.InsecticideId);
                    Insects.ElementAt(listInsects.SelectedIndex).InsecticideIds = ids;
                }
                else
                {
                    Insects.ElementAt(listInsects.SelectedIndex).InsecticideIds.Add(i.InsecticideId);
                }
                labelInsecticides.Content = Insects.ElementAt(listInsects.SelectedIndex).Name + ": Insecticides (" + LoadedInsecticides.Count + ")";
                await Insects.ElementAt(listInsects.SelectedIndex).SaveAsync();

                listInsecticides.SelectedIndex = LoadedInsecticides.Count - 1;
            };
            window.ShowDialog();
        }

        private int GetFirstInsecticideId()
        {
            int i = 1;
            bool found = true;
            while (found)
            {
                found = false;
                foreach (Insecticide c in Insecticides)
                {
                    if (i == c.InsecticideId)
                    {
                        found = true;
                        i++;
                        break;
                    }
                }
            }
            return i;
        }

        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (loading) return;
            listInsects.ItemsSource = null;
            listInsecticides.ItemsSource = null;

            listInsects.SelectionChanged -= listInsects_SelectionChanged;
            listInsects.MouseDoubleClick -= listInsects_MouseDoubleClick;

            listInsecticides.MouseDoubleClick -= listInsecticides_MouseDoubleClick;

            FindInsects();
        }
    }
}
