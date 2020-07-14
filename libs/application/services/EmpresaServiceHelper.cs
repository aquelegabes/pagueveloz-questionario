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
    public static class EmpresaServiceHelper
    {
        public static async Task<IEnumerable<EmpresaVM>> GetPaginated(
            this IAPIService<EmpresaVM> service, string endpoint,
            string cnpj = "", string nome = "", bool semFornecedor = false,
            int includeId = 0, int page = 1, int take = 15)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrWhiteSpace(cnpj))
                query["cnpj"] = cnpj;
            if (!string.IsNullOrWhiteSpace(nome))
                query["nome"] = nome;
            if (includeId != 0)
                query["includeId"] = includeId.ToString();


            query["semFornecedor"] = semFornecedor.ToString();
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

                    return JsonConvert.DeserializeObject<IEnumerable<EmpresaVM>>(jsonString);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}