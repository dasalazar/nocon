using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using nocon.Bus;

namespace nocon.Dao
{
    public class ClsEquipamentoDao
    {
        #region SQLs
        private string getSQL_GETEquipamentos()
        {
            string strSql = "SELECT TAB_EQUIPAMENTO.ID_EQUIPAMENTO, ";
            strSql += "TAB_EQUIPAMENTO.NOME_EQUIPAMENTO, ";
            strSql += "TAB_EQUIPAMENTO.ID_CLIENTE, ";
            strSql += "TAB_CLIENTE.NOME_CLIENTE, ";
            strSql += "TAB_EQUIPAMENTO.ID_LOCAL ";
            strSql += "FROM  ";
            strSql += "TAB_EQUIPAMENTO INNER JOIN TAB_CLIENTE ";
            strSql += "ON TAB_EQUIPAMENTO.ID_CLIENTE = TAB_CLIENTE.ID_CLIENTE ";
            return strSql;
        }
        #endregion

        #region CRUD

        public DataSet getEquipamentos()
        {
            ClsBdSql banco = new ClsBdSql();
            try
            {
                banco.openConn();
                return banco.GETSql(this.getSQL_GETEquipamentos());
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
