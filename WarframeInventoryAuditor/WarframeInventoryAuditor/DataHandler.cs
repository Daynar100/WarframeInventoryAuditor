using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Drawing;

namespace WarframeInventoryAuditor
{
    public class DataHandler
    {
        public DataHandler()
        {
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            client = new HttpClient();
            client.BaseAddress = new Uri("https://api.warframe.market/v1/");
            client.DefaultRequestHeaders.Add("Platform", "pc");
            client.DefaultRequestHeaders.Add("Language", "en");

            image_client = new HttpClient();
            image_client.BaseAddress = new Uri("https://warframe.market/static/assets/");

            if (File.Exists("items.txt"))
            {
                items = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<String, Dictionary<String, String>>>(File.ReadAllText("items.txt"));
            }
            else
            {
                items = new Dictionary<String, Dictionary<String, String>>();
                FetchItems();
            }

            relics.Add(new Relic("Lith A2", "Forma Blueprint", "Lex Prime Barrel", "Valkyr Prime Blueprint", "Akbronco Prime Link", "Cernos Prime Blueprint", "Akstiletto Prime Blueprint"));
            relics.Add(new Relic("Lith B2", "Forma Blueprint", "Paris Prime Lower Limb", "Tigris Prime Stock", "Braton Prime Receiver", "Orthos Prime Blade", "Ballistica Prime Blueprint"));
            relics.Add(new Relic("Lith C2", "Bronco Prime Receiver", "Paris Prime Upper Limb", "Venka Prime Blades", "Akbolto Prime Barrel", "Nami Skyla Prime Handle", "Cernos Prime Lower Limb"));
            relics.Add(new Relic("Lith N3", "Burston Prime Receiver", "Sybaris Prime Stock", "Valkyr Prime Blueprint", "Braton Prime Blueprint", "Forma Blueprint", "Nekros Prime Blueprint"));
            relics.Add(new Relic("Lith S7", "Galatine Prime Blade", "Lex Prime Blueprint", "Mirage Prime Chassis", "Akbolto Prime Link", "Paris Prime Grip", "Sybaris Prime Barrel"));
            relics.Add(new Relic("Lith T1", "Akbronco Prime Blueprint", "Burston Prime Blueprint", "Forma Blueprint", "Ballistica Prime Upper Limb", "Valkyr Prime Neuroptics", "Tigris Prime Blueprint"));
            relics.Add(new Relic("Lith V2", "Fang Prime Blueprint", "Lex Prime Barrel", "Paris Prime Lower Limb", "Forma Blueprint", "Paris Prime Upper Limb", "Vauban Prime Neuroptics"));
            relics.Add(new Relic("Lith V3", "Braton Prime Blueprint", "Cernos Prime Upper Limb", "Paris Prime Lower Limb", "Helios Prime Systems", "Tigris Prime Barrel", "Valkyr Prime Neuroptics"));
            relics.Add(new Relic("Meso G1", "Braton Prime Barrel", "Forma Blueprint", "Venka Prime Blades", "Ballistica Prime String", "Silva & Aegis Prime Blade", "Galatine Prime Blueprint"));
            relics.Add(new Relic("Meso H1", "Hydroid Prime Chassis", "Mirage Prime Neuroptics", "Paris Prime Blueprint", "Fragor Prime Blueprint", "Oberon Prime Blueprint", "Helios Prime Cerebrum"));
            relics.Add(new Relic("Meso K1", "Cernos Prime Upper Limb", "Nekros Prime Chassis", "Orthos Prime Handle", "Akstiletto Prime Barrel", "Tigris Prime Receiver", "Kogake Prime Gauntlet"));
            relics.Add(new Relic("Meso N5", "Braton Prime Stock", "Forma Blueprint", "Lex Prime Receiver", "Hydroid Prime Blueprint", "Tigris Prime Receiver", "Nekros Prime Neuroptics"));
            relics.Add(new Relic("Meso O1", "Akbronco Prime Blueprint", "Forma Blueprint", "Paris Prime Lower Limb", "Akstiletto Prime Link", "Cernos Prime String", "Oberon Prime Neuroptics"));
            relics.Add(new Relic("Neo B2", "Braton Prime Stock", "Fang Prime Blueprint", "Tigris Prime Stock", "Forma Blueprint", "Venka Prime Blueprint", "Banshee Prime Chassis"));
            relics.Add(new Relic("Neo H1", "Burston Prime Receiver", "Fragor Prime Head", "Orthos Prime Handle", "Banshee Prime Blueprint", "Tigris Prime Barrel", "Hydroid Prime Neuroptics"));
            relics.Add(new Relic("Neo M1", "Akbolto Prime Blueprint", "Bronco Prime Blueprint", "Paris Prime String", "Euphona Prime Barrel", "Helios Prime Blueprint", "Mirage Prime Blueprint"));
            relics.Add(new Relic("Neo S7", "Banshee Prime Neuroptics", "Fang Prime Blade", "Helios Prime Carapace", "Ballistica Prime Receiver", "Sybaris Prime Receiver", "Silva & Aegis Prime Guard"));
            relics.Add(new Relic("Neo V2", "Braton Prime Stock", "Fang Prime Handle", "Galatine Prime Blade", "Forma Blueprint", "Galatine Prime Handle", "Vauban Prime Blueprint"));
            relics.Add(new Relic("Neo V5", "Helios Prime Carapace", "Paris Prime String", "Silva & Aegis Prime Hilt", "Burston Prime Barrel", "Forma Blueprint", "Vauban Prime Neuroptics"));
            relics.Add(new Relic("Neo V6", "Burston Prime Stock", "Cernos Prime Grip", "Forma Blueprint", "Fragor Prime Handle", "Mirage Prime Neuroptics", "Vauban Prime Chassis"));
            relics.Add(new Relic("Axi A3", "Braton Prime Barrel", "Helios Prime Carapace", "Kogake Prime Boot", "Cernos Prime String", "Hydroid Prime Neuroptics", "Akbolto Prime Receiver"));
            relics.Add(new Relic("Axi B2", "Fang Prime Blade", "Fragor Prime Head", "Sybaris Prime Blueprint", "Forma Blueprint", "Orthos Prime Blueprint", "Banshee Prime Neuroptics"));
            relics.Add(new Relic("Axi E2", "Braton Prime Stock", "Lex Prime Receiver", "Paris Prime Blueprint", "Bronco Prime Barrel", "Forma Blueprint", "Euphona Prime Receiver"));
            relics.Add(new Relic("Axi N5", "Euphona Prime Blueprint", "Lex Prime Barrel", "Oberon Prime Chassis", "Helios Prime Systems", "Nekros Prime Neuroptics", "Nami Skyla Prime Blade"));
            relics.Add(new Relic("Axi O1", "Euphona Prime Blueprint", "Forma Blueprint", "Paris Prime Blueprint", "Akstiletto Prime Receiver", "Galatine Prime Handle", "Oberon Prime Neuroptics"));
            relics.Add(new Relic("Axi V6", "Ballistica Prime Lower Limb", "Braton Prime Blueprint", "Forma Blueprint", "Fang Prime Handle", "Galatine Prime Handle", "Valkyr Prime Chassis"));
            relics.Add(new Relic("Axi V7", "Lex Prime Barrel", "Nami Skyla Prime Blueprint", "Valkyr Prime Blueprint", "Kogake Prime Blueprint", "Silva & Aegis Prime Blueprint", "Venka Prime Gauntlet"));


        }

        public void Save(String file = "items.txt")
        {
            File.WriteAllText(file, Newtonsoft.Json.JsonConvert.SerializeObject(items));
        }

        public async Task<String> GetItemProperty(String name, String property)
        {
            name = name.ToUpper();
            if (items.TryGetValue(name, out Dictionary<String, String> data))
            {
                if (data.TryGetValue(property, out string property_value))
                {
                    return property_value;
                }
                await UpdateItemData(name);
                if (data.TryGetValue(property, out property_value))
                {
                    return property_value;
                }
            }
            return "";
        }

        public async Task<Bitmap> GetItemThumbnail(String name)
        {
            if (!Directory.Exists("images"))
                Directory.CreateDirectory("images");
            while (image_lock) { await Task.Delay(333); }
            image_lock = true;
            name = name.ToUpper();
            //richTextBox1.Text += "Finding " + name + " thumbnail\n";
            Bitmap thumb = null;
            if (items.TryGetValue(name, out Dictionary<String, String> data))
            {
                if (data.TryGetValue("local_thumb", out string file_name) && File.Exists(file_name))
                {
                    //richTextBox1.Text += "using local file\n";
                    try
                    {
                        image_lock = false;
                        return new Bitmap(file_name);
                    }
                    catch (Exception e)
                    {
                        //corrupt file
                    }
                }
                if (!data.ContainsKey("thumb"))
                {
                    await UpdateItemData(name);
                    if (!data.ContainsKey("thumb"))
                    {
                        image_lock = false;
                        return null;
                    }

                }
                await AwaitHttpLock();
                Stream s = await image_client.GetStreamAsync(data["thumb"]);
                http_lock = false;
                thumb = new Bitmap(Image.FromStream(s));
                String fname = @"images/" + name + "thumb.bmp";
                thumb.Save(fname);
                data["local_thumb"] = fname;
                items[name] = data;
            }
            image_lock = false;
            return thumb;
        }

        public async Task<float> GetItemPrice(String name, String metric = "avg_price")
        {
            name = name.ToUpper();
            if (items.TryGetValue(name, out Dictionary<String, String> data))
            {
                //check if it has values and values were updated today
                String price = "";
                if (!data.TryGetValue("Price Last Updated", out string update_string) ||
                    !DateTime.TryParse(update_string, out DateTime time) ||
                    time.Date != DateTime.UtcNow.Date)
                {

                    await UpdateItemPrice(name);
                    if (!data.TryGetValue(metric, out price))
                    {
                        return 0;
                    }

                }
                else if (!data.TryGetValue(metric, out price))
                {
                    return 0;
                }
                if (float.TryParse(price, out float p))
                    return p;
                
            }
            return 0;
        }

        private async Task UpdateItemData(String name)
        {
            name = name.ToUpper();
            if (items.TryGetValue(name, out Dictionary<String, String> data))
            {
                if (data.TryGetValue("url_name", out string url_name))
                {
                    await AwaitHttpLock();
                    using (HttpResponseMessage responce = await client.GetAsync(@"items/" + url_name))
                    {
                        http_lock = false;
                        var str = await responce.Content.ReadAsStringAsync();
                        dynamic payload = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                        Newtonsoft.Json.Linq.JArray jitems = payload.payload.item.items_in_set;
                        foreach (Newtonsoft.Json.Linq.JObject jitem in jitems)
                        {
                            String iname = jitem.Value<Newtonsoft.Json.Linq.JObject>("en").Value<String>("item_name").ToUpper();

                            if (items.TryGetValue(iname, out Dictionary<String, String> possible_data))
                            {
                                if (jitem.TryGetValue("ducats", out Newtonsoft.Json.Linq.JToken ducats))
                                {
                                    possible_data["ducats"] = ducats.ToString();

                                }
                                if (jitem.TryGetValue("thumb", out Newtonsoft.Json.Linq.JToken thumb))
                                {
                                    possible_data["thumb"] = thumb.ToString();
                                }
                                if (jitem.TryGetValue("rarity", out Newtonsoft.Json.Linq.JToken rarity))
                                {
                                    possible_data["rarity"] = rarity.ToString();
                                }
                            }
                        }
                    }
                }
            }
        }

        private async Task UpdateItemPrice(String name)
        {
            name = name.ToUpper();
            if (items.TryGetValue(name, out Dictionary<String, String> data))
            {
                if (data.TryGetValue("url_name", out string url_name))
                {
                    await AwaitHttpLock();
                    using (HttpResponseMessage responce = await client.GetAsync(@"items/" + url_name + "/statistics"))
                    {
                        http_lock = false;
                        var str = await responce.Content.ReadAsStringAsync();
                        Newtonsoft.Json.Linq.JObject payload = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(str).payload;
                        Newtonsoft.Json.Linq.JArray jitems = payload.Value<Newtonsoft.Json.Linq.JObject>("statistics").Value<Newtonsoft.Json.Linq.JArray>("90days");
                        if (jitems.HasValues)
                        {
                            Dictionary<String, String> stats = jitems.Last.ToObject<Dictionary<String, String>>();
                            foreach (var item in stats)
                            {
                                data[item.Key] = item.Value;
                            }
                        }
                        data["Price Last Updated"] = DateTime.UtcNow.ToString();
                    }
                }
            }
        }

        private async void FetchItems()
        {
            using (HttpResponseMessage responce = await client.GetAsync("items"))
            {
                var str = await responce.Content.ReadAsStringAsync();
                dynamic payload = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                Newtonsoft.Json.Linq.JArray jitems = payload.payload.items.en;

                foreach (Newtonsoft.Json.Linq.JObject i in jitems)
                {
                    items.Add(i.Value<String>("item_name").ToUpper(), i.ToObject<Dictionary<String, String>>());
                }
            }
        }

        private async Task AwaitHttpLock()
        {
            //Wait for the currently executing http requst to end
            while(http_lock)
            {
                await Task.Delay(333);
            }

            //the calling event should now execute with a .5s delay to prevent spamming WFMarket
            http_lock = true;
            await Task.Delay(500);
        }

        //this is for updating the webpage and should be removed if the webpage gets an actual webhost
        //and can do this automatically
        public async void UpdateAll(System.Windows.Forms.Control log)
        {
            if (!Directory.Exists("images"))
                Directory.CreateDirectory("images");
            await AwaitHttpLock();
            String length = items.Values.Count.ToString();
            int num = 0;
            foreach (Dictionary<String, String> item in items.Values)
            {
                if (!item.TryGetValue("url_name", out String url_name))
                    continue;
                if (!item.TryGetValue("thumb", out String thumb_url))
                {
                    log.Text = num.ToString() + "/" + length + "  Updating " + url_name + " data";
                    log.Refresh();
                    try
                    {
                        using (HttpResponseMessage responce = await client.GetAsync(@"items/" + url_name))
                        {
                            var str = await responce.Content.ReadAsStringAsync();
                            dynamic payload = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                            Newtonsoft.Json.Linq.JArray jitems = payload.payload.item.items_in_set;
                            foreach (Newtonsoft.Json.Linq.JObject jitem in jitems)
                            {
                                String iname = jitem.Value<Newtonsoft.Json.Linq.JObject>("en").Value<String>("item_name").ToUpper();

                                if (items.TryGetValue(iname, out Dictionary<String, String> possible_data))
                                {
                                    if (jitem.TryGetValue("ducats", out Newtonsoft.Json.Linq.JToken ducats))
                                    {
                                        possible_data["ducats"] = ducats.ToString();

                                    }
                                    if (jitem.TryGetValue("thumb", out Newtonsoft.Json.Linq.JToken thumb))
                                    {
                                        possible_data["thumb"] = thumb.ToString();
                                    }
                                    if (jitem.TryGetValue("rarity", out Newtonsoft.Json.Linq.JToken rarity))
                                    {
                                        possible_data["rarity"] = rarity.ToString();
                                    }
                                }
                            }
                        }
                        item.TryGetValue("thumb", out thumb_url);
                        await Task.Delay(1000);
                    }
                    catch (Exception e) { }
                    
                }
                try
                {
                    log.Text = num.ToString() + "/" + length + "  Updating " + url_name + " price";
                    log.Refresh();
                    using (HttpResponseMessage responce = await client.GetAsync(@"items/" + url_name + "/statistics"))
                    {
                        var str = await responce.Content.ReadAsStringAsync();
                        Newtonsoft.Json.Linq.JObject payload = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(str).payload;
                        Newtonsoft.Json.Linq.JArray jitems = payload.Value<Newtonsoft.Json.Linq.JObject>("statistics").Value<Newtonsoft.Json.Linq.JArray>("90days");
                        if (jitems.HasValues)
                        {
                            Dictionary<String, String> stats = jitems.Last.ToObject<Dictionary<String, String>>();
                            foreach (var i in stats)
                            {
                                item[i.Key] = i.Value;
                            }
                        }
                        item["Price Last Updated"] = DateTime.UtcNow.ToString();
                    }
                }
                catch (Exception e)
                { }
                await Task.Delay(1000);
                if ((!item.TryGetValue("local_thumb", out string file_name) || !File.Exists(file_name)) && thumb_url != "")
                {
                    try
                    {
                        log.Text = num.ToString() + "/" + length + "  Updating " + url_name + " thumbnail";
                        log.Refresh();
                        Stream s = await image_client.GetStreamAsync(thumb_url);
                        Bitmap thumb = new Bitmap(Image.FromStream(s));
                        String fname = "images/" + item["item_name"].ToUpper() + "thumb.bmp";
                        thumb.Save(fname);
                        item["local_thumb"] = fname;
                        await Task.Delay(1000);
                    }
                    catch (Exception e)
                    { }
                }
                ++num;
                
            }
            http_lock = false;
            Save();
            log.Text = "Done!";
        }

        public List<Relic> relics = new List<Relic>();
        private Dictionary<String, Dictionary<String, String>> items;
        private HttpClient client, image_client;
        private bool http_lock = false, image_lock = false;
        
    }
}
