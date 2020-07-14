using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PagueVeloz.Application.Interfaces
{
    public interface IAPIService<T>
    {
        HttpClient Client { get; }
        Task<T> GetById(string endpoint, int id);
        Task<T> Add(string endpoint, T model);
        Task<T> Update(string endpoint, T model);
        Task<bool> Delete(string endpoint, int id);
    }
}