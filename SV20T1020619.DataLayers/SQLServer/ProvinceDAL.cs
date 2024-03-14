using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using SV20T1020619.DomainModels;

namespace SV20T1020619.DataLayers.SQLServer
{
    public class ProvinceDAL : BaseDAL, ICommonDAL<Province>
    {
        public ProvinceDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Province data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue = "")
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Province? Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsUsed(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Province> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Province> List = new List<Province>();
            using (var connection = OpenConnection())
            {
                var sql = @"select * from Provinces";
                List = connection.Query<Province>(sql: sql, commandType: CommandType.Text).ToList();
                connection.Close();
            }
            return List;
        }

        public bool Update(Province data)
        {
            throw new NotImplementedException();
        }
    }
}
