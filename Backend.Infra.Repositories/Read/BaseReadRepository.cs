using Npgsql;
using System.Data;

namespace Backend.Infra.Repositories.Read
{
    public abstract class BaseReadRepository
    {
        protected IDbConnection GetConnection()
        {
            var connection = new NpgsqlConnection(Settings.ConnectionString);

            connection.Open();

            // if (setSearchPath)
            // {
            //using (var cmd = connection.CreateCommand())
            //{
            //    cmd.CommandText = string.Format("SET search_path TO {0}, public", searchPath);
            //    cmd.ExecuteNonQuery();
            //}
            // }

            return connection;
        }
    }
}
