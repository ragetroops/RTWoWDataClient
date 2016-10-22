using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RTWoWDataClient
{
    class Program
    {

        private enum eventTypes
        {
            Raid = 1,
            Dungeon = 2,
            PvP = 3,
            Meeting = 4,
            Other = 5,
            HeroicDungeon = 6
        }


        static void Main(string[] args)
        {
            string[] contents = File.ReadAllLines(Properties.Settings.Default.SavedVariablesPath);
            foreach (string item in contents)
            {
                if (item != "")
                {
                    produceJSON(item);
                }
            }
        }

        private static void produceJSON(string item)
        {
            string datatype = item.Split('=')[0].Trim();
            string data = item.Split('=')[1].Trim().Trim('"').TrimEnd(';');

            if (datatype == "GuildEvents")
            {
                string[] elements = data.Split(';');

                JArray events = new JArray();
                foreach (string x in elements)
                {
                    string[] eventdetails = x.Split(',');
                    events.Add(new JObject(
                        new JProperty("date", eventdetails[1] + "." + eventdetails[0]),
                        new JProperty("weekday", eventdetails[2]),
                        new JProperty("time", eventdetails[3] + ":" + eventdetails[4]),
                        new JProperty("eventType", Enum.GetName(typeof(eventTypes),Convert.ToInt32(eventdetails[5]))),
                        new JProperty("title", eventdetails[6]),
                        new JProperty("calendarType", eventdetails[7]),
                        new JProperty("texture", eventdetails[8])));//TODO: enum
                }
                JObject guildEvents = new JObject(
                    new JProperty("GuildEvents", events));
                Console.WriteLine(guildEvents.ToString());
            }
        }
    }
}
