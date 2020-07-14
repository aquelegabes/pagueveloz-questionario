using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using PagueVeloz.Application.Interfaces;
using PagueVeloz.Domain.ViewModels;

namespace PagueVeloz.Application.Services
{
    public static class FornecedorServiceHelper
    {
        public static async Task<IEnumerable<FornecedorVM>> GetPaginated(
            this IAPIService<FornecedorVM> service, string endpoint,
            string cpf = "", string cnpj = "", string nome = "", DateTime createdAt = default,
            int page = 1, int take = 15)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrWhiteSpace(cpf))
                query["cpf"] = cpf;
            if (!string.IsNullOrWhiteSpace(cnpj))
                query["cnpj"] = cnpj;
            if (!string.IsNullOrWhiteSpace(nome))
                query["nome"] = nome;
            if (createdAt.Date != default)
                query["createdAt"] = JsonConvert.SerializeObject(createdAt);

            query["page"] = page.ToString();
            query["take"] = take.ToString();

            string requestUri = endpoint.EndsWith("/") ?
                endpoint.Remove(endpoint.Length - 1) + "?" + query.ToString() :
                endpoint + "?" + query.ToString();

            try
            {
                using (var reqMsg = new HttpRequestMessage(HttpMethod.Get, requestUri))
                {
                    var response = await service.Client.SendAsync(reqMsg);
                    response.EnsureSuccessStatusCode();
                    var jsonString = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<IEnumerable<FornecedorVM>>(jsonString);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}