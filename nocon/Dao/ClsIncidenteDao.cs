using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using nocon.Bus;
using nocon.Data;

namespace nocon.Dao
{
    public class ClsIncidenteDao
    {
        #region SQLs
        /// <summary>
        /// Método que retorna o SQL para consulta de incidentes no ORACLE que iniciaram resolução e não terminaram, status "EN"
        /// </summary>
        /// <returns>String SQL</returns>
        private string getSQL_GETReembolsosPendentesEN()
        {

            string strSql = "SELECT ";
            strSql += "INC_ID, ";
            strSql += "CLI_ID, ";
            strSql += "TO_CHAR(INC_DT_ABERT, 'DD/MM/YYYY HH24:MI:SS') AS INC_DT_ABERT, ";
            strSql += "INC_CARTAO, ";
            strSql += "INC_STATUS, ";
            strSql += "INC_VALOR, ";
            strSql += "INC_NSU_ORIGEM, ";
            strSql += "INC_NSU_RESOLUCAO ";
            strSql += "FROM APPPORTAL.portal_incidente WHERE INC_STATUS = 'EN' AND CLI_ID = 11 ORDER BY INC_DT_ABERT DESC ";
            return strSql;
        }

        /// <summary>
        /// Método que retorna o SQL para consulta de incidentes no ORACLE que iniciaram resolução e não terminaram, status "EN"
        /// </summary>
        /// <returns>String SQL</returns>
        private string getSQL_SETReembolsosAP(ClsIncidente incidente)
        {
            ClsIncidente inc = new ClsIncidente();
            inc = incidente;
            string strSql = "UPDATE APPPORTAL.portal_incidente ";
            strSql += "SET INC_STATUS = 'AP' ";
            strSql += "WHERE INC_STATUS = 'EN' AND CLI_ID = 11 ";
            strSql += "AND INC_CARTAO = " + inc.IncCartao;
            return strSql;
        }

        /// <summary>
        /// Método que retorna o SQL para consulta de novos incidentes no ORACLE status "AP" cliente RioCard
        /// </summary>
        /// <returns>String SQL</returns>
        private string getSQL_GETReembolsosNovos(long reembolsoId)
        {
            string strSql = "SELECT * FROM APPPORTAL.portal_incidente WHERE CLI_ID = 11 AND INC_STATUS = 'AP' AND INC_ID > " + reembolsoId + " ORDER BY INC_ID ASC";
            return strSql;
        }
        #endregion

        #region CRUD

        /// <summary>
        /// Método que retorna os incidentes pendentes, status "EN"
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet getReembolsosPendentesEN()
        {
            DataSet dt;
            ClsBdOra banco = new ClsBdOra();
            try
            {
                banco.openConn();
                dt = banco.GETSql(this.getSQL_GETReembolsosPendentesEN());
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Método que altera o incidente pendente, status "EN" -> status "AP"
        /// </summary>
        /// <returns>Boolean</returns>
        public bool setReembolsoStatusAP(ClsIncidente incidente)
        {
            ClsBdOra banco = new ClsBdOra();
            ClsIncidente inc = new ClsIncidente();
            inc = incidente;
            try
            {
                banco.openConn();
                bool exec = banco.SETSql(this.getSQL_SETReembolsosAP(inc));
                return exec;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Método incidentes novos status "AP" do cliente RioCard
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet getReembolsosNovos()
        {
            DataSet dt;
            ClsBdOra banco = new ClsBdOra();
            ClsParametroBus pb = new ClsParametroBus();

            try
            {
                ClsParametro param = pb.getValorParam("COD_MAX_REEMBOLSO_RIOCARD");

                banco.openConn();
                dt = banco.GETSql(this.getSQL_GETReembolsosNovos(int.Parse(param.Parametro)));
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
