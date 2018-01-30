using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace InstaPhoto
{
    public class AddInDatanase:Dataprovider
    {

        DownloadImages _down = new DownloadImages();
        public void createTable (string name_tb)
        {
            try
            {
                ConnectDatabase();
                string str = "IF NOT EXISTS(SELECT * from sysobjects WHERE name = '" + name_tb + "')";
                str += " BEGIN";
                str += " CREATE TABLE " + name_tb + "";
                str += " (ID int  PRIMARY KEY NOT NULL,";
                str += " Image Varchar(500) NOT NULL,";
                str += " Image_name Varchar(500) NOT NULL,";
                str += " Username text NOT NULL,";
                str += " Profile_picture text NULL,";
                str += " Caption text NULL,";
                str += " Tag text NULL)";
                str += " END ";
                
                cmd = new SqlCommand(str,conn);
                cmd.ExecuteNonQuery();
                CloseConnection();
                GCLass.Table_name= name_tb;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            
        }
        public void AddImages (string image,string User,string profile_picture, string caption,string tags)
        {

            int _ID = Execute.Readvarible(GCLass.Path + "//ID.txt");
            _ID++;
            Execute.Savevarible(GCLass._path_ID, _ID.ToString());
            try
            {
                if (CheckImages(image))
                {
                    ConnectDatabase();
                    string str = "INSERT INTO " + GCLass.Table_name + " (ID,Image,Image_name,Username,Profile_picture,Caption,Tag) Values (@ID, @Image ,@Image_name, @Username, @Profile_picture, @Caption, @Tag)";
                    cmd = new SqlCommand(str, conn);
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = _ID;
                    cmd.Parameters.Add("@Image", SqlDbType.Text).Value = image;
                    cmd.Parameters.Add("@Image_name", SqlDbType.Text).Value = _down.Img_name(image);
                    cmd.Parameters.Add("@Username", SqlDbType.Text).Value = User;
                    cmd.Parameters.Add("@Profile_picture", SqlDbType.Text).Value =profile_picture;
                    cmd.Parameters.Add("@Caption", SqlDbType.Text).Value = caption;
                    cmd.Parameters.Add("@Tag", SqlDbType.Text).Value = tags;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    CloseConnection();
                }
                    
                
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public bool CheckImages(string image)
        {
            try
            {
                DataTable _tb = new DataTable();
                ConnectDatabase();
                string _sqlQuery = "select * from " + GCLass.Table_name;
                _sqlQuery += " WHERE Image = '" + image + "'";
                _sqlQuery += " OR Image_name = '" +_down.Img_name( image) + "'";
                da = new SqlDataAdapter(_sqlQuery, conn);
                da.Fill(_tb);

                CloseConnection();
                if (_tb.Rows.Count >0)
                    return false;
                return true;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataTable ListPicture(string nameTable)
        {
            DataTable _tb = new DataTable();
            ConnectDatabase();
            string _sqlQuery = "select * from "+nameTable;
            da = new SqlDataAdapter(_sqlQuery, conn);
            da.Fill(_tb);
            return _tb;
        }

    }
}
