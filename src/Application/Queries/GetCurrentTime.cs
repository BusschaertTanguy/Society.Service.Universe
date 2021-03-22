using System;
using System.Threading.Tasks;
using Application.Connections;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries
{
    public static class GetCurrentTime
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
            public Result(DateTime currentTime)
            {
                CurrentTime = currentTime;
            }

            public DateTime CurrentTime { get; }
        }

        internal class Handler : QueryHandler<Query, Result>
        {
            private readonly IDbConnectionProvider _connectionProvider;

            public Handler(ILogger<QueryHandler<Query, Result>> logger, IDbConnectionProvider connectionProvider) : base(logger)
            {
                _connectionProvider = connectionProvider;
            }

            protected override async Task<Result> Process(Query query)
            {
                using var connection = _connectionProvider.GetDbConnection();

                const string dbQuery = "SELECT [CurrentTime] FROM [Universe];";

                return await connection.QueryFirstOrDefaultAsync<Result>(dbQuery);
            }
        }
    }
}