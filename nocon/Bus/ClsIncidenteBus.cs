using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using nocon.Data;
using nocon.Dao;

namespace nocon.Bus
{
    public class ClsIncidenteBus
    {
        public List<ClsIncidente> getReembolsosPendentesEN()
        {
            List<ClsIncidente> lstIncs = new List<ClsIncidente>();
            ClsIncidenteDao incd = new ClsIncidenteDao();
            DataSet dt = incd.getReembolsosPendentesEN();
            ClsIncidente inc;
            try
            {
                if (dt != null)
                {
                    if (dt.Tables.Count > 0)
                    {
                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Tables[0].Rows)
                            {
                                inc = new ClsIncidente();
                                inc.IncCliId = int.Parse(row["CLI_ID"].ToString());
                                inc.IncId = long.Parse(row["INC_ID"].ToString());
                                inc.IncDtAbert = DateTime.Parse(row["INC_DT_ABERT"].ToString());
                                inc.IncCartao = long.Parse(row["INC_CARTAO"].ToString());
                                inc.IncStatus = row["INC_STATUS"].ToString();
                                inc.IncNsuOrigem = row["INC_NSU_ORIGEM"].ToString();
                                inc.IncNsuResolucao = row["INC_NSU_RESOLUCAO"].ToString();
                                inc.IncValor = decimal.Parse(row["INC_VALOR"].ToString());
                                lstIncs.Add(inc);
                            }
                        }
                    }
                }
                return lstIncs;
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }
        }

        public bool setReembolsoStatusAP(ClsIncidente incidente) //Chamada pels APP
        {
            ClsIncidenteDao incd = new ClsIncidenteDao();
            ClsIncidente inc = incidente;

            if (this.verifyUpdateStatus(inc))
            {
                return incd.setReembolsoStatusAP(inc);
            }
            else
            {
                return false;
            }
        }

        public List<ClsIncidente> setReembolsoStatusAP() //Chamada pels APP
        {
            ClsIncidenteDao incd = new ClsIncidenteDao();
            List<ClsIncidente> incs = this.getReembolsosPendentesEN();
            List<ClsIncidente> incsAlterados = new List<ClsIncidente>();

            foreach (ClsIncidente inc in incs)
            {
                if (this.verifyUpdateStatus(inc))
                {
                    if (incd.setReembolsoStatusAP(inc))
                        incsAlterados.Add(inc);
                }
            }

            return incsAlterados;
        }

        /// <summary>
        /// Verifica se o incidente já foi criado a mais de 2 minutos para não correr o risco de alterar um incidente alterar o status de um incidente que está sendo resolvido
        /// </summary>
        /// <param name="inc"></param>
        /// <returns></returns>
        private bool verifyUpdateStatus(ClsIncidente inc)
        {
            DateTime dt = DateTime.Now;
            inc.IncDtAbert = inc.IncDtAbert.AddMinutes(2); //Soma dois minutos na data de abertura do incidente

            if (inc.IncDtAbert.ToString("HH:mm") != dt.ToString("HH:mm")) //Verifica se o incidente foi a menos de dois minutos
                return true;
            else
                return false;
        }


        /// <summary>
        /// Verifica novos incidentes no ORACLE e cadastra no SQL SERVER, tabela TAB_MONITORACAO
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<ClsIncidente> getInsertNewsIncidentesForMonitoracao()
        {
            List<ClsIncidente> lstIncs = new List<ClsIncidente>();
            ClsIncidenteDao incd = new ClsIncidenteDao();
            DataSet dt = incd.getReembolsosNovos();
            ClsIncidente inc;

            if (dt != null)
            {
                if (dt.Tables.Count > 0)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Tables[0].Rows)
                        {
                            inc = new ClsIncidente();
                            inc.IncCliId = int.Parse(row["CLI_ID"].ToString());
                            inc.IncId = long.Parse(row["INC_ID"].ToString());
                            inc.IncEquipamento = long.Parse(row["EQP_ID"].ToString());
                            inc.IncDtAbert = DateTime.Parse(row["INC_DT_ABERT"].ToString());
                            inc.IncCartao = long.Parse(row["INC_CARTAO"].ToString());
                            inc.IncStatus = row["INC_STATUS"].ToString();
                            inc.IncNsuOrigem = row["INC_NSU_ORIGEM"].ToString();
                            inc.IncNsuResolucao = row["INC_NSU_RESOLUCAO"].ToString();
                            inc.IncValor = decimal.Parse(row["INC_VALOR"].ToString());
                            inc.IncMotivo = row["INC_MOTIVO"].ToString();

                            lstIncs.Add(inc);
                        }
                    }

                }
            }

            ClsParametroBus pb = new ClsParametroBus();
            if (lstIncs.Count > 0)
            {
                pb.setValorParam("COD_MAX_REEMBOLSO_RIOCARD", lstIncs[lstIncs.Count - 1].IncId.ToString());
                insertNewsIncidentesMonitoracao(lstIncs);
            }

            return lstIncs;
        }

        private bool insertNewsIncidentesMonitoracao(List<ClsIncidente> listIncidente)
        {
            ClsMonitoracao monitoracao;
            List<ClsMonitoracao> listMonitoracao = new List<ClsMonitoracao>();
            ClsMonitoracaoBus mb;

            try
            {

                foreach (ClsIncidente i in listIncidente)
                {
                    monitoracao = new ClsMonitoracao();
                    monitoracao.IdEquipamento = i.IncEquipamento;
                    monitoracao.DataHoraAlerta = DateTime.Now;
                    monitoracao.IdAlerta = 8;
                    monitoracao.DescricaoAlerta = "Data: " + i.IncDtAbert.ToString() + Environment.NewLine;
                    monitoracao.DescricaoAlerta = "Cartão Chip: " + i.IncCartao.ToString() + Environment.NewLine;
                    monitoracao.DescricaoAlerta += "NSU Origem: " + i.IncNsuOrigem + Environment.NewLine;
                    monitoracao.DescricaoAlerta += "Status: " + i.IncStatus + Environment.NewLine;
                    monitoracao.DescricaoAlerta += "Valor: R$ " + i.IncValor.ToString("##0.00") + Environment.NewLine;
                    monitoracao.DescricaoAlerta += "Motivo: " + i.IncMotivo + Environment.NewLine;

                    mb = new ClsMonitoracaoBus();
                    mb.setNewMonitoracao(monitoracao);

                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }
    }
}
