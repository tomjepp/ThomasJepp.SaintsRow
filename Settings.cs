using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ThomasJepp.SaintsRow
{
    public class Settings
    {
        private static string ThisAssemblyPath
        {
            get
            {
                return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
        }


        public string SaintsRow2Path { get; private set; }
        public string SaintsRowTheThirdPath { get; private set; }
        public string SaintsRowIVPath { get; private set; }
        public string SaintsRowGatOutOfHellPath { get; private set; }

        public Settings()
        {
            JObject json = null;

            using (StreamReader sr = new StreamReader(Path.Combine(ThisAssemblyPath, "settings.json")))
            {
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    JToken token = JToken.ReadFrom(reader);
                    json = token as JObject;
                }
            }

            if (json == null)
            {
                throw new InvalidDataException("Could not parse settings.json!");
            }

            JObject paths = json["paths"] as JObject;
            if (paths == null)
            {
                throw new InvalidDataException("Could not find a 'paths' object in settings.json.");
            }

            SaintsRow2Path = paths["sr2"].Value<string>();
            SaintsRowTheThirdPath = paths["srtt"].Value<string>();
            SaintsRowIVPath = paths["sriv"].Value<string>();
            SaintsRowGatOutOfHellPath = paths["srgooh"].Value<string>();
        }
    }
}
