using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarframeInventoryAuditor
{
    public class Relic
    {
        public Relic(String n, String c1, String c2, String c3, String u1, String u2, String r)
        {
            name = n;
            items = new string[6];
            items[0] = c1;
            items[1] = c2;
            items[2] = c3;
            items[3] = u1;
            items[4] = u2;
            items[5] = r;
        }

        public String GetName()
        {
            return name;
        }

        public String GetItemName(int index)
        {
            if (index > 5)
                return "";
            return items[index];
        }

        private String name;
        private string[] items;
    }
}
