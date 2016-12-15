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

namespace Soybean_Advisor_Database_Manager
{
    /// <summary>
    /// Interaction logic for DatabaseVersionControl.xaml
    /// </summary>
    public partial class DatabaseVersionControl : UserControl
    {
        Data CurrentData;

        public DatabaseVersionControl()
        {
            InitializeComponent();
            GetData();
        }

        public async void GetData()
        {
            var dataQuery = new ParseQuery<Data>();
            try
            {
                CurrentData = await dataQuery.FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                return;
            }
            

            UpdateVersionLabels();

            btnDiseaseIncrease.Click += delegate
            {
                CurrentData.DiseaseVersion += 1;
                UpdateVersionLabels();
                CurrentData.SaveAsync();
            };

            btnInsectIncrease.Click += delegate 
            {
                CurrentData.InsectVersion += 1;
                UpdateVersionLabels();
                CurrentData.SaveAsync();
            };

            btnDeficiencyIncrease.Click += delegate
            {
                CurrentData.DeficiencyVersion += 1;
                UpdateVersionLabels();
                CurrentData.SaveAsync();
            };
        }

        public void UpdateVersionLabels()
        {
            labelDiseaseVersion.Content = "Disease Version: " + CurrentData.DiseaseVersion;
            labelInsectVersion.Content = "Insect Version: " + CurrentData.InsectVersion;
            labelDeficiencyVersion.Content = "Deficiency Version: " + CurrentData.DeficiencyVersion;
        }
    }
}
