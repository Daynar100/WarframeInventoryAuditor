using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using WindowsInput;
using System.Net.Http;


namespace WarframeInventoryAuditor
{
    
    public partial class Form1 : Form
    {
        List<Tuple<String,uint>> inventory = new List<Tuple<String, uint>>();
        private Button btnModAnalysis;
        private Button btnRelicAnalysis;
        private Label lblPTotal;
        private Panel pnlInv;
        private Button btnRelicOpening;
        private PictureBox pictureBox1;
        private RichTextBox richTextBox1;
        private Button button1;
        private Button button2;
        DataHandler dh;

        public Form1()
        {
            InitializeComponent();
            dh = new DataHandler();
        }

        private async void PopulateItemBox()
        {
            pnlInv.AutoScroll = false;
            pnlInv.VerticalScroll.Value = 0;
            float plat_total = 0;
            pnlInv.Controls.Clear();
            for (int i = 0; i < Math.Ceiling((decimal)inventory.Count / (decimal)7.0); ++i)
                for (int ii = 0; ii < Math.Min(7, (inventory.Count - i * 7)); ++ii)
                {
                    String item_name = await dh.GetItemProperty(inventory[i * 7 + ii].Item1, "item_name");
                    float price = await dh.GetItemPrice(inventory[i * 7 + ii].Item1);
                    if (item_name == "" || price == 0)
                    {
                        CreateNewItemDisplay(new Point(10 + ii * 150, 10 + i * 150), inventory[i * 7 + ii].Item1, 0, 0, true);
                        continue;
                    }
                    CreateNewItemDisplay(new Point(10 + ii * 150, 10 + i * 150), item_name, (int)inventory[i * 7 + ii].Item2, price);
                    plat_total += (int)inventory[i * 7 + ii].Item2 * price;
                }
            lblPTotal.Text = "Plat Total: " + plat_total.ToString();
            pnlInv.AutoScroll = true;
        }

        private void Form1_Closed(object sender, FormClosedEventArgs e)
        {
            dh.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Audit();
            //ScanMods();
            /*ItemImage(new Point(98,327 + 207));
            inventory.Add(ParseImage());
            GetItems();
            if (File.Exists("inventory.txt"))
            {
                inventory = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Tuple<String, uint>>>(File.ReadAllText("inventory.txt"));
            }
            else
                Audit();
            float plat_total = 0;
            //
            Audit();

            foreach (Tuple<String, uint> i in inventory)
            {
                //richTextBox1.Text += i.Item1 + " : " + i.Item2 + "\n";
                //UpdateItemData(i.Item1);
                //Thread.Sleep(500);
                //UpdateItemPrice(i.Item1);
                if (items.TryGetValue(i.Item1, out Dictionary<String, String> item))
                {
                    String outstr = "";
                    if (item.TryGetValue("item_name", out String o))
                        outstr += o + " ";
                    outstr += i.Item2.ToString() + " ";
                    if (item.TryGetValue("ducats", out String o2))
                        outstr += o2 + " ";
                    if (item.TryGetValue("moving_avg", out String o3))
                    {
                        plat_total += float.Parse(o3) * (float) i.Item2;
                        outstr += o3;
                    }
                    richTextBox1.Text += outstr + '\n';
                    //Thread.Sleep(500);
                }
                else
                    richTextBox1.Text += "ERROR: no item found for " + i.Item1 + '\n';
                //
            }
            richTextBox1.Text += "Total Plat Value: " + plat_total + '\n';
            //*/

        }

        private async void CreateNewItemDisplay(Point origin, String name, int count, float price, bool unknown = false)
        {

            PictureBox Icon = new PictureBox();
            Icon.Location = origin;
            Icon.SizeMode = PictureBoxSizeMode.Zoom;
            pnlInv.Controls.Add(Icon);

            Label lname = new Label();
            lname.Location = new Point(origin.X,origin.Y + Icon.Height + 10);
            lname.TextAlign = ContentAlignment.MiddleCenter;
            lname.AutoSize = true;
            pnlInv.Controls.Add(lname);

            Label plattext = new Label();
            plattext.Location = new Point(origin.X, origin.Y + Icon.Height + 40);
            plattext.TextAlign = ContentAlignment.MiddleCenter;
            plattext.AutoSize = true;
            pnlInv.Controls.Add(plattext);

            
            lname.Text = name;
            if (unknown)
            {
                lname.AutoSize = false;
                lname.Width = 125;
                plattext.Text = "Error: Unknown Item!";
            }
            else
            {
                plattext.Text = count.ToString() + "(Total) * " + price.ToString() + "(Price) " + " = " + (count * price).ToString("N1");
                Icon.Image = await dh.GetItemThumbnail(name);
            }



        }

        private void ScanMods()
        {
            IntPtr warframe = FindWindow("WarframePublicEvolutionGfxD3D11", "WARFRAME");
            SetForegroundWindow(warframe);
            RECT rect = new RECT();
            GetWindowRect(warframe, out rect);
            DoMouseClick(rect.Right - rect.Left - 3, (rect.Bottom - rect.Top) / 2);
            Thread.Sleep(100);
            WindowsInput.InputSimulator keyboardSimulator = new InputSimulator();
            keyboardSimulator.Mouse.XButtonClick(0);
            Thread.Sleep(1000);
            GetModImage(new Point(105, (rect.Bottom - rect.Top) / 2 + 55));
            ParseMod();
        }

        private void GetModImage(Point origin)
        {
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
            Rectangle bounds = new Rectangle(origin, new Size(220, 27));
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(origin, Point.Empty, bitmap.Size);
                }
                pictureBox1.Image = bitmap;
                pictureBox1.Refresh();
                Color common = Color.FromArgb(218,158,131);
                for (int y = 0; y < bitmap.Height; y++)
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        Color c = bitmap.GetPixel(x, y);
                        if (Math.Abs(c.GetHue() - common.GetHue()) < .6)
                        {
                            if (x != 0 && y != 0)
                            {
                                //bitmap.SetPixel(x - 1, y, Color.Black);
                                bitmap.SetPixel(x, y, Color.Black);
                                //bitmap.SetPixel(x, y - 1, Color.Black);
                            }
                        }
                        else
                            bitmap.SetPixel(x, y, Color.White);
                    }
                using (Bitmap bitmap_edited = new Bitmap(bitmap))
                {
                    for (int y = 1; y < bitmap.Height -1; y++)
                        for (int x = 1; x < bitmap.Width -1; x++)
                        {
                            Color c = bitmap.GetPixel(x, y);
                            if (c.GetBrightness() > .5)
                            {
                                uint count = 0;
                                if (bitmap.GetPixel(x-1, y).GetBrightness() < .5)
                                    ++count;
                                if (bitmap.GetPixel(x+1, y).GetBrightness() < .5)
                                    ++count;
                                if (bitmap.GetPixel(x, y-1).GetBrightness() < .5)
                                    ++count;
                                if (bitmap.GetPixel(x, y+1).GetBrightness() < .5)
                                    ++count;
                                if (count >= 2)
                                    bitmap_edited.SetPixel(x, y, Color.Black);
                            }
                        }
                    pictureBox1.Image = bitmap_edited;
                    pictureBox1.Refresh();//*/
                    bitmap.Save("item.bmp");
                }
            }
            origin.X += 10;
            origin.Y -= 35;
            bounds.Width = 50;
            bounds.Height = 17;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(origin, Point.Empty, bitmap.Size);
                }
                //pictureBox1.Image = bitmap;
                //pictureBox1.Refresh();
                /*int rgb;
                Color c;
                for (int y = 0; y < bitmap.Height; y++)
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        c = bitmap.GetPixel(x, y);
                        rgb = (int)((c.R + c.G + c.B) / 3);
                        rgb = rgb < 193 ? 255 : 0;
                        bitmap.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                    }
                pictureBox1.Image = bitmap;
                pictureBox1.Refresh();*/
                bitmap.Save("itemcount.bmp");
            }
        }
        private Tuple<String, uint> ParseMod()
        {
            String text = "";
            using (var ocrEngine = new TesseractEngine(@"tessdata", @"eng", EngineMode.TesseractOnly, @"Mods.txt"))
            {
                //name
                while (true)
                {

                    try
                    {
                        using (var imageWithText = Pix.LoadFromFile(@"item.bmp"))
                        {
                            using (var page = ocrEngine.Process(imageWithText, Tesseract.PageSegMode.SingleLine))
                            {
                                text = page.GetText();
                                text = text.Replace("\n", "").ToUpper();
                                richTextBox1.Text += text + '\n';
                                break;
                            }
                        }
                    }
                    catch (IOException)
                    {
                        //wait and retry
                        Thread.Sleep(1000);
                    }
                }
                //count
                while (true)
                {
                    try
                    {
                        using (var imageWithText = Pix.LoadFromFile(@"itemcount.bmp"))
                        {
                            using (var page = ocrEngine.Process(imageWithText, Tesseract.PageSegMode.SingleWord))
                            {
                                var count_text = page.GetText();
                                //System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[^\d]");
                                /*if (count_text != "")
                                {
                                    uint.TryParse(count_text.Replace(" ", "").Split('O').First<string>(), out uint num);
                                    //richTextBox1.Text += count_text + ": " + num.ToString() + '\n';

                                    return new Tuple<String, uint>(text, num);

                                }*/
                                richTextBox1.Text += count_text + '\n';

                                return new Tuple<String, uint>(text, 1);
                            }
                        }
                    }
                    catch (IOException)
                    {
                        //wait and retry
                        Thread.Sleep(1000);
                    }
                }
                //Thread.Sleep(1000);
            }
        }


        private async void Audit()
        {
            inventory.Clear();
            IntPtr warframe = FindWindow("WarframePublicEvolutionGfxD3D11", "WARFRAME");
            SetForegroundWindow(warframe);
            DoMouseClick();
            Thread.Sleep(100);
            WindowsInput.InputSimulator keyboardSimulator = new InputSimulator();
            keyboardSimulator.Mouse.XButtonClick(0);
            await Task.Delay(150);
            //keyboardSimulator.Mouse.VerticalScroll(-1);
            string name = "";
            //parse first screen
            for (int y = 0; y < 2; ++y)
                for (int x = 0; x < 6; ++x)
                {
                    ItemImage(new Point(98 + 290 * x, 327 + 207 * y));
                    //Thread.Sleep(1000);
                    Tuple<String, uint> result = ParseImage();
                    name = result.Item1;
                    //Thread.Sleep(1000);
                    if (name == "")
                    {
                        y = 2;
                        break;
                    }
                    inventory.Add(result);
                }
            //scroll and parse
            string lastTop = name;
            while (name != "")
            {
                keyboardSimulator.Mouse.VerticalScroll(-1);
                await Task.Delay(1200);
                for (int y = 0; y < 2; ++y)
                {
                    ItemImage(new Point(98 + 290 * 5, 327 + 207 * y));
                    Tuple<String, uint> result = ParseImage();
                    name = result.Item1;
                    if (name == lastTop || name == "")
                    {
                        name = "";
                        break;
                    }
                    else if (y == 0)
                    {
                        inventory.Add(result);
                        lastTop = name;
                    }
                    else
                        inventory.Add(result);
                }
            }
            File.WriteAllText("inventory.txt", Newtonsoft.Json.JsonConvert.SerializeObject(inventory));
            PopulateItemBox();
        }

        private void ItemImage(Point origin)
        {
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
            Rectangle bounds = new Rectangle(origin, new System.Drawing.Size(270, 75));
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(origin, Point.Empty, bitmap.Size);
                }
                pictureBox1.Image = bitmap;
                pictureBox1.Refresh();
                int rgb;
                Color c;
                for (int y = 0; y < bitmap.Height; y++)
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        c = bitmap.GetPixel(x, y);
                        rgb = (int)((c.R + c.G + c.B) / 3);
                        rgb = rgb < 128 ? 0 : 255;
                        if (rgb == 0 && x < 4)
                        {
                            int starty = (int)Math.Floor((decimal)y / 23) * 23;
                            for (int yy = starty; yy < Math.Min(starty + 23,bitmap.Height); yy++)
                                for (int xx = 0; xx < bitmap.Width; xx++)
                                    bitmap.SetPixel(xx, yy, Color.White);
                            y = starty + 22;
                            break;
                        }
                        else
                            bitmap.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                    }
                Bitmap one_line_bitmap = new Bitmap(bounds.Width * 3, 25);
                using (Graphics g = Graphics.FromImage(one_line_bitmap))
                {
                    for (var i = 0; i < 3; ++i)
                    {
                        g.DrawImage(bitmap, new Rectangle(i * bounds.Width, 0, bounds.Width, 25), 0, i * 23, bounds.Width, 23, GraphicsUnit.Pixel);
                    }
                }
                pictureBox1.Image = one_line_bitmap;
                pictureBox1.Refresh();
                one_line_bitmap.Save("item.bmp");
            }
            //count
            bounds = new Rectangle(origin, new System.Drawing.Size(235, 20));
            origin.Y -= 71;
            origin.X += 37;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(origin, Point.Empty, bitmap.Size);
                }
                
                for (int y = 0; y < bitmap.Height; y++)
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        Color c = bitmap.GetPixel(x, y);
                        int rgb = (int)((c.R + c.G + c.B) / 3);
                        rgb = rgb < 128 ? 255 : 0;
                        bitmap.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                    }
                //pictureBox1.Image = bitmap;
                //pictureBox1.Refresh();
                bitmap.Save("itemcount.bmp");
            }
        }

        private Tuple<String, uint> ParseImage()
        {
            String text = "";
            richTextBox1.Text = Path.Combine(Environment.CurrentDirectory,@"tessdata");
            richTextBox1.Refresh();
            using (var ocrEngine = new TesseractEngine(Path.Combine(Environment.CurrentDirectory, @"tessdata"), @"eng", EngineMode.TesseractOnly, @"test.txt"))
            {
                //name
                while (true)
                {

                    try
                    {
                        using (var imageWithText = Pix.LoadFromFile(@"item.bmp"))
                        {
                            if (pictureBox1.Image != null)
                                pictureBox1.Image.Dispose();
                            pictureBox1.Image = Image.FromFile(@"item.bmp");
                            pictureBox1.Refresh();
                            using (var page = ocrEngine.Process(imageWithText, Tesseract.PageSegMode.SingleBlock))
                            {
                                text = page.GetText();
                                text = text.Replace("\n", "").ToUpper();
                                text = text.Replace("SVSTEMS", "SYSTEMS");
                                if(text.Contains("BLUEPRINT") && (text.Contains("SYSTEMS") || text.Contains("CHASSIS") || text.Contains("HARNESS") || text.Contains("NEUROPTICS")))
                                {
                                    text = text.Remove(text.IndexOf(" BLUEPRINT"),10);
                                }
                                //richTextBox1.Text += text + '\n';
                                break;
                            }
                        }
                    }
                    catch (IOException)
                    {
                        //wait and retry
                        Thread.Sleep(1000);
                    }
                }
                //count
                while(true)
                {
                    try
                    {
                        using (var imageWithText = Pix.LoadFromFile(@"itemcount.bmp"))
                        {

                            if (pictureBox1.Image != null)
                                pictureBox1.Image.Dispose();
                            pictureBox1.Image = Image.FromFile(@"itemcount.bmp");
                            pictureBox1.Refresh();
                            using (var page = ocrEngine.Process(imageWithText, Tesseract.PageSegMode.SingleBlock))
                            {
                                var count_text = page.GetText();
                                //System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[^\d]");
                                if (count_text.Contains("OWNED"))
                                {
                                    uint.TryParse(count_text.Replace(" ","").Split('O').First<string>(), out uint num);
                                    //richTextBox1.Text += count_text + ": " + num.ToString() + '\n';

                                    return new Tuple<String, uint>(text, num);

                                }
                                
                                return new Tuple<String, uint>(text, 1);
                            }
                        }
                    }
                    catch (IOException)
                    {
                        //wait and retry
                        Thread.Sleep(1000);
                    }
                }
                //Thread.Sleep(1000);
            }
        }

        //Stuff I copy pasted from other people/places :D

        //windows docs
        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public void DoMouseClick(int x = 100, int y = 100)
        {
            //Call the imported function with the cursor's current position
            
            var X = Cursor.Position.X;
            var Y = Cursor.Position.Y;
            Cursor.Position = new Point(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, 0);
            //Cursor.Position = new Point(X, Y);
        }


        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }


    
        private void btnRelicOpening_Click(object sender, EventArgs e)
        {
            RelicOpening form = new RelicOpening(dh);
            form.Show();
        }

        private void btnRelicAnalysis_Click(object sender, EventArgs e)
        {
            RelicAnalysis form = new RelicAnalysis(dh);
            form.Show();
        }

        private void btnModAnalysis_Click(object sender, EventArgs e)
        {
            ModAnalysis form = new ModAnalysis(dh);
            form.Show();
        }

        private void InitializeComponent()
        {
            this.btnModAnalysis = new System.Windows.Forms.Button();
            this.btnRelicAnalysis = new System.Windows.Forms.Button();
            this.lblPTotal = new System.Windows.Forms.Label();
            this.pnlInv = new System.Windows.Forms.Panel();
            this.btnRelicOpening = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnModAnalysis
            // 
            this.btnModAnalysis.Location = new System.Drawing.Point(454, 12);
            this.btnModAnalysis.Name = "btnModAnalysis";
            this.btnModAnalysis.Size = new System.Drawing.Size(93, 23);
            this.btnModAnalysis.TabIndex = 18;
            this.btnModAnalysis.Text = "Mod Analysis";
            this.btnModAnalysis.UseVisualStyleBackColor = true;
            this.btnModAnalysis.Click += new System.EventHandler(this.btnModAnalysis_Click);
            // 
            // btnRelicAnalysis
            // 
            this.btnRelicAnalysis.Location = new System.Drawing.Point(345, 12);
            this.btnRelicAnalysis.Name = "btnRelicAnalysis";
            this.btnRelicAnalysis.Size = new System.Drawing.Size(103, 23);
            this.btnRelicAnalysis.TabIndex = 17;
            this.btnRelicAnalysis.Text = "Relic Analysis";
            this.btnRelicAnalysis.UseVisualStyleBackColor = true;
            this.btnRelicAnalysis.Click += new System.EventHandler(this.btnRelicAnalysis_Click);
            // 
            // lblPTotal
            // 
            this.lblPTotal.AutoSize = true;
            this.lblPTotal.Location = new System.Drawing.Point(12, 38);
            this.lblPTotal.Name = "lblPTotal";
            this.lblPTotal.Size = new System.Drawing.Size(55, 13);
            this.lblPTotal.TabIndex = 16;
            this.lblPTotal.Text = "Plat Total:";
            // 
            // pnlInv
            // 
            this.pnlInv.Location = new System.Drawing.Point(15, 58);
            this.pnlInv.Name = "pnlInv";
            this.pnlInv.Size = new System.Drawing.Size(1070, 278);
            this.pnlInv.TabIndex = 15;
            // 
            // btnRelicOpening
            // 
            this.btnRelicOpening.Location = new System.Drawing.Point(201, 11);
            this.btnRelicOpening.Name = "btnRelicOpening";
            this.btnRelicOpening.Size = new System.Drawing.Size(138, 23);
            this.btnRelicOpening.TabIndex = 14;
            this.btnRelicOpening.Text = "Relic Opening Assistor";
            this.btnRelicOpening.UseVisualStyleBackColor = true;
            this.btnRelicOpening.Click += new System.EventHandler(this.btnRelicOpening_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(905, 342);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(180, 59);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(15, 342);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(884, 59);
            this.richTextBox1.TabIndex = 11;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Audit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(553, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "Force Update All Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1089, 406);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnModAnalysis);
            this.Controls.Add(this.btnRelicAnalysis);
            this.Controls.Add(this.lblPTotal);
            this.Controls.Add(this.pnlInv);
            this.Controls.Add(this.btnRelicOpening);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_Closed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dh.UpdateAll(richTextBox1);
        }
    }
   
}
