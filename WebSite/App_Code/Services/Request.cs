using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebSite.App_Code.Services
{
    public class Request : IRequest
    {
        public async Task DeleteAsync(string uri)
        {
            HttpClient httpClient = new HttpClient();
            await httpClient.DeleteAsync(uri);
        }

        public async Task<TResult> GetAsync<TResult>(string uri)
        {
            try
            {


                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(uri);


                string serialized = await response.Content.ReadAsStringAsync();

                TResult result = await Task.Run(() =>
                    JsonConvert.DeserializeObject<TResult>(serialized));

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<TResult> PostAsync<TResult>(string uri, TResult data)
        {
            HttpClient httpClient = new HttpClient();


            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);


            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized));

            return result;
        }

        //public async Task<TResult> PostAsync<TResult>(string uri, string data, string clientId)
        //{
        //    HttpClient httpClient = new HttpClient();

        //                var content = new StringContent(data);
        //    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        //    HttpResponseMessage response = await httpClient.PostAsync(uri, content);


        //    string serialized = await response.Content.ReadAsStringAsync();

        //    TResult result = await Task.Run(() =>
        //        JsonConvert.DeserializeObject<TResult>(serialized));

        //    return result;
        //}

        public async Task<TResult> PutAsync<TResult>(string uri, TResult data)
        {
            HttpClient httpClient = new HttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PutAsync(uri, content);


            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized));

            return result;
        }
    }
}