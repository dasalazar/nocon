using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using nocon.Bus;
using nocon.Data;

namespace nocon.Dao
{
    public class ClsMonitoracaoDao
    {
        #region SQLs
        /// <summary>
        /// Método que retorna o SQL para INSERT de alertas no SQL Server
        /// </summary>
        /// <returns>String SQL</returns>
        private string getSQL_SETNewMonitoracao(ClsMonitoracao monitoracao)
        {

            string strSql = "INSERT INTO dbo.TAB_MONITORACAO ";
            strSql += "(ID_EQUIPAMENTO, ";
            strSql += "ID_ALERTA, ";
            strSql += "DATA_HORA_ALERTA, ";
            strSql += "IND_RESOLVIDO, ";
            strSql += "DESC_ALERTA, ";
            strSql += "IND_EMAIL_ENVIADO) ";
            strSql += "VALUES( ";
            strSql += monitoracao.IdEquipamento.ToString() + ", ";
            strSql += monitoracao.IdAlerta.ToString() + ", ";
            strSql += "'" + monitoracao.DataHoraAlerta.ToString("yyyy-MM-dd hh:mm:ss") + "', ";
            strSql += "0, ";
            strSql += " '" + monitoracao.DescricaoAlerta.ToString() + "', ";
            strSql += "0 ";
            strSql += ")";

            return strSql;
        }

        private string getSQL_GETMonirotacoesEmailAlerta()
        {
            string strSql = "SELECT ";
            strSql += "TAB_MONITORACAO.ID_MONITORACAO, ";
            strSql += "(SELECT TAB_CLIENTE.NOME_CLIENTE FROM TAB_CLIENTE WHERE TAB_CLIENTE.ID_CLIENTE = TAB_EQUIPAMENTO.ID_CLIENTE) AS CLIENTE, ";
            strSql += "(SELECT TAB_TIPO_ALERTA.DESC_ALERTA FROM TAB_TIPO_ALERTA WHERE TAB_TIPO_ALERTA.ID_ALERTA = TAB_MONITORACAO.ID_ALERTA ) AS ALERTA, ";
            strSql += "(SELECT TAB_LOCAL.NOME_LOCAL FROM TAB_LOCAL WHERE TAB_LOCAL.ID_LOCAL = TAB_EQUIPAMENTO.ID_LOCAL) AS LOCAL, ";
            strSql += "TAB_EQUIPAMENTO.NOME_EQUIPAMENTO, ";
            strSql += "TAB_MONITORACAO.DATA_HORA_ALERTA, ";
            strSql += "TAB_MONITORACAO.DATA_HORA_RESOLVIDO, ";
            strSql += "TAB_MONITORACAO.DESC_ALERTA ";
            strSql += "FROM TAB_MONITORACAO INNER JOIN TAB_EQUIPAMENTO ";
            strSql += "ON TAB_MONITORACAO.ID_EQUIPAMENTO = TAB_EQUIPAMENTO.ID_EQUIPAMENTO ";
            strSql += "WHERE TAB_MONITORACAO.IND_EMAIL_ENVIADO = 0 ";
            return strSql;
        }

        private string getSQL_SETEmailEnviadoMonitoracao(long idMonitoracao)
        {
            string strSql = "UPDATE TAB_MONITORACAO ";
            strSql += "SET IND_EMAIL_ENVIADO = 1 ";
            strSql += "WHERE ID_MONITORACAO = " + idMonitoracao.ToString();
            return strSql;
        }

        private string getSQL_getMonitoracaoExistenteENaoResolvida(long idEqp, int idTipoAlerta)
        {
            string strSql = "SELECT * FROM TAB_MONITORACAO ";
            strSql += " WHERE ID_EQUIPAMENTO = " + idEqp.ToString();
            strSql += " AND ID_ALERTA = " + idTipoAlerta.ToString();
            strSql += " AND IND_RESOLVIDO = 0 ";
            return strSql;
        }

        private string getSQL_MonitoracaoAceitador(long idEqp)
        {
            string strSql = "select MM.MOD_DESC, ";
            strSql += "c.cam_id, ";
            strSql += "c.cam_desc, ";
            strSql += "a.cav_id, ";
            strSql += "cv.cav_desc, ";
            strSql += "a.vea_data, ";
            strSql += "a.vea_dthrcad ";
            strSql += "from appmonitor.monitor_campo c inner join  appmonitor.monitor_mapvetstat_campo t ";
            strSql += "on t.cam_id = c.cam_id left join appmonitor.monitor_vetor_status_atual a ";
            strSql += "on a.cam_id = c.cam_id and a.eqp_id = " + idEqp.ToString();
            strSql += "inner join appmonitor.monitor_campovalor cv ";
            strSql += "on cv.cav_id = a.cav_id and cv.cam_id = a.cam_id ";
            strSql += "INNER JOIN APPMONITOR.monitor_modulo MM ";
            strSql += "ON c.MOD_ID = MM.MOD_ID ";
            strSql += "where ";
            strSql += "t.mvs_id in (187, 188) ";
            strSql += "and c.cam_id in(1268,1269,1270,1271) ";
            return strSql;
        }

        private string getSQL_MonitoracaoImpressora(long idEqp)
        {
            string strSql = "select MM.MOD_DESC, ";
            strSql += "c.cam_id, ";
            strSql += "c.cam_desc, ";
            strSql += "a.cav_id, ";
            strSql += "cv.cav_desc, ";
            strSql += "a.vea_data, ";
            strSql += "a.vea_dthrcad ";
            strSql += "from appmonitor.monitor_campo c inner join  appmonitor.monitor_mapvetstat_campo t ";
            strSql += "on t.cam_id = c.cam_id left join appmonitor.monitor_vetor_status_atual a ";
            strSql += "on a.cam_id = c.cam_id and a.eqp_id = " + idEqp.ToString();
            strSql += "inner join appmonitor.monitor_campovalor cv ";
            strSql += "on cv.cav_id = a.cav_id and cv.cam_id = a.cam_id ";
            strSql += "INNER JOIN APPMONITOR.monitor_modulo MM ";
            strSql += "ON c.MOD_ID = MM.MOD_ID ";
            strSql += "where ";
            strSql += "t.mvs_id in (187, 188) ";
            strSql += "and c.cam_id in(1235,1236,1234) ";
            return strSql;
        }

        private string getSQL_MonitoracaoLeitorCartoes(long idEqp)
        {
            string strSql = "select MM.MOD_DESC, ";
            strSql += "c.cam_id, ";
            strSql += "c.cam_desc, ";
            strSql += "a.cav_id, ";
            strSql += "cv.cav_desc, ";
            strSql += "a.vea_data, ";
            strSql += "a.vea_dthrcad ";
            strSql += "from appmonitor.monitor_campo c inner join  appmonitor.monitor_mapvetstat_campo t ";
            strSql += "on t.cam_id = c.cam_id left join appmonitor.monitor_vetor_status_atual a ";
            strSql += "on a.cam_id = c.cam_id and a.eqp_id = " + idEqp.ToString();
            strSql += "inner join appmonitor.monitor_campovalor cv ";
            strSql += "on cv.cav_id = a.cav_id and cv.cam_id = a.cam_id ";
            strSql += "INNER JOIN APPMONITOR.monitor_modulo MM ";
            strSql += "ON c.MOD_ID = MM.MOD_ID ";
            strSql += "where ";
            strSql += "t.mvs_id in (187, 188) ";
            strSql += "and c.cam_id in(1238,1339) ";
            return strSql;
        }

        private string getSQL_setCorrecaoRejected(long idEqp)
        {
            string strSql = "INSERT INTO APPMONITOR.MONITOR_COMANDOREMOTO ";
            strSql += " SELECT APPMONITOR.SEQ_MON_COMANDOREMOTO_ID.NEXTVAL, ";
            strSql += " E.EQP_ID, ";
            strSql += " 5, ";
            strSql += @" 'RHKEY_LOCAL_MACHINE\SOFTWARE\PERTO S.A.\MONITORAMENTO\ACEITADORNOTAS\DEVOLVIDAS_DIA#0', ";
            strSql += " SYSDATE, ";
            strSql += " NULL, ";
            strSql += " 17, ";
            strSql += " NULL, ";
            strSql += " NULL, ";
            strSql += " NULL ";
            strSql += " FROM APPACESSO.ACESSO_EQUIPAMENTO E ";
            strSql += " WHERE E.EQP_ID = " + idEqp.ToString();
            strSql += " AND E.EQP_INDATIVO = 1 ";
            strSql += " AND E.EQP_INDBLOCK = 0 ";
            strSql += " AND E.EQP_STATUS = 4 ";
            return strSql;
        }
        #endregion

        #region CRUD

        /// <summary>
        /// Método que executa o SQL de INSERT de alertas no SQL Server
        /// </summary>
        /// <returns>Boolean</returns>
        public bool setNewMonitoracao(ClsMonitoracao monitoracao)
        {
            ClsMonitoracao monit = monitoracao;
            ClsBdSql banco = new ClsBdSql();
            try
            {
                banco.openConn();
                return banco.SETSql(this.getSQL_SETNewMonitoracao(monit));
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataSet getMonirotacoesEmailAlerta()
        {
            ClsBdSql banco = new ClsBdSql();
            try
            {
                banco.openConn();
                return banco.GETSql(this.getSQL_GETMonirotacoesEmailAlerta());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet getMonitoracaoAceitador(long idEqp)
        {
            ClsBdOra banco = new ClsBdOra();
            try
            {
                banco.openConn();
                return banco.GETSql(this.getSQL_MonitoracaoAceitador(idEqp));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool setEmailEnviadoMonitoracao(long idMonitoracao)
        {
            ClsBdSql banco = new ClsBdSql();
            try
            {
                banco.openConn();
                return banco.SETSql(this.getSQL_SETEmailEnviadoMonitoracao(idMonitoracao));
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataSet getMonitoracaoExistenteENaoResolvida(long idEqp, int idTipoAlerta)
        {
            ClsBdSql banco = new ClsBdSql();
            try
            {
                banco.openConn();
                return banco.GETSql(this.getSQL_getMonitoracaoExistenteENaoResolvida(idEqp, idTipoAlerta));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet getMonitoracaoImpressora(long idEqp)
        {
            ClsBdOra banco = new ClsBdOra();
            try
            {
                banco.openConn();
                return banco.GETSql(this.getSQL_MonitoracaoImpressora(idEqp));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet getMonitoracaoLeitorCartoes(long idEqp)
        {
            ClsBdOra banco = new ClsBdOra();
            try
            {
                banco.openConn();
                return banco.GETSql(this.getSQL_MonitoracaoLeitorCartoes(idEqp));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool setCorrecaoRejected(long idEqp)
        {
            ClsBdOra banco = new ClsBdOra();
            try
            {
                banco.openConn();
                return banco.SETSql(this.getSQL_setCorrecaoRejected(idEqp));
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        #endregion
    }
}
