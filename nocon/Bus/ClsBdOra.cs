using System;
using System.Collections.Generic;
using System.Text;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;
using System.Data;

namespace nocon.Bus
{
    public class ClsBdOra
    {
        private static string connString = "Data Source=PROD_CAD_RS;User Id=dsalazar;Password=blog1002";
        //private static string connString = "Data Source=DESENV_CAD;User Id=system;Password=oracle";

        public OracleConnection conn = new OracleConnection(connString);

        public void openConn()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
        }

        public void closeConn(OracleConnection conn)
        {
            conn.Close();
        }

        public bool SETSql(string stringSql)
        {
            bool boo = false;
            OracleCommand cmd = new OracleCommand();
            try
            {
                if (this.conn.State == ConnectionState.Closed)
                    this.openConn();
                cmd.Connection = this.conn;
                OracleTransaction trans = conn.BeginTransaction();
                cmd.CommandText = stringSql;
                cmd.ExecuteNonQuery();
                trans.Commit();

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
            OracleDataAdapter da = new OracleDataAdapter();

            try
            {
                if (this.conn.State == ConnectionState.Closed)
                    this.conn.Open();
                OracleCommand cmd = new OracleCommand(stringSql, this.conn);
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

        public OracleDataReader GETSqlReader(string stringSql)
        {
            OracleDataReader dr;

            if (this.conn.State != ConnectionState.Open)
                this.conn.Open();
            OracleCommand cmd = new OracleCommand(stringSql, this.conn);
            dr = cmd.ExecuteReader();

            this.conn.Close();
            return dr;
        }
    }
}
