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
using System.Net;
using System.Net.Http;
using System.IO;
using Microsoft.Win32;

namespace Soybean_Advisor_Database_Manager
{
    /// <summary>
    /// Interaction logic for PictureManager.xaml
    /// </summary>
    public partial class PictureManager : Window
    {
        Pictureable Item;
        List<Picture> Pictures = new List<Picture>();

        ParseFile Upload = null;
        BitmapImage UploadImage = null;

        public PictureManager()
        {
            InitializeComponent();
        }

        public PictureManager(Pictureable p)
        {
            InitializeComponent();
            Item = p;
            GetPictures();
        }

        public async void GetPictures()
        {
            Pictures.Clear();
            listPictures.Items.Clear();
            txtSource.Text = "";
            Upload = null;

            int itemId = Item.GetPictureID();

            var pictureQuery = from picture in new ParseQuery<Picture>()
                               where picture.ItemId == itemId
                               orderby picture.PictureId
                               select picture;
            Pictures = (await pictureQuery.FindAsync()).ToList();

            int i = 0;

            lblPicturesText.Content = "Pictures (" + Pictures.Count + ")";
            List<ListBoxItem> items = new List<ListBoxItem>();

            foreach (Picture p in Pictures)
            {
                i++;
                ListBoxItem item = new ListBoxItem();
                Image imageView = new Image();
                byte[] data = await new HttpClient().GetByteArrayAsync(p.File.Url);
                if (data != null && data.Length != 0)
                {
                    var image = new BitmapImage();
                    using (var mem = new MemoryStream(data))
                    {
                        mem.Position = 0;
                        image.BeginInit();
                        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = null;
                        image.StreamSource = mem;
                        image.EndInit();
                    }
                    image.Freeze();
                    imageView.Source = image;
                    imageView.MaxWidth = 160;
                    imageView.MaxHeight = 160;
                    imageView.Stretch = Stretch.Uniform;
                }
                item.Content = imageView;
                item.Selected += item_Selected;
                items.Add(item);

                ComboBoxItem comboItem = new ComboBoxItem();
                comboItem.Content = "" + i;
                comboNumber.Items.Add(comboItem);

                comboNumber.SelectedValuePath = "Content";
            }

            foreach (ListBoxItem l in items)
            {
                listPictures.Items.Add(l);
            }

            txtLoadingPictures.Visibility = Visibility.Hidden;
            lblPicturesText.Content = "Pictures (" + Pictures.Count + ")";
        }

        void item_Selected(object sender, RoutedEventArgs e)
        {
            comboNumber.IsEnabled = true;
            btnDeletePicture.IsEnabled = true;
            btnUpdateSelected.IsEnabled = true;

            int index = listPictures.Items.IndexOf(sender as ListBoxItem);
            txtSource.Text = Pictures.ElementAt(index).Source;
            comboNumber.SelectionChanged -= comboNumber_SelectionChanged;
            comboNumber.SelectedIndex = index;
            comboNumber.SelectionChanged += comboNumber_SelectionChanged;
            uploadImage.Source = ((Image)((ListBoxItem)listPictures.Items.GetItemAt(index)).Content).Source;
        }

        private async void btnUpdateSelected_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            Picture p = Pictures.ElementAt(listPictures.SelectedIndex);
            p.Source = txtSource.Text;
            bool withImage = Upload != null;
            if (Upload != null)
            {
                p.FileName = Upload.Name;
                await Upload.SaveAsync();
                p.File = Upload;
            }

            await p.SaveAsync();
            Upload = null;

            ListBoxItem item = listPictures.SelectedItem as ListBoxItem;
            (item.Content as Image).Source = UploadImage;
            UploadImage = null;
            this.IsEnabled = true;
            MessageBox.Show("Picture updated successfully" + (withImage ? " with new image" : ""));
        }

        private void btnDeletePicture_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this picture?", "Are you sure?", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                Picture p = Pictures.ElementAt(listPictures.SelectedIndex);
                p.DeleteAsync();
                Pictures.Remove(p);
                comboNumber.SelectionChanged -= comboNumber_SelectionChanged;
                comboNumber.Items.RemoveAt(comboNumber.Items.Count - 1);
                comboNumber.SelectionChanged += comboNumber_SelectionChanged;
                listPictures.Items.RemoveAt(listPictures.SelectedIndex);

                int i = 0;
                foreach (Picture pic in Pictures)
                {
                    i++;
                    pic.PictureId = i;
                    pic.SaveAsync();
                }

                txtSource.Text = "";
                Upload = null;
                listPictures.SelectedIndex = listPictures.Items.Count - 1;
                lblPicturesText.Content = "Pictures (" + Pictures.Count + ")";
                MessageBox.Show("Successfully deleted image.");
            }

        }

        private async void btnAddNewPicture_Click(object sender, RoutedEventArgs e)
        {
            if (Upload != null)
            {
                this.IsEnabled = false;
                Picture p = new Picture()
                {
                    FileName = Upload.Name,
                    ItemId = Item.GetPictureID(),
                    PictureId = Pictures.Count + 1,
                    Source = txtSource.Text
                };
                await Upload.SaveAsync();
                p.File = Upload;
                await p.SaveAsync();
                Console.WriteLine("Adding Picture, Count = " + Pictures.Count);
                Pictures.Add(p);
                Console.WriteLine("Added Picture, Count = " + Pictures.Count);
                ListBoxItem listitem = new ListBoxItem();
                Image imageView = new Image();
                imageView.Source = UploadImage;
                imageView.MaxWidth = 160;
                imageView.MaxHeight = 160;
                imageView.Stretch = Stretch.Uniform;
                listitem.Content = imageView;
                listitem.Selected += item_Selected;
                listPictures.Items.Add(listitem);

                ComboBoxItem comboitem = new ComboBoxItem();
                comboitem.Content = "" + Pictures.Count;
                comboNumber.Items.Add(comboitem);

                listPictures.SelectedIndex = listPictures.Items.Count - 1;

                lblPicturesText.Content = "Pictures (" + Pictures.Count + ")";

                this.IsEnabled = true;

                Upload = null;
                UploadImage = null;

                MessageBox.Show("Upload successful.");
            }
        }

        private void comboNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.Write("Selector changed");

            int index = (sender as ComboBox).SelectedIndex;

            if (index != listPictures.SelectedIndex)
            {

                // insert picture
                Picture p = Pictures.ElementAt(listPictures.SelectedIndex);
                Pictures.Remove(p);
                Pictures.Insert(index, p);

                for (int i = 0; i < Pictures.Count; i++)
                {
                    Pictures.ElementAt(i).PictureId = i + 1;
                    Pictures.ElementAt(i).SaveAsync();
                }

                ListBoxItem l = listPictures.Items.GetItemAt(listPictures.SelectedIndex) as ListBoxItem;
                listPictures.Items.RemoveAt(listPictures.SelectedIndex);
                listPictures.Items.Insert(index, l);


                listPictures.SelectedIndex = index;
            }

            // insert listboxitem


        }

        private void btnChooseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            fileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";

            if (fileDialog.ShowDialog() == true)
            {
                try
                {
                    UploadImage = new BitmapImage(new Uri(fileDialog.FileName));
                    uploadImage.Source = UploadImage;

                    byte[] data;
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(UploadImage));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        data = ms.ToArray();
                    }

                    Upload = new ParseFile(fileDialog.SafeFileName, data);

                    txtSource.Text = "";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Invalid file selection.");
                }
                
            }
                
        }
    }
}
