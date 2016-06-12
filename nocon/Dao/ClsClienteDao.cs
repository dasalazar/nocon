using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using nocon.Bus;

namespace nocon.Dao
{
    /// <summary>
    /// Douglas Salazar
    /// Classe de acesso aos dados;
    /// SQL, métodos exclusivos para tratamento de captura, alteração, exclusão dos dados
    /// </summary>
    public class ClsClienteDao
    {
        #region SQLs
        private string getSQLOssDia(string data)
        {

            string strSql = "SELECT * FROM TAB_OSS INNER JOIN TAB_CLIENTES ";
            strSql += " ON TAB_OSS.ID_CLIENTE = TAB_CLIENTES.ID_CLIENTE ";
            strSql += " INNER JOIN TAB_ENDERECOS ";
            strSql += " ON TAB_CLIENTES.ID_CEP = TAB_ENDERECOS.ID_CEP ";
            strSql += " INNER JOIN TAB_NODES ";
            strSql += " ON TAB_ENDERECOS.ID_NODE = TAB_NODES.ID_NODE ";
            strSql += " INNER JOIN TAB_AREAS ";
            strSql += " ON TAB_NODES.ID_AREA = TAB_AREAS.ID_AREA ";
            strSql += " WHERE DATA_ATEND = '" + DateTime.Parse(data).ToString("yyyy-M-d") + "' ORDER BY TAB_ENDERECOS.DESC_ENDERECO ASC";
            //strSql += " SELECT * FROM TAB_OSS INNER JOIN TAB_CLIENTES ";
            //strSql += " ON TAB_OSS.ID_CLIENTE = TAB_CLIENTES.ID_CLIENTE ";

            return strSql;
        }
        #endregion

        #region CRUD
        public DataSet getOssDia(string data)
        {
            DataSet dt;
            ClsBdOra banco = new ClsBdOra();
            try
            {
                banco.openConn();
                dt = banco.GETSql(this.getSQLOssDia(data));
                return dt;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
