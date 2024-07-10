using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace DFAlert
{
    internal class PushBullet
    {
        public class Note(string title, string body)
        {
            public string Type => "note";
            public string Title { get; set; } = title;
            public string Body { get; set; } = body;

            public void Push(string token)
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://api.pushbullet.com/v2/pushes");
                    request.Headers.Add("Authorization", $"Bearer {token}");
                    request.Content = new StringContent(this.ToJson(), Encoding.UTF8, "application/json");

                    var client = new HttpClient();
                    client.SendAsync(request);
                }
                else
                    Log.Print("PushBullet, missing token.");
            }


            public void Push()
            {
                this.Push(Settings.Current.pBullet.token);
            }

            public string ToJson()
            {
                return $"{{\"type\":\"note\",\"title\":\"{title}\",\"body\":\"{body}\"}}";
            }
        }
    }
}