using StreamingMusic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StreamingMusic.Interfaces
{
    public interface IDatabaseService
    {
        Task Add(string name);
        Task<IEnumerable<MyData>> GetData();
        Task<MyData> GetData(int id);
        Task Remove(int id);
    }
}
