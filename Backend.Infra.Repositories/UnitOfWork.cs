using Backend.Domain.Interfaces.Infra;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Backend.Infra.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        protected IDbConnection _connection;
        IDbTransaction _transaction;

        public void BeginTransaction()
        {
            if (_transaction == null)
                _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                try
                {
                    _transaction.Commit();
                }
                finally
                {
                    if (!((NpgsqlTransaction)_transaction).IsCompleted)
                        _transaction.Rollback();

                    _transaction = null;
                }

            }
        }

        public dynamic OpenConnection()
        {
            if (_connection == null)
            {
                _connection = new NpgsqlConnection(Settings.ConnectionString);
                _connection.Open();

                // if (setSearchPath)
                // {
                //     using (var cmd = _connection.CreateCommand())
                //     {
                //         cmd.CommandText = string.Format("SET search_path TO {0}, public", searchPath);
                //         cmd.ExecuteNonQuery();
                //     }
                // }
            }

            return _connection;
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                try
                {
                    if (!((NpgsqlTransaction)_transaction).IsCompleted)
                        _transaction.Rollback();
                }
                finally
                {
                    _transaction = null;
                }
            }
        }

        public void CloseConnection()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection = null;
            }
        }

        public void Dispose()
        {
            if ((_connection != null) && (_connection.State != ConnectionState.Closed))
            {
                _connection.Close();
                _connection = null;
            }
        }
    }
}
