using System;
using System.Text;
using System.Net.Http;
using System.Net;
using System.Collections.Specialized;

namespace ServerApiTesting
{
    public class ApiWorker
    {
        public string url { get; set; }

        public ApiWorker(string url)
        {
            this.url = url;
        }

        public string Get(string uri)
        {
            var url = $"{this.url}{uri}";
            var result = "";
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(url);
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                result = response.Content.ReadAsStringAsync().Result;
            }
            return result;
        }


        public void Put(string uri, byte[] data)
        {
            var url = $"{this.url}{uri}";
            using (var client = new System.Net.WebClient())
            {

                //Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                //client.Headers.Add("Authorization", Convert.ToBase64String(encoding.GetBytes("test:test")));
                client.UploadData(url, "PUT", data);
            }
        }

        public void PutAuth(string uri, string login, string password, byte[] data)
        {
            var url = $"{this.url}{uri}";
            using (var client = new System.Net.WebClient())
            {

                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                client.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(encoding.GetBytes($"{login}:{password}")));
                client.UploadData(url, "PUT", data);
            }
        }

        public void Delete(string uri)
        {
            var url = $"{this.url}{uri}";
            var data = new byte[0];
            using (var client = new System.Net.WebClient())
            {
                client.UploadData(url, "DELETE", data);
            }
        }

        public string Post(string uri)
        {
            string responseInString = "";
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["username"] = "myUser";
                data["password"] = "myPassword";
                var response = wb.UploadValues(url, "POST", data);
                responseInString = Encoding.UTF8.GetString(response);
            }
            return responseInString;
        }
    }
}
