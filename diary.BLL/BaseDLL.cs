using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using diary.Entities;

namespace diary.BLL
{
    public class BaseDLL
    {
        const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\rafa_\OneDrive\VS\DiaryElite\diary.DAL\DiaryEliteDb.mdf;Integrated Security=True;User Instance=False";

        public SqlCommand GetCommand(String query)
        {
            var connection = new SqlConnection(ConnectionString);

            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = connection;
            return cmd;
        }
    }
}
