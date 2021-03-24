using System.Threading.Tasks;
using Application.ReadModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries
{
    public static class GetCurrentTime
    {
        public class Query : IRequest<UniverseCurrentTimeReadModel>
        {
        }

        internal class Handler : QueryHandler<Query, UniverseCurrentTimeReadModel>
        {
            private readonly IUniverseQueries _queries;

            public Handler(ILogger<QueryHandler<Query, UniverseCurrentTimeReadModel>> logger, IUniverseQueries queries) : base(logger)
            {
                _queries = queries;
            }

            protected override Task<UniverseCurrentTimeReadModel> Process(Query query)
            {
                return _queries.GetCurrentTime();
            }
        }
    }
}