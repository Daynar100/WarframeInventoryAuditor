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
    public partial class RelicAnalysis : Form
    {
        DataHandler dh;
        //Tuple contails panel and relic index in the relic list
        List<Tuple<Panel,List<float>>> relics = new List<Tuple<Panel, List<float>>>();
        public RelicAnalysis(DataHandler d)
        {
            dh = d;
            InitializeComponent();
            cmbPlatSource.SelectedIndex = 0;
            cmbSort.SelectedIndex = 0;
            //init panels
            Task t = InitPanels();
        }
        private void Status(String s)
        {
            lblStatus.Text = "Status: " + s;
            lblStatus.Refresh();
        }
        private async Task InitPanels()
        {
            relics.Clear();
            pnlRelics.Controls.Clear();
            pnlRelics.AutoScroll = false;
            pnlRelics.VerticalScroll.Value = 0;
            Status("Initializing Relics");
            String value_type = "avg_price";
            switch (cmbPlatSource.SelectedIndex)
            {
                case 1:
                    value_type = "min_price";
                    break;
                case 2:
                    value_type = "max_price";
                    break;
            }
            
            for (int i = 0; i < dh.relics.Count; ++i)
            {
                Panel panel = new Panel();
                panel.Size = new Size(1050, 50);
                panel.Location = new Point(0,50 * i);
                Relic r = dh.relics[i];

                Label name = new Label();
                name.Text = r.GetName();
                name.Location = new Point(10, 25);
                panel.Controls.Add(name);

                List<float> values = new List<float>();
                for (int j = 0; j < 6; ++j)
                {
                    String iname = r.GetItemName(j);
                    Label liname = new Label();
                    liname.Text = iname;
                    liname.Location = new Point(10 + 150 * (j + 1), 10);
                    liname.AutoSize = true;
                    panel.Controls.Add(liname);

                    Label plat = new Label();
                    float value = await dh.GetItemPrice(iname, value_type);
                    values.Add(value);
                    plat.Text = value.ToString("N2") + " Plat";
                    plat.Location = new Point(10 + 150 * (j + 1), 35);
                    plat.AutoSize = true;
                    panel.Controls.Add(plat);
                }
                relics.Add(new Tuple<Panel, List<float>>(panel, values));
                pnlRelics.Controls.Add(panel);
            }
            SortPanels();
        }

        private void SortPanels()
        {
            Status("Sorting Relics");
            pnlRelics.VerticalScroll.Value = 0;
            pnlRelics.AutoScroll = false;
            List<Tuple<Panel, float>> value_list = new List<Tuple<Panel, float>>();
            int relic_num = 0;
            foreach(Tuple<Panel, List<float>> t in relics)
            {
                List<float> v = t.Item2;
                float value = 0;
                switch (cmbSort.SelectedIndex)
                {
                    case 0: // RNG Intact
                        value = (float)(v[0] * .2533 + v[1] * .2533 + v[2] * .2533 + v[3] * .11 + v[4] * .11 + v[5] * .2);
                        break;
                    case 1: // RNG Exceptional
                        value = (float)(v[0] * .2333 + v[1] * .2333 + v[2] * .2333 + v[3] * .13 + v[4] * .13 + v[5] * .4);
                        break;
                    case 2: // RNG Flawless
                        value = (float)(v[0] * .2 + v[1] * .2 + v[2] * .2 + v[3] * .17 + v[4] * .17 + v[5] * .6);
                        break;
                    case 3: // RNG Radient
                        value = (float)(v[0] * .1667 + v[1] * .1667 + v[2] * .1667 + v[3] * .20 + v[4] * .20 + v[5] * .10);
                        break;
                    case 4: // avg common
                        value = (v[0]+ v[1] + v[2]) / 3;
                        break;
                    case 5: // avg uncommon
                        value = (v[3] + v[4]) / 2;
                        break;
                    case 6: // rare
                        value = v[5];
                        break;
                    case 7: // natural
                        value = relics.Count - relic_num++;
                        break;
                }
                value_list.Add(new Tuple<Panel, float>(t.Item1, value));
            }
            value_list.Sort((x, y) => y.Item2.CompareTo(x.Item2));
            int count = 0;
            foreach(Tuple<Panel, float> t in value_list)
            {
                t.Item1.Location = new Point(0, count++ * 50);            
            }
            Status("Finished Sorting Relics");
            pnlRelics.AutoScroll = true;

        }

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortPanels();
        }

        private void cmbPlatSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitPanels();
        }
    }
}
