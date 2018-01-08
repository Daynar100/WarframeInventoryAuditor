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
    public partial class ModAnalysis : Form
    {
        //name,rarity,value
        List<Tuple<String, String, float, float, float>> mods = new List<Tuple<String, String, float, float, float>>();
        private Panel panel1;
        private ComboBox cmbSort;
        private ComboBox comboBox1;
        private CheckBox chbCommons;
        private CheckBox chbUncommons;
        private CheckBox chbRares;
        private CheckBox chbPrimed;
        private CheckBox chbNoData;
        Form1 main_form;
        public ModAnalysis(Form1 f)
        {
            main_form = f;
            InitializeComponent();
            InitMods();
            cmbSort.SelectedIndex = 0;
        }

        private async void InitMods()
        {
            if (System.IO.File.Exists("mod_list.txt"))
            {
                String[] names = System.IO.File.ReadAllLines("mod_list.txt");
                for (int i = 0; i < names.GetLength(0); ++i)
                {
                    String name = names[i];
                    float price = await main_form.GetItemPrice(name, "avg_price");
                    float min = await main_form.GetItemPrice(name, "min_price");
                    float max = await main_form.GetItemPrice(name, "max_price");
                    String rarity = await main_form.GetItemProperty(name, "rarity");
                    mods.Add(new Tuple<String, String, float, float, float>(name, rarity, price, min, max));
                }
            }
            UpdatePanel();
        }

        private void UpdatePanel()
        {
            panel1.AutoScroll = false;
            panel1.VerticalScroll.Value = 0;
            panel1.Controls.Clear();

            switch (cmbSort.SelectedIndex)
            {
                case 0:
                    mods.Sort((x, y) => y.Item3.CompareTo(x.Item3));
                    break;
                case 1:
                    mods.Sort((x, y) => y.Item4.CompareTo(x.Item4));
                    break;
                case 2:
                    mods.Sort((x, y) => y.Item5.CompareTo(x.Item5));
                    break;
            }

            int count = 0;
            foreach (Tuple<String, String, float, float, float> t in mods)
            {
                //exclude based on checkboxes
                if (!chbNoData.Checked && t.Item3 == 0)
                    continue;
                else if (t.Item2 == "common")
                {
                    if (!chbCommons.Checked)
                        continue;
                }
                else if (t.Item2 == "uncommon")
                {
                    if (!chbUncommons.Checked)
                        continue;
                }
                else if (t.Item2 == "rare")
                {
                    if (!chbRares.Checked)
                        continue;
                }
                else if (t.Item2 == "legendary")
                {
                    if (!chbPrimed.Checked)
                        continue;
                }

                Label n = new Label();
                n.AutoSize = true;
                n.Location = new Point(300, count * 25);
                n.Text = t.Item1;
                panel1.Controls.Add(n);

                Label p = new Label();
                p.AutoSize = true;
                p.Location = new Point(10, count * 25);
                if (t.Item3 != 0)
                {
                    p.Text = "Average: " + t.Item3.ToString("N2").PadRight(10) + " Min: " + t.Item4.ToString("N2").PadRight(10) + " Max : " + t.Item5.ToString("N2");
                }
                else
                    p.Text = "No Data";
                panel1.Controls.Add(p);
                ++count;
            }

            panel1.AutoScroll = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePanel();
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbSort = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.chbCommons = new System.Windows.Forms.CheckBox();
            this.chbUncommons = new System.Windows.Forms.CheckBox();
            this.chbRares = new System.Windows.Forms.CheckBox();
            this.chbPrimed = new System.Windows.Forms.CheckBox();
            this.chbNoData = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(705, 480);
            this.panel1.TabIndex = 0;
            // 
            // cmbSort
            // 
            this.cmbSort.FormattingEnabled = true;
            this.cmbSort.Items.AddRange(new object[] {
            "Average",
            "Min",
            "Max"});
            this.cmbSort.Location = new System.Drawing.Point(12, 12);
            this.cmbSort.Name = "cmbSort";
            this.cmbSort.Size = new System.Drawing.Size(103, 21);
            this.cmbSort.TabIndex = 1;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(121, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 2;
            // 
            // chbCommons
            // 
            this.chbCommons.AutoSize = true;
            this.chbCommons.Checked = true;
            this.chbCommons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbCommons.Location = new System.Drawing.Point(249, 15);
            this.chbCommons.Name = "chbCommons";
            this.chbCommons.Size = new System.Drawing.Size(72, 17);
            this.chbCommons.TabIndex = 3;
            this.chbCommons.Text = "Commons";
            this.chbCommons.UseVisualStyleBackColor = true;
            // 
            // chbUncommons
            // 
            this.chbUncommons.AutoSize = true;
            this.chbUncommons.Checked = true;
            this.chbUncommons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbUncommons.Location = new System.Drawing.Point(327, 16);
            this.chbUncommons.Name = "chbUncommons";
            this.chbUncommons.Size = new System.Drawing.Size(85, 17);
            this.chbUncommons.TabIndex = 4;
            this.chbUncommons.Text = "Uncommons";
            this.chbUncommons.UseVisualStyleBackColor = true;
            // 
            // chbRares
            // 
            this.chbRares.AutoSize = true;
            this.chbRares.Checked = true;
            this.chbRares.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbRares.Location = new System.Drawing.Point(418, 15);
            this.chbRares.Name = "chbRares";
            this.chbRares.Size = new System.Drawing.Size(54, 17);
            this.chbRares.TabIndex = 5;
            this.chbRares.Text = "Rares";
            this.chbRares.UseVisualStyleBackColor = true;
            // 
            // chbPrimed
            // 
            this.chbPrimed.AutoSize = true;
            this.chbPrimed.Checked = true;
            this.chbPrimed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbPrimed.Location = new System.Drawing.Point(478, 16);
            this.chbPrimed.Name = "chbPrimed";
            this.chbPrimed.Size = new System.Drawing.Size(76, 17);
            this.chbPrimed.TabIndex = 6;
            this.chbPrimed.Text = "Legendary";
            this.chbPrimed.UseVisualStyleBackColor = true;
            // 
            // chbNoData
            // 
            this.chbNoData.AutoSize = true;
            this.chbNoData.Location = new System.Drawing.Point(560, 16);
            this.chbNoData.Name = "chbNoData";
            this.chbNoData.Size = new System.Drawing.Size(64, 17);
            this.chbNoData.TabIndex = 7;
            this.chbNoData.Text = "No data";
            this.chbNoData.UseVisualStyleBackColor = true;
            // 
            // ModAnalysis
            // 
            this.ClientSize = new System.Drawing.Size(729, 535);
            this.Controls.Add(this.chbNoData);
            this.Controls.Add(this.chbPrimed);
            this.Controls.Add(this.chbRares);
            this.Controls.Add(this.chbUncommons);
            this.Controls.Add(this.chbCommons);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.cmbSort);
            this.Controls.Add(this.panel1);
            this.Name = "ModAnalysis";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
