using System.Threading.Tasks;
using Application.ReadModels;

namespace Application.Queries
{
    internal interface IUniverseQueries
    {
        Task<UniverseCurrentTimeReadModel> GetCurrentTime();
    }
}