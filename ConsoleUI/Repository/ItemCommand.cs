using ConsoleUI.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ConsoleUI.Repository
{
    class ItemCommand
    {
        private string _connectionString;

        public ItemCommand(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IList<ItemModel> GetList()
        {
            List<ItemModel> items = new List<ItemModel>();

            var sql = "Item_GetList";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                items = connection.Query<ItemModel>(sql).ToList();
            }

            return items;
        }
    }
}
