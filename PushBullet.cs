using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace DFAlert
{
    class PushBullet
    {     
        [DataContract]
        public class Note
        {
            public Note(string title, string body)
            {
                this.body = body;
                this.title = title;
                this.type = "note";
            }

            [DataMember]
            private string type;
            [DataMember]
            private string title;
            [DataMember]
            private string body;

            public void Push(string token)
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://api.pushbullet.com/v2/pushes");
                    request.Headers.Add("Authorization", string.Format("Bearer {0}", token));
                    request.Content = new StringContent(this.ToJson(), Encoding.UTF8, "application/json");
                    var client = new HttpClient();
                    client.SendAsync(request);
                }
                else 
                    Log.print("Pushbullet, missing token.");
                
            }

            public void Push()
            {
                this.Push(Settings.Current.pBullet.token);
            }
            private string ToJson() {
                using (var mStream = new MemoryStream())
                {
                    using (var sr = new StreamReader(mStream))
                    {
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Note));
                        ser.WriteObject(mStream, this);
                        mStream.Position = 0;
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}

