using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using nocon.Bus;

namespace nocon.Dao
{
    public class ClsEmailDao
    {
        #region SQLs
        private string getSQLEmailsLocal(long idMonitoracao, bool enviaParaCliente)
        {
            string strSql = "";
            if (enviaParaCliente)
            {
                strSql = "SELECT * ";
                strSql += " FROM TAB_LOCAL_EMAIL ";
                strSql += " WHERE ID_LOCAL IN(1000,2000) ";
                strSql += " UNION ";
                strSql += " SELECT * ";
                strSql += " FROM TAB_LOCAL_EMAIL ";
                strSql += " WHERE ID_LOCAL = ";
                strSql += " ( ";
                strSql += " SELECT ID_LOCAL ";
                strSql += " FROM TAB_EQUIPAMENTO  ";
                strSql += " WHERE ID_EQUIPAMENTO = (SELECT ID_EQUIPAMENTO FROM TAB_MONITORACAO WHERE ID_MONITORACAO = " + idMonitoracao + ") ";
                strSql += " )";
            }
            else
            {
                strSql = " SELECT * ";
                strSql += " FROM TAB_LOCAL_EMAIL ";
                strSql += " WHERE ID_LOCAL IN(1000)";
            }

            return strSql;
        }

        private string getSQLEnviaEmailCliente(long idMonitoracao)
        {
            string strSql = "SELECT IND_ENVIO_EMAIL_CLIENTE ";
            strSql += "FROM TAB_MONITORACAO INNER JOIN TAB_TIPO_ALERTA ";
            strSql += "ON TAB_MONITORACAO.ID_ALERTA = TAB_TIPO_ALERTA.ID_ALERTA ";
            strSql += "WHERE ID_MONITORACAO = " + idMonitoracao.ToString();
            return strSql;
        }
        #endregion

        #region CRUD
        public DataSet getEmailsLocal(long idMonitoracao)
        {
            DataSet dt;
            ClsBdSql banco = new ClsBdSql();
            try
            {
                banco.openConn();
                bool booIndEnviaEmailCliente;
                int indAtivoEnviaCliente = (int)banco.GETSqlScalar(this.getSQLEnviaEmailCliente(idMonitoracao));
                if (indAtivoEnviaCliente == 1)
                    booIndEnviaEmailCliente = true;
                else
                    booIndEnviaEmailCliente = false;

                dt = banco.GETSql(this.getSQLEmailsLocal(idMonitoracao, booIndEnviaEmailCliente));
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool enviaEmailCliente(long idMonitoracao)
        {
            return false;
        }
        #endregion
    }
}
