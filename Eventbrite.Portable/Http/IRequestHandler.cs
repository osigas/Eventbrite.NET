using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventbriteNET.Http
{
    interface IRequestHandler
    {
        IList<T> Get<T>() where T : EventbriteObject;
        T Get<T>(long id) where T : EventbriteObject;
        void Create<T>(T entity) where T : EventbriteObject;
        void Update<T>(T entity) where T : EventbriteObject;
        Task<IList<T>> GetAsync<T>() where T : EventbriteObject;
        Task<T> GetAsync<T>(long id) where T : EventbriteObject;
        Task CreateAsync<T>(T entity) where T : EventbriteObject;
        Task UpdateAsync<T>(T entity) where T : EventbriteObject;
    }
}
