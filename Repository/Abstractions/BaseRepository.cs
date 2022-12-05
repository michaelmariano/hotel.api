using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace Repository.Abstractions
{
    public abstract class BaseRepository<TEntity>
    {
        protected string ConnectionRead => GetConnectionRead();
        private string ConnectionWrite => GetConnectionWrite();

        protected string Table { get; set; }
        protected string SchemaName { get; set; }
        protected DynamicParameters _params;
        protected string _sqlCommand;

        private static IConfiguration Configuration;


        public BaseRepository()
        {
            var type = typeof(TEntity);

            var attribute = type.GetCustomAttribute<TableAttribute>();

            if (attribute == null)
                throw new MissingFieldException($"The Type {type.Name} does not has the TableAttribute");

            this.Table = attribute.Name;

            _params = new DynamicParameters();
        }

        private static string GetConnectionRead()
        {
            if (Configuration == null)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                Configuration = builder.Build();
            }

            return Configuration["ConnectionStrings:MainConnectionRead"];
        }

        private static string GetConnectionWrite()
        {
            if (Configuration == null)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                Configuration = builder.Build();
            }

            return Configuration["ConnectionStrings:MainConnectionWrite"];
        }

        public async Task ExecuteOneRecordAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            int rows = 0;

            using (var db = new NpgsqlConnection(ConnectionWrite))
            {
                await db.OpenAsync();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        rows = await db.ExecuteAsync(sql, param, commandTimeout: commandTimeout, commandType: commandType);
                    }
                    catch (Exception ex)
                    {
                        //Todo log
                    }
                    finally
                    {
                        if (rows == 1)
                            await transaction.CommitAsync();
                        else
                            await transaction.RollbackAsync();
                    }
                }
            }
        }

        public async Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            int result = 0;

            try
            {
                using (var db = new NpgsqlConnection(ConnectionWrite))
                {
                    result = (await db.ExecuteAsync(sql, param, commandTimeout: commandTimeout, commandType: commandType));
                }
            }
            catch (Exception ex)
            {
                //TODO Log
            }

            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var db = new NpgsqlConnection(ConnectionRead))
                return (await db.QueryAsync<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType));
        }
    }
}