using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using PagueVeloz.Application.Interfaces;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace PagueVeloz.Application.Services
{
    public class APIService<TViewModel> : IAPIService<TViewModel>
    {
        public HttpClient Client { get; }

        public APIService(
            HttpClient client,
            IConfiguration config
        )
        {
            client.BaseAddress = new Uri(config["API:BaseUrl"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );

            Client = client;
        }

        public async Task<TViewModel> Add(string endpoint, TViewModel model)
        {
            using (var reqMsg = new HttpRequestMessage(HttpMethod.Post, endpoint))
            {
                reqMsg.Content = new StringContent(
                    content: JsonConvert.SerializeObject(model),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json"
                );

                var response = await Client.SendAsync(reqMsg);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TViewModel>(jsonString);
                }
                else {
                    var err = new HttpRequestException(message: "error posting object.");
                    err.Data["response"] = response;

                    throw err;
                }
            }
        }

        public async Task<bool> Delete(string endpoint, int id)
        {
            using (var reqMsg = new HttpRequestMessage(HttpMethod.Delete, endpoint + id))
            {
                var response = await Client.SendAsync(reqMsg);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else {
                    var err = new HttpRequestException(message: "error posting object.");
                    err.Data["response"] = response;

                    throw err;
                }
            }
        }

        public async Task<TViewModel> GetById(string endpoint, int id)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, endpoint + id))
            {
                var response = await Client.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TViewModel>(jsonString);
            }
        }

        public async Task<TViewModel> Update(string endpoint, TViewModel model)
        {
            using (var reqMsg = new HttpRequestMessage(HttpMethod.Put, endpoint))
            {
                reqMsg.Content = new StringContent(
                    content: JsonConvert.SerializeObject(model),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json"
                );

                var response = await Client.SendAsync(reqMsg);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TViewModel>(jsonString);
                }
                else {
                    var err = new HttpRequestException(message: "error posting object.");
                    err.Data["response"] = response;

                    throw err;
                }
            }
        }
    }
}