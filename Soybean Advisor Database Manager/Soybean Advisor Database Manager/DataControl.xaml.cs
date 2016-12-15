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
using System.Text.RegularExpressions;

namespace Soybean_Advisor_Database_Manager
{
    /// <summary>
    /// Interaction logic for DataControl.xaml
    /// </summary>
    public partial class DataControl : UserControl
    {
        Data CurrentData;

        TextBox[] boxes;

        public DataControl()
        {
            InitializeComponent();
            FindData();
        }

        public async void FindData()
        {
            CurrentData = await new ParseQuery<Data>().FirstOrDefaultAsync();

            boxes = new TextBox[] {
                txtr0c0, txtr1c0, txtr2c0, txtr3c0, txtr0c1, txtr1c1, txtr2c1, txtr3c1, txtr0c2, txtr1c2, txtr2c2,
                txtr3c2, txtr0c3, txtr1c3, txtr2c3, txtr3c3, txtr0c4, txtr1c4, txtr2c4, txtr3c4,
                // 0 - 19
                ptxtr0c0, ptxtr0c1, ptxtr1c0, ptxtr1c1, ptxtr2c0, ptxtr2c1, ptxtr3c0, ptxtr3c1, ptxtr4c0, ptxtr4c1,
                // 20 - 29
                ktxtr0c0, ktxtr0c1, ktxtr1c0, ktxtr1c1, ktxtr2c0, ktxtr2c1, ktxtr3c0, ktxtr3c1, ktxtr4c0, ktxtr4c1,
                // 30 - 39
                itxtr0c0, itxtr0c1, itxtr1c0, itxtr1c1, itxtr2c0, itxtr2c1, itxtr3c0, itxtr3c1, itxtr4c0, itxtr4c1,
                itxtr5c0, itxtr5c1
                // 40
            };

            // Set the limerate textboxes
            txtr0c0.Text = "" + CurrentData.LimeRate.ElementAt(0).ElementAt(0);
            txtr1c0.Text = "" + CurrentData.LimeRate.ElementAt(1).ElementAt(0);
            txtr2c0.Text = "" + CurrentData.LimeRate.ElementAt(2).ElementAt(0);
            txtr3c0.Text = "" + CurrentData.LimeRate.ElementAt(3).ElementAt(0);

            txtr0c1.Text = "" + CurrentData.LimeRate.ElementAt(0).ElementAt(1);
            txtr1c1.Text = "" + CurrentData.LimeRate.ElementAt(1).ElementAt(1);
            txtr2c1.Text = "" + CurrentData.LimeRate.ElementAt(2).ElementAt(1);
            txtr3c1.Text = "" + CurrentData.LimeRate.ElementAt(3).ElementAt(1);

            txtr0c2.Text = "" + CurrentData.LimeRate.ElementAt(0).ElementAt(2);
            txtr1c2.Text = "" + CurrentData.LimeRate.ElementAt(1).ElementAt(2);
            txtr2c2.Text = "" + CurrentData.LimeRate.ElementAt(2).ElementAt(2);
            txtr3c2.Text = "" + CurrentData.LimeRate.ElementAt(3).ElementAt(2);

            txtr0c3.Text = "" + CurrentData.LimeRate.ElementAt(0).ElementAt(3);
            txtr1c3.Text = "" + CurrentData.LimeRate.ElementAt(1).ElementAt(3);
            txtr2c3.Text = "" + CurrentData.LimeRate.ElementAt(2).ElementAt(3);
            txtr3c3.Text = "" + CurrentData.LimeRate.ElementAt(3).ElementAt(3);

            // Set the phosphorus textboxes
            ptxtr0c0.Text = "" + CurrentData.Phosphorus.ElementAt(0).ElementAt(0);
            ptxtr1c0.Text = "" + CurrentData.Phosphorus.ElementAt(0).ElementAt(1);
            ptxtr2c0.Text = "" + CurrentData.Phosphorus.ElementAt(0).ElementAt(2);
            ptxtr3c0.Text = "" + CurrentData.Phosphorus.ElementAt(0).ElementAt(3);
            ptxtr4c0.Text = "" + CurrentData.Phosphorus.ElementAt(0).ElementAt(4);

            ptxtr0c1.Text = "" + CurrentData.Phosphorus.ElementAt(1).ElementAt(0);
            ptxtr1c1.Text = "" + CurrentData.Phosphorus.ElementAt(1).ElementAt(1);
            ptxtr2c1.Text = "" + CurrentData.Phosphorus.ElementAt(1).ElementAt(2);
            ptxtr3c1.Text = "" + CurrentData.Phosphorus.ElementAt(1).ElementAt(3);
            ptxtr4c1.Text = "" + CurrentData.Phosphorus.ElementAt(1).ElementAt(4);

            // Set the potassium textboxes
            ktxtr0c0.Text = "" + CurrentData.Potassium.ElementAt(0).ElementAt(0);
            ktxtr1c0.Text = "" + CurrentData.Potassium.ElementAt(0).ElementAt(1);
            ktxtr2c0.Text = "" + CurrentData.Potassium.ElementAt(0).ElementAt(2);
            ktxtr3c0.Text = "" + CurrentData.Potassium.ElementAt(0).ElementAt(3);
            ktxtr4c0.Text = "" + CurrentData.Potassium.ElementAt(0).ElementAt(4);

            ktxtr0c1.Text = "" + CurrentData.Potassium.ElementAt(1).ElementAt(0);
            ktxtr1c1.Text = "" + CurrentData.Potassium.ElementAt(1).ElementAt(1);
            ktxtr2c1.Text = "" + CurrentData.Potassium.ElementAt(1).ElementAt(2);
            ktxtr3c1.Text = "" + CurrentData.Potassium.ElementAt(1).ElementAt(3);
            ktxtr4c1.Text = "" + CurrentData.Potassium.ElementAt(1).ElementAt(4);

            // Set the insect textboxes
            itxtr0c0.Text = "" + CurrentData.InsectThresholds.ElementAt(0).ElementAt(0);
            itxtr0c1.Text = "" + CurrentData.InsectThresholds.ElementAt(0).ElementAt(1);

            itxtr1c0.Text = "" + CurrentData.InsectThresholds.ElementAt(1).ElementAt(0);
            itxtr1c1.Text = "" + CurrentData.InsectThresholds.ElementAt(1).ElementAt(1);

            itxtr2c0.Text = "" + CurrentData.InsectThresholds.ElementAt(2).ElementAt(0);
            itxtr2c1.Text = "" + CurrentData.InsectThresholds.ElementAt(2).ElementAt(1);

            itxtr3c0.Text = "" + CurrentData.InsectThresholds.ElementAt(3).ElementAt(0);
            itxtr3c1.Text = "" + CurrentData.InsectThresholds.ElementAt(3).ElementAt(1);

            itxtr4c0.Text = "" + CurrentData.InsectThresholds.ElementAt(4).ElementAt(0);
            itxtr4c1.Text = "" + CurrentData.InsectThresholds.ElementAt(4).ElementAt(1);

            itxtr5c0.Text = "" + CurrentData.InsectThresholds.ElementAt(5).ElementAt(0);
            itxtr5c1.Text = "" + CurrentData.InsectThresholds.ElementAt(5).ElementAt(1);

            foreach (TextBox t in boxes)
                if (t.Text.Equals("-1"))
                    t.Text = "";
        }

        private async void btnSaveData_Click(object sender, RoutedEventArgs e)
        {
            for (int j = 0; j < boxes.Length; j++)
            {
                TextBox t = boxes[j];
                if (t.Text.Equals(""))
                {
                    t.Text = "-1";
                }

                if (t.Text.Contains(","))
                {
                    t.Text = t.Text.Replace(",", "");
                }

                try
                {
                    int.Parse(t.Text);
                }
                catch (Exception ex)
                {
                    String output = "Insect Thresholds";

                    if (j < 40)
                        output = "Potassium Application";
                    if (j < 30)
                        output = "Phosphorus Application";
                    if (j < 20)
                        output = "Lime Application";

                    MessageBox.Show("Invalid value " + t.Text + " in " + output, "Invalid value");
                    return;
                }
            }

            // Save lime data
            List<int> l1 = new List<int>();
            l1.Add(int.Parse(txtr0c0.Text));
            l1.Add(int.Parse(txtr0c1.Text));
            l1.Add(int.Parse(txtr0c2.Text));
            l1.Add(int.Parse(txtr0c3.Text));

            List<int> l2 = new List<int>();
            l2.Add(int.Parse(txtr1c0.Text));
            l2.Add(int.Parse(txtr1c1.Text));
            l2.Add(int.Parse(txtr1c2.Text));
            l2.Add(int.Parse(txtr1c3.Text));

            List<int> l3 = new List<int>();
            l3.Add(int.Parse(txtr2c0.Text));
            l3.Add(int.Parse(txtr2c1.Text));
            l3.Add(int.Parse(txtr2c2.Text));
            l3.Add(int.Parse(txtr2c3.Text));

            List<int> l4 = new List<int>();
            l4.Add(int.Parse(txtr3c0.Text));
            l4.Add(int.Parse(txtr3c1.Text));
            l4.Add(int.Parse(txtr3c2.Text));
            l4.Add(int.Parse(txtr3c3.Text));

            IList<IList<int>> limelist = new List<IList<int>>();
            limelist.Add(l1);
            limelist.Add(l2);
            limelist.Add(l3);
            limelist.Add(l4);

            CurrentData.LimeRate = limelist;

            // Save Phosphorus Data
            List<int> pl1 = new List<int>();
            pl1.Add(int.Parse(ptxtr0c0.Text));
            pl1.Add(int.Parse(ptxtr1c0.Text));
            pl1.Add(int.Parse(ptxtr2c0.Text));
            pl1.Add(int.Parse(ptxtr3c0.Text));
            pl1.Add(int.Parse(ptxtr4c0.Text));

            List<int> pl2 = new List<int>();
            pl2.Add(int.Parse(ptxtr0c1.Text));
            pl2.Add(int.Parse(ptxtr1c1.Text));
            pl2.Add(int.Parse(ptxtr2c1.Text));
            pl2.Add(int.Parse(ptxtr3c1.Text));
            pl2.Add(int.Parse(ptxtr4c1.Text));

            IList<IList<int>> plist = new List<IList<int>>();
            plist.Add(pl1);
            plist.Add(pl2);

            CurrentData.Phosphorus = plist;

            // Save Potassium Data
            List<int> kl1 = new List<int>();
            kl1.Add(int.Parse(ktxtr0c0.Text));
            kl1.Add(int.Parse(ktxtr1c0.Text));
            kl1.Add(int.Parse(ktxtr2c0.Text));
            kl1.Add(int.Parse(ktxtr3c0.Text));
            kl1.Add(int.Parse(ktxtr4c0.Text));

            List<int> kl2 = new List<int>();
            kl2.Add(int.Parse(ktxtr0c1.Text));
            kl2.Add(int.Parse(ktxtr1c1.Text));
            kl2.Add(int.Parse(ktxtr2c1.Text));
            kl2.Add(int.Parse(ktxtr3c1.Text));
            kl2.Add(int.Parse(ktxtr4c1.Text));

            IList<IList<int>> klist = new List<IList<int>>();
            klist.Add(kl1);
            klist.Add(kl2);

            CurrentData.Potassium = klist;

            // Save Insect Data
            List<int> il1 = new List<int>();
            il1.Add(int.Parse(itxtr0c0.Text));
            il1.Add(int.Parse(itxtr0c1.Text));

            List<int> il2 = new List<int>();
            il2.Add(int.Parse(itxtr1c0.Text));
            il2.Add(int.Parse(itxtr1c1.Text));

            List<int> il3 = new List<int>();
            il3.Add(int.Parse(itxtr2c0.Text));
            il3.Add(int.Parse(itxtr2c1.Text));

            List<int> il4 = new List<int>();
            il4.Add(int.Parse(itxtr3c0.Text));
            il4.Add(int.Parse(itxtr3c1.Text));

            List<int> il5 = new List<int>();
            il5.Add(int.Parse(itxtr4c0.Text));
            il5.Add(int.Parse(itxtr4c1.Text));

            List<int> il6 = new List<int>();
            il6.Add(int.Parse(itxtr5c0.Text));
            il6.Add(int.Parse(itxtr5c1.Text));

            IList<IList<int>> ilist = new List<IList<int>>();
            ilist.Add(il1);
            ilist.Add(il2);
            ilist.Add(il3);
            ilist.Add(il4);
            ilist.Add(il5);
            ilist.Add(il6);

            CurrentData.InsectThresholds = ilist;

            foreach (TextBox t in boxes)
                if (t.Text.Equals("-1"))
                    t.Text = "";

            // Save Data Object
            await CurrentData.SaveAsync();
            MessageBox.Show("Data saved successfully.");
        }

        private void textPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }
    }
}
