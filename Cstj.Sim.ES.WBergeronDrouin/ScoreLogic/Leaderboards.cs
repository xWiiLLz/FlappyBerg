using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBerg.ScoreLogic
{
    public class Leaderboards
    {
        public List<ScoreEntry> Scores { get; private set; }

        public Leaderboards()
        {
            Scores = new List<ScoreEntry>();
        }

        public void LoadTop10()
        {
            if (Scores.Count != 0)
            {
                Scores.Clear();
            }
            WebClient client = new WebClient();
            Stream stream;
            try
            {
                stream = client.OpenRead("http://api.flappyberg.com/top10.php");
                StreamReader reader = new StreamReader(stream);

                Newtonsoft.Json.Linq.JArray jArray = Newtonsoft.Json.Linq.JArray.Parse(reader.ReadLine());
                foreach (var entry in jArray)
                {
                    Scores.Add(JsonConvert.DeserializeObject<ScoreEntry>(entry.ToString()));
                }
            }
            catch
            {
                throw new Exception("Server");
            }
        }

        public static bool AddScoreEntry(ScoreEntry scoreEntry)
        {
            JObject scoreJObject = JObject.FromObject(scoreEntry);
            string url = "http://api.flappyberg.com/send_score.php";

            string result = "";
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                try
                {
                    result = client.UploadString(url, "POST", JObject.FromObject(scoreEntry).ToString(Formatting.None));
                    client.Dispose();
                }
                catch(Exception e)
                {
                    client.Dispose();
                    throw e;
                }
            }
            if(result.Contains("Succes"))
            {
                return true;
            }
            else if(result.Contains("Error query"))
            {
                throw new Exception("Query");
            }
            else if(result.Contains("Error db"))
            {
                throw new Exception("DB");
            }
            else
            {
                return false;
            }
            
        } 
    }
}
