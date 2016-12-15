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
    /// Interaction logic for InsectObjectEditor.xaml
    /// </summary>
    public partial class InsectObjectEditor : Window
    {
        Insect CurrentInsect;
        List<Picture> Pictures = new List<Picture>();

        public InsectObjectEditor()
        {
            InitializeComponent();
        }

        public InsectObjectEditor(Insect i)
        {
            InitializeComponent();
            CurrentInsect = i;

            textBoxName.Text = i.Name;
            textBoxName.TextChanged += delegate
            {
                CurrentInsect.Name = textBoxName.Text;
            };
            textBoxCategory.Text = i.Category;
            textBoxCategory.TextChanged += delegate
            {
                CurrentInsect.Category = textBoxCategory.Text;
            };
            textBoxDescription.Text = i.Description;
            textBoxDescription.TextChanged += delegate
            {
                CurrentInsect.Description = textBoxDescription.Text;
            };

            this.Closing += async delegate
            {
                await CurrentInsect.SaveAsync();
            };

            GetPictures();
        }

        public async void GetPictures()
        {
            var pictureQuery = from pic in new ParseQuery<Picture>()
                               orderby pic.ItemId
                               select pic;
            pictureQuery = pictureQuery.Limit(1000);
            Pictures = (await pictureQuery.FindAsync()).ToList();
        }

        private void btnManagePictures_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentInsect.PictureId == -1)
            {
                CurrentInsect.PictureId = GetFirstPictureId();
            }
            PictureManager window = new PictureManager(CurrentInsect);
            window.Closing += async delegate
            {
                var pictureQuery = from picture in new ParseQuery<Picture>()
                                   where picture.ItemId == CurrentInsect.PictureId
                                   select picture;
                var result = await pictureQuery.FindAsync();
                if (result.Count() == 0)
                {
                    CurrentInsect.PictureId = -1;
                }

            };
            window.ShowDialog();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

    }
}
