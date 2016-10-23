using System.IO;
using System.Collections.Specialized;
using System;
using System.Net;

namespace RTWoWDataClient
{
    class Program
    {
        static void Main()
        {
            string[] contents = File.ReadAllLines(Properties.Settings.Default.SavedVariablesPath);
            NameValueCollection kv = new NameValueCollection();
            foreach (string item in contents)
            {
                if (item != "")
                {
                    kv.Add(Sanitize(item));
                }
            }
            using (WebClient client = new WebClient())
            {
                client.UploadValues("http://ragetroops.eu/api/wowdata/input", kv);
            }
            Console.WriteLine(kv.Get("GuildEvents"));
        }

        private static NameValueCollection Sanitize(string item)
        {
            NameValueCollection ret = new NameValueCollection();
            string[] split = item.Split('=');
            ret.Add(split[0].Trim(), split[1].Trim().Trim('"').Trim(';')); // Not pretty
            return ret;
        }
    }
}
