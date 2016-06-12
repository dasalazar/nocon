using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace nocon.Bus
{
    public class ClsBdSql
    {
        private static string connString = "Server=192.168.199.16;Database=NOCON;User Id=dsalazar;Password=blog1002;";
        
        public SqlConnection conn = new SqlConnection(connString);

        public void openConn()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
        }

        public void closeConn(SqlConnection conn)
        {
            conn.Close();
        }

        public bool SETSql(string stringSql)
        {
            bool boo = false;
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (this.conn.State == ConnectionState.Closed)
                    this.openConn();
                cmd.Connection = this.conn;
                cmd.CommandText = stringSql;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                this.conn.Close();
            }
        }

        public DataSet GETSql(string stringSql)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                if (this.conn.State == ConnectionState.Closed)
                    this.conn.Open();
                SqlCommand cmd = new SqlCommand(stringSql, this.conn);
                da.SelectCommand = cmd;
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                this.conn.Close();
            }
        }

        public Object GETSqlScalar(string stringSql)
        {
            DataSet ds = new DataSet();
            try
            {
                if (this.conn.State == ConnectionState.Closed)
                    this.conn.Open();
                SqlCommand cmd = new SqlCommand(stringSql, this.conn);
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                this.conn.Close();
            }
        }

        public SqlDataReader GETSqlReader(string stringSql)
        {
            SqlDataReader dr;

            if (this.conn.State != ConnectionState.Open)
                this.conn.Open();
            SqlCommand cmd = new SqlCommand(stringSql, this.conn);
            dr = cmd.ExecuteReader();

            this.conn.Close();
            return dr;
        }
    }
}
