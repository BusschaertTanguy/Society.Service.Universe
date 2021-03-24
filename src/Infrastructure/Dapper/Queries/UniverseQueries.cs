using System.Threading.Tasks;
using Application.Queries;
using Application.ReadModels;
using Dapper;
using Infrastructure.Queries;

namespace Infrastructure.Dapper.Queries
{
    internal class UniverseQueries : IUniverseQueries
    {
        private readonly SqlDbConnectionProvider _connectionProvider;

        public UniverseQueries(SqlDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<UniverseCurrentTimeReadModel> GetCurrentTime()
        {
            using var connection = _connectionProvider.GetDbConnection();

            const string dbQuery = "SELECT [CurrentTime] FROM [Universe];";

            return await connection.QueryFirstOrDefaultAsync<UniverseCurrentTimeReadModel>(dbQuery);
        }
    }
}