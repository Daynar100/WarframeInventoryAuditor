using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarframeInventoryAuditor
{
    public partial class RelicOpening : Form
    {
        DataHandler dh;
        private List<Tuple<Task<float>, String,String>> ongoing_tasks = new List<Tuple<Task<float>, String, String>>();
        public RelicOpening(DataHandler d)
        {
            dh = d;
            InitializeComponent();
            List<String> relic_names = new List<String>();
            foreach(Relic r in dh.relics)
            {
                relic_names.Add(r.GetName());
            }
            cmbRelics.DataSource = relic_names;
            comboBox1.DataSource = new List<String>(relic_names);
            comboBox2.DataSource = new List<String>(relic_names);
            comboBox3.DataSource = new List<String>(relic_names);
            UpdateRelicPanel(groupBox1, 0);
            UpdateRelicPanel(groupBox2, 0);
            UpdateRelicPanel(groupBox3, 0);
            UpdateRelicPanel(groupBox4, 0);
        }

        private void UpdateRelicPanel(GroupBox gb, int cmbIndex)
        {
            Relic r = dh.relics[cmbIndex];
            foreach (Control p in gb.Controls)
            {
                Label min = null, max = null, avg = null;
                String name = "";
                foreach (Control c in p.Controls)
                {
                    name = r.GetItemName(int.Parse(p.Tag.ToString()));
                    switch(c.Tag)
                    {
                        case "name":
                            c.Text = name;
                            break;
                        case "min":
                            min = (Label)c;
                            break;
                        case "max":
                            max = (Label)c;
                            break;
                        case "avg":
                            avg = (Label)c;
                            break;
                        case "pb":
                            UpdateThumbnail((PictureBox)c, name);
                            break;
                    }
                    
                }
                UpdateValue(name, min, max, avg);
            }
        }

        private async void UpdateThumbnail(PictureBox pb, String name)
        {
            pb.Image = null;
            pb.AccessibleName = name;
            if (name == "Forma Blueprint")
            {
                pb.Image = Image.FromFile("Forma2.png");
            }
            else
            {
                Bitmap b = await dh.GetItemThumbnail(name);
                if(pb.AccessibleName == name)
                    pb.Image = b;
            }
        }

        private async void UpdateValue(String name, Label min, Label max, Label avg)
        {
            if (min == null || max == null || avg == null)
            {
                return;
            }
            min.Text = "Min: ";
            max.Text = "Max: ";
            avg.Text = "Average";
            min.AccessibleName = name;
            Task<float> t;
            if (ongoing_tasks.Exists(x => (x.Item2 == name && x.Item3 == "avg_price")))
                t = ongoing_tasks.Find(x => (x.Item2 == name && x.Item3 == "avg_price")).Item1;
            else
            {
                t = dh.GetItemPrice(name, "avg_price");
                ongoing_tasks.Add(new Tuple<Task<float>, String, String>(t, name, "avg_price"));
            }
            String s = (await t).ToString("N2");
            if (min.AccessibleName == name)
            {
                avg.Text = s;
                min.Text = "Min: " + (await dh.GetItemPrice(name, "min_price")).ToString("N2");
                max.Text = "Max: " + (await dh.GetItemPrice(name, "max_price")).ToString("N2");
            }
        }

        private void cmbRelics_SelectedIndexChanged(Object sender, EventArgs e)
        {
            ongoing_tasks.Clear();
            UpdateRelicPanel((GroupBox)((ComboBox)sender).Parent,((ComboBox)sender).SelectedIndex);
        }
    }
}
