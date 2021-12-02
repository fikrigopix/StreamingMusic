using StreamingMusic.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StreamingMusic.Interfaces
{
    public interface IDatabaseService
    {
        Task Init();
        Task Add(DateTime latestOpenApp);
        Task<IEnumerable<Table1>> GetData();
        Task<Table1> GetData(int id);
        Task Remove(int id);

    }
}
