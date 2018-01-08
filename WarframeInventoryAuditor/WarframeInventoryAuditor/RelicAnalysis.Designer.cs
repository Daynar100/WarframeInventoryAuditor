namespace WarframeInventoryAuditor
{
    partial class RelicAnalysis
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlRelics = new System.Windows.Forms.Panel();
            this.cmbSort = new System.Windows.Forms.ComboBox();
            this.cmbPlatSource = new System.Windows.Forms.ComboBox();
            this.cbVaulted = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(409, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status";
            // 
            // pnlRelics
            // 
            this.pnlRelics.Location = new System.Drawing.Point(11, 31);
            this.pnlRelics.Name = "pnlRelics";
            this.pnlRelics.Size = new System.Drawing.Size(1079, 360);
            this.pnlRelics.TabIndex = 1;
            // 
            // cmbSort
            // 
            this.cmbSort.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbSort.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSort.FormattingEnabled = true;
            this.cmbSort.Items.AddRange(new object[] {
            "RNG (Intact)",
            "RNG (Exceptional)",
            "RNG (Flawless)",
            "RNG (Radiant)",
            "Average Common",
            "Average Uncommon",
            "Rare",
            "Natural"});
            this.cmbSort.Location = new System.Drawing.Point(11, 5);
            this.cmbSort.Name = "cmbSort";
            this.cmbSort.Size = new System.Drawing.Size(121, 21);
            this.cmbSort.TabIndex = 2;
            this.cmbSort.SelectedIndexChanged += new System.EventHandler(this.cmbSort_SelectedIndexChanged);
            // 
            // cmbPlatSource
            // 
            this.cmbPlatSource.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbPlatSource.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbPlatSource.FormattingEnabled = true;
            this.cmbPlatSource.Items.AddRange(new object[] {
            "Average Mean",
            "Min",
            "Max"});
            this.cmbPlatSource.Location = new System.Drawing.Point(138, 4);
            this.cmbPlatSource.Name = "cmbPlatSource";
            this.cmbPlatSource.Size = new System.Drawing.Size(121, 21);
            this.cmbPlatSource.TabIndex = 3;
            this.cmbPlatSource.SelectedIndexChanged += new System.EventHandler(this.cmbPlatSource_SelectedIndexChanged);
            // 
            // cbVaulted
            // 
            this.cbVaulted.AutoSize = true;
            this.cbVaulted.Location = new System.Drawing.Point(266, 8);
            this.cbVaulted.Name = "cbVaulted";
            this.cbVaulted.Size = new System.Drawing.Size(125, 17);
            this.cbVaulted.TabIndex = 4;
            this.cbVaulted.Text = "include vaulted relics";
            this.cbVaulted.UseVisualStyleBackColor = true;
            this.cbVaulted.Visible = false;
            // 
            // RelicAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1102, 398);
            this.Controls.Add(this.cbVaulted);
            this.Controls.Add(this.cmbPlatSource);
            this.Controls.Add(this.cmbSort);
            this.Controls.Add(this.pnlRelics);
            this.Controls.Add(this.lblStatus);
            this.Name = "RelicAnalysis";
            this.Text = "RelicAnalysis";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel pnlRelics;
        private System.Windows.Forms.ComboBox cmbSort;
        private System.Windows.Forms.ComboBox cmbPlatSource;
        private System.Windows.Forms.CheckBox cbVaulted;
    }
}