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
        Form1 main_form;
        public RelicOpening(Form1 f)
        {
            main_form = f;
            InitializeComponent();
            List<String> relic_names = new List<String>();
            foreach(Form1.Relic r in main_form.relics)
            {
                relic_names.Add(r.name);
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

        private async void UpdateRelicPanel(GroupBox gb, int cmbIndex)
        {
            Form1.Relic r = main_form.relics[cmbIndex];
            foreach (Control p in gb.Controls)
            {
                foreach(Control c in p.Controls)
                {
                    String name = r.stuff[int.Parse(p.Tag.ToString())];
                    switch(c.Tag)
                    {
                        case "name":
                            c.Text = name;
                            break;
                        case "min":
                            c.Text = "Min: " + (await main_form.GetItemPrice(name, "min_price")).ToString("N2");
                            break;
                        case "max":
                            c.Text = "Max: " + (await main_form.GetItemPrice(name, "max_price")).ToString("N2");
                            break;
                        case "avg":
                            c.Text = (await main_form.GetItemPrice(name)).ToString("N2");
                            break;
                        case "pb":
                            UpdateThumbnail((PictureBox)c, name);
                            break;
                    }
                }
            }
        }

        private async void UpdateThumbnail(PictureBox pb, String name)
        {
            pb.Image = null;
            if (name == "Forma Blueprint")
            {
                pb.Image = Image.FromFile("Forma2.png");
            }
            else
            {
                pb.Image = await main_form.GetItemThumbnail(name);
            }
        }

        private void cmbRelics_SelectedIndexChanged(Object sender, EventArgs e)
        {
            UpdateRelicPanel((GroupBox)((ComboBox)sender).Parent,((ComboBox)sender).SelectedIndex);
        }
    }
}
