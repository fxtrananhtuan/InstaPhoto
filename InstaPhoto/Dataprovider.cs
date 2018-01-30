using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace InstaPhoto
{
    public class Dataprovider
    {
        protected SqlConnection conn;
        protected SqlCommand cmd;
        protected SqlDataAdapter da;
        protected SqlConnection connection;

        public bool ConnectDatabase()
        {
            string _strconn = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=InstaPhoto;Integrated Security=True";
            try
            {
                conn = new SqlConnection(_strconn);
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }

        }
        public void CloseConnection()
        {
            conn.Close();
        }

    }
}
