using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Parse;

namespace Soybean_Advisor_Database_Manager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            ParseObject.RegisterSubclass<Disease>();
            ParseObject.RegisterSubclass<Control>();
            ParseObject.RegisterSubclass<Insect>();
            ParseObject.RegisterSubclass<Insecticide>();
            ParseObject.RegisterSubclass<Deficiency>();
            ParseObject.RegisterSubclass<Picture>();
            ParseObject.RegisterSubclass<Data>();
            ParseClient.Initialize("REMOVED_KEY", "REMOVED_KEY");
        }
    }
}
