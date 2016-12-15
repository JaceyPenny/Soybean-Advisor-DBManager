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
    /// Interaction logic for DiseaseDatabaseControl.xaml
    /// </summary>
    public partial class DiseaseDatabaseControl : UserControl
    {
        ObservableCollection<Disease> Diseases = new ObservableCollection<Disease>();
        ObservableCollection<Control> Controls = new ObservableCollection<Control>();
        ObservableCollection<Control> LoadedControls = new ObservableCollection<Control>();

        public DiseaseDatabaseControl()
        {
            InitializeComponent();
            FindDiseases();
        }

        private async void FindDiseases()
        {
            labelDiseasesLoading.Visibility = Visibility.Visible;

            labelControls.Content = "Controls";
            labelDiseases.Content = "Diseases";

            var diseaseQuery = from disease in new ParseQuery<Disease>()
                               orderby disease.Name
                               select disease;
            diseaseQuery = diseaseQuery.Limit(1000);
            Diseases = new ObservableCollection<Disease>(await diseaseQuery.FindAsync());

            var controlQuery = from control in new ParseQuery<Control>()
                               orderby control.ControlId
                               select control;
            controlQuery = controlQuery.Limit(1000);
            Controls = new ObservableCollection<Control>(await controlQuery.FindAsync());

            listDiseases.ItemsSource = Diseases;
            listControls.ItemsSource = LoadedControls;

            listDiseases.SelectionChanged += listDiseases_SelectionChanged;

            listDiseases.MouseDoubleClick += listDiseases_MouseDoubleClick;

            listControls.MouseDoubleClick += listControls_MouseDoubleClick;

            labelDiseasesLoading.Visibility = Visibility.Hidden;
            labelDiseases.Content = "Diseases (" + Diseases.Count + ")";
        }

        void listControls_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listControls.SelectedIndex == -1) return;
            Control c = LoadedControls.ElementAt(listControls.SelectedIndex);
            ControlObjectEditor window = new ControlObjectEditor(c);
            window.Closed += async delegate
            {
                await c.SaveAsync();
            };
            window.ShowDialog();
        }

        void listDiseases_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listDiseases.SelectedIndex == -1) return;
            FindControls(Diseases.ElementAt(listDiseases.SelectedIndex));
        }

        void listDiseases_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Disease d = Diseases.ElementAt(listDiseases.SelectedIndex);
            DiseaseObjectEditor window = new DiseaseObjectEditor(d);
            window.Closed += async delegate
            {
                await d.SaveAsync();
            };
            window.ShowDialog();
        }

        private void FindControls(Disease d)
        {
            lblNoControls.Visibility = Visibility.Hidden;
            LoadedControls.Clear();
            List<int> controlIds = d.ControlIds.ToList();

            foreach (Control c in Controls)
            {
                if (controlIds.Contains(c.ControlId))
                {
                    LoadedControls.Add(c);
                }
            }

            labelControls.Content = d.Name + ": Controls (" + LoadedControls.Count + ")";

            if (LoadedControls.Count == 0)
            {
                lblNoControls.Visibility = Visibility.Visible;
            }
        }

        private void buttonControlDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listControls.SelectedIndex == -1) return;

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this control?", "Are you sure?", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                int index = listControls.SelectedIndex;
                Control c = LoadedControls.ElementAt(index);
                Disease d = Diseases.ElementAt(listDiseases.SelectedIndex);
                Controls.Remove(c);
                LoadedControls.Remove(c);

                List<int> clist = d.ControlIds.ToList();
                clist.RemoveAt(index);
                d.ControlIds = clist;

                d.SaveAsync();
                c.DeleteAsync();

                FindControls(Diseases.ElementAt(listDiseases.SelectedIndex));

                index--;
                if (index < 0)
                    index = 0;
                if (LoadedControls.Count > 0)
                    listControls.SelectedIndex = index;

                MessageBox.Show("Control deleted.");
            }
        }

        private void buttonControlAdd_Click(object sender, RoutedEventArgs e)
        {
            Control c = new Control
            {
                ControlId = GetFirstAvailableID()
            };
            Controls.Add(c);
            LoadedControls.Add(c);
            lblNoControls.Visibility = Visibility.Hidden;
            ControlObjectEditor editor = new ControlObjectEditor(c);
            editor.Closed += async delegate
            {
                await c.SaveAsync();

                if (Diseases.ElementAt(listDiseases.SelectedIndex).ControlIds.Count == 0)
                {
                    List<int> ids = new List<int>();
                    ids.Add(c.ControlId);
                    Diseases.ElementAt(listDiseases.SelectedIndex).ControlIds = ids;
                }
                else
                {
                    Diseases.ElementAt(listDiseases.SelectedIndex).ControlIds.Add(c.ControlId);
                }
                FindControls(Diseases.ElementAt(listDiseases.SelectedIndex));
                await (Diseases.ElementAt(listDiseases.SelectedIndex)).SaveAsync();

                listControls.SelectedIndex = LoadedControls.Count - 1;
            };
            editor.ShowDialog();
        }

        private int GetFirstAvailableID()
        {
            int i = 1;
            bool found = true;
            while (found)
            {
                found = false;
                foreach (Control c in Controls)
                {
                    if (i == c.ControlId)
                    {
                        found = true;
                        i++;
                        break;
                    }
                }
            }
            return i;
        }

        private async void buttonDiseaseDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listDiseases.SelectedIndex == -1) return;
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this disease and its controls and pictures from the database?", "Are you sure?", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                int index = listDiseases.SelectedIndex;
                Disease d = Diseases.ElementAt(index);

                foreach (Control c in LoadedControls)
                {
                    await c.DeleteAsync();
                }

                var pictureQuery = from pic in new ParseQuery<Picture>()
                                   where pic.ItemId == d.PictureId
                                   select pic;
                List<Picture> results = (await pictureQuery.FindAsync()).ToList();

                foreach (Picture p in results)
                {
                    await p.DeleteAsync();
                }

                Diseases.Remove(d);
                await d.DeleteAsync();

                index--;
                if (index < 0) index = 0;
                if (Diseases.Count > 0)
                    listDiseases.SelectedIndex = index;
                labelDiseases.Content = "Diseases (" + Diseases.Count + ")";
            }
        }

        private void buttonDiseaseAdd_Click(object sender, RoutedEventArgs e)
        {
            Disease d = new Disease()
            {
                DiseaseId = GetFirstAvailableDiseaseID(),
                PictureId = -1
            };
            Diseases.Add(d);

            Console.WriteLine("Disease ID: " + d.DiseaseId);

            DiseaseObjectEditor editor = new DiseaseObjectEditor(d);
            editor.Closed += delegate
            {
                d.SaveAsync();
            };
            editor.ShowDialog();

        }

        private int GetFirstAvailableDiseaseID()
        {
            int i = 1;
            bool found = true;
            while (found)
            {
                found = false;
                foreach (Disease d in Diseases)
                {
                    if (i == d.DiseaseId)
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
            listDiseases.ItemsSource = null;
            listControls.ItemsSource = null;

            listDiseases.MouseDoubleClick -= listDiseases_MouseDoubleClick;
            listDiseases.SelectionChanged -= listDiseases_SelectionChanged;

            listControls.MouseDoubleClick -= listControls_MouseDoubleClick;

            FindDiseases();
        }

        private void btnDiseaseEdit_Click(object sender, RoutedEventArgs e)
        {
            if (listDiseases.SelectedIndex != -1)
            {
                int index = listDiseases.SelectedIndex;
                DiseaseObjectEditor window = new DiseaseObjectEditor(Diseases.ElementAt(index));
                window.Closed += delegate
                {
                    Diseases.ElementAt(index).SaveAsync();
                };
                window.ShowDialog();
            }
        }
    }
}
