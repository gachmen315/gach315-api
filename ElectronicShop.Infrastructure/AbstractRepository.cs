using ElectronicShop.Model.ComplexType;
using System.Data;
using System.Data.SqlClient;
using static Dapper.SqlMapper;

namespace ElectronicShop.Infrastructure
{
    public class AbstractRepository<T, Tid> where T : class
    {
        public IDbConnection ConnectionRead => new SqlConnection(ApplicationSettingFactory.GetApplicationSettings()?.ConnectionString);

        public async Task<IEnumerable<O>> Query<O>(string storeName, dynamic objectParam, DatabaseMode mode = DatabaseMode.Read, IDbConnection? dbConnection = null, IDbTransaction? transaction = null)
        {
            if (dbConnection == null) dbConnection = ConnectionRead;
            return await dbConnection.QueryAsync<O>(storeName, param: (object)objectParam, commandType: CommandType.StoredProcedure, transaction: transaction);
        }

        public async Task<O> QueryScalar<O>(string storeName, dynamic objectParam, DatabaseMode mode = DatabaseMode.Read, IDbConnection? dbConnection = null, IDbTransaction? transaction = null) where O : IComparable, IConvertible, IEquatable<O>
        {
            if (dbConnection == null) dbConnection = ConnectionRead;
            return await dbConnection.ExecuteScalarAsync<O>(storeName, param: (object)objectParam, commandType: CommandType.StoredProcedure, transaction: transaction);
        }

        public async Task<int> Execute(string storeName, dynamic objectParam, DatabaseMode mode = DatabaseMode.Read, IDbConnection? dbConnection = null, IDbTransaction? transaction = null)
        {
            if (dbConnection == null) dbConnection = ConnectionRead;
            return await dbConnection.ExecuteAsync(storeName, param: (object)objectParam, commandType: CommandType.StoredProcedure, transaction: transaction);
        }

        public async Task<GridReader> QueryMultiple(string storeName, dynamic objectParam, DatabaseMode mode = DatabaseMode.Read, IDbConnection? dbConnection = null, IDbTransaction? transaction = null)
        {
            if (dbConnection == null) dbConnection = ConnectionRead;
            return await dbConnection.QueryMultipleAsync(storeName, param: (object)objectParam, commandType: CommandType.StoredProcedure, transaction: transaction);
        }
    }
}