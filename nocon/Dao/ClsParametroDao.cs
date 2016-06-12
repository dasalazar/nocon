using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using nocon.Bus;

namespace nocon.Dao
{
    public class ClsParametroDao
    {
        #region SQLs
        private string getSQL_GETValorParam(string chave)
        {
            string strSql = "SELECT * FROM dbo.TAB_PARAMETRO WHERE CHAVE_PARAM = '" + chave +"'";
            return strSql;
        }

        private string getSQL_SETValorParam(string chave, string novoValor)
        {
            string strSql = "UPDATE dbo.TAB_PARAMETRO SET PARAMETRO = '" + novoValor + "' WHERE CHAVE_PARAM = '" + chave + "'";
            return strSql;
        }


        #endregion

        #region CRUD
        public DataSet getValorParam(string chave)
        {
            DataSet dt;
            string c = chave;
            ClsBdSql banco = new ClsBdSql();
            try
            {
                banco.openConn();
                dt = banco.GETSql(this.getSQL_GETValorParam(c));
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool setValorParam(string chave, string novoValor)
        {
            string c = chave;
            string n = novoValor;
            ClsBdSql banco = new ClsBdSql();
            try
            {
                banco.openConn();
                return banco.SETSql(this.getSQL_SETValorParam(c,n));
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
