using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Asp_Xioma
{
    public class DB
    {
        public static string ConnString { get; set; }

        //Execute connection to database and pulling data
        public static List<T> PullData<T>(string sql, Func<SqlDataReader, T> processRecord, Action<SqlCommand> setParameters = null)
        {
            List<T> list = new List<T>();
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (setParameters != null)
                        setParameters(cmd);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                            list.Add(processRecord(dr));
                    }
                }
            }

            return list;
        }


        //Execute connection to databse and insert data, delete data or update data.
        public static int Modify(string sql, Action<SqlCommand> setParameters)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (setParameters != null)
                    {
                        setParameters(cmd);
                    }
                    rowsAffected = cmd.ExecuteNonQuery();
                }


            }
            return rowsAffected;
        }

    }
}
