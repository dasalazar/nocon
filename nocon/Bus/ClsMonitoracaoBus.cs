using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using nocon.Dao;
using nocon.Data;

namespace nocon.Bus
{
    public class ClsMonitoracaoBus
    {
        public bool setNewMonitoracao(ClsMonitoracao monitoracao)
        {
            ClsMonitoracaoDao md = new ClsMonitoracaoDao();
            try
            {
                return md.setNewMonitoracao(monitoracao);
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }

        public List<ClsMonitoracao> getMonitoracoesEmailAlerta()
        {
            DataSet dt = new DataSet();
            List<ClsMonitoracao> lst;
            ClsMonitoracao mon;

            ClsMonitoracaoDao md = new ClsMonitoracaoDao();
            try
            {
                lst = new List<ClsMonitoracao>();
                dt = md.getMonirotacoesEmailAlerta();

                if (dt != null)
                {
                    if (dt.Tables.Count > 0)
                    {
                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Tables[0].Rows)
                            {
                                mon = new ClsMonitoracao();

                                mon.IdMonitoracao = long.Parse(row["ID_MONITORACAO"].ToString());
                                mon.DescCliente = row["CLIENTE"].ToString();
                                mon.DescAlerta = row["ALERTA"].ToString();
                                mon.DescLocal = row["LOCAL"].ToString();
                                mon.DescEquipamento = row["NOME_EQUIPAMENTO"].ToString();
                                mon.DataHoraAlerta = DateTime.Parse(row["DATA_HORA_ALERTA"].ToString());
                                if (row["DATA_HORA_RESOLVIDO"].ToString().Trim() != "")
                                    mon.DataHoraResolvido = DateTime.Parse(row["DATA_HORA_RESOLVIDO"].ToString());
                                mon.DescricaoAlerta = row["DESC_ALERTA"].ToString();
                                lst.Add(mon);
                            }
                        }
                    }
                }

                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }
        }

        public bool setEmailEnviadoMonitoracao(long idMonitoracao)
        {
            ClsMonitoracaoDao md = new ClsMonitoracaoDao();
            try
            {
                return md.setEmailEnviadoMonitoracao(idMonitoracao);

            }
            catch (Exception ex)
            {
                return false;

            }
        }

        public List<ClsMonitoracao> getMonitoracaoAceitador()
        {
            ClsMonitoracaoDao md = new ClsMonitoracaoDao();
            ClsEquipamentoBus eb = new ClsEquipamentoBus();
            List<ClsEquipamento> eqps = new List<ClsEquipamento>();
            List<ClsMonitoracao> listMonitoracoes = new List<ClsMonitoracao>();
            ClsMonitoracao monit;
            DataSet dt;
            bool erroEncontrado;

            try
            {
                eqps = eb.getEquipamentos();
                foreach (ClsEquipamento e in eqps)
                {
                    dt = new DataSet();
                    dt = md.getMonitoracaoAceitador(e.IdEqp);
                    if (dt != null)
                    {
                        if (dt.Tables.Count > 0)
                        {
                            if (dt.Tables[0].Rows.Count > 0)
                            {
                                monit = new ClsMonitoracao();
                                monit.DataHoraAlerta = DateTime.Now;
                                monit.IdEquipamento = e.IdEqp;
                                monit.IdAlerta = 5;
                                monit.DescAlerta = string.Empty;
                                erroEncontrado = false;
                                foreach (DataRow row in dt.Tables[0].Rows)
                                {
                                    if (int.Parse(row["CAM_ID"].ToString().Trim()) == 1268)
                                    {
                                        if (row["CAV_DESC"] != string.Empty && row["CAV_DESC"] != null)
                                            if ((row["CAV_DESC"].ToString().Trim()) == "Alto")
                                            {
                                                monit.DescricaoAlerta += "Nível de Cédulas: Alto ";
                                            }
                                    }
                                    else if (int.Parse(row["CAM_ID"].ToString().Trim()) == 1269)
                                    {
                                        if (row["CAV_DESC"] != string.Empty && row["CAV_DESC"] != null)
                                            if ((row["CAV_DESC"].ToString().Trim()) != "Ok")
                                            {
                                                erroEncontrado = true;
                                                monit.DescricaoAlerta += "Status Cassete: " + row["VEA_DATA"].ToString().Trim();
                                            }
                                    }
                                    else if (int.Parse(row["CAM_ID"].ToString().Trim()) == 1270)
                                    {
                                        if (row["CAV_DESC"] != string.Empty && row["CAV_DESC"] != null)
                                            if ((row["CAV_DESC"].ToString().Trim()) != "Ok")
                                            {
                                                erroEncontrado = true;
                                                monit.DescricaoAlerta += "Status Aceitador: " + row["VEA_DATA"].ToString().Trim();
                                            }
                                    }
                                    else if (int.Parse(row["CAM_ID"].ToString().Trim()) == 1271)
                                    {
                                        if (erroEncontrado)
                                        {
                                            monit.DescricaoAlerta += "Erro Aceitador: " + row["VEA_DATA"].ToString().Trim();
                                        }
                                    }

                                }
                                if (monit.DescricaoAlerta != string.Empty && monit.DescricaoAlerta != null)
                                    if (!this.getMonitoracaoExistenteENaoResolvida(e.IdEqp, 5))
                                        listMonitoracoes.Add(monit);
                            }
                        }
                    }
                }

                foreach (ClsMonitoracao mon in listMonitoracoes)
                {
                    this.setNewMonitoracao(mon);
                }
                return listMonitoracoes;
            }
            catch
            {
                return null;
            }
        }

        public List<ClsMonitoracao> getMonitoracaoAceitadorRejected()
        {
            ClsMonitoracaoDao md = new ClsMonitoracaoDao();
            ClsEquipamentoBus eb = new ClsEquipamentoBus();
            List<ClsEquipamento> eqps = new List<ClsEquipamento>();
            List<ClsMonitoracao> listMonitoracoes = new List<ClsMonitoracao>();
            ClsMonitoracao monit;
            DataSet dt;
            bool erroEncontrado;

            try
            {
                eqps = eb.getEquipamentos();
                foreach (ClsEquipamento e in eqps)
                {
                    dt = new DataSet();
                    dt = md.getMonitoracaoAceitador(e.IdEqp);
                    if (dt != null)
                    {
                        if (dt.Tables.Count > 0)
                        {
                            if (dt.Tables[0].Rows.Count > 0)
                            {
                                monit = new ClsMonitoracao();
                                monit.DataHoraAlerta = DateTime.Now;
                                monit.IdEquipamento = e.IdEqp;
                                monit.IdAlerta = 5;
                                monit.DescAlerta = string.Empty;
                                erroEncontrado = false;
                                foreach (DataRow row in dt.Tables[0].Rows)
                                {
                                    if (int.Parse(row["CAM_ID"].ToString().Trim()) == 1271)
                                    {
                                        if (row["VEA_DATA"].ToString().Trim() != string.Empty)
                                        {
                                            if (row["VEA_DATA"].ToString().Trim().Equals("REJECTED") || row["VEA_DATA"].ToString().Trim().Equals("16 - ERRO CEDULAS REJEITADAS"))
                                            {
                                                monit.DescricaoAlerta = "REJECTED";
                                                listMonitoracoes.Add(monit);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }

                foreach (ClsMonitoracao mon in listMonitoracoes)
                {
                    this.setCorrecaoRejected(mon.IdEquipamento);
                }
                return listMonitoracoes;
            }
            catch
            {
                return null;
            }
        }

        private bool getMonitoracaoExistenteENaoResolvida(long idEqp, int idTipoAlerta)
        {
            ClsMonitoracaoDao mb = new ClsMonitoracaoDao();
            DataSet dt = mb.getMonitoracaoExistenteENaoResolvida(idEqp, idTipoAlerta);
            if (dt != null)
            {
                if (dt.Tables.Count > 0)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public List<ClsMonitoracao> getMonitoracaoImpressora()
        {
            ClsMonitoracaoDao md = new ClsMonitoracaoDao();
            ClsEquipamentoBus eb = new ClsEquipamentoBus();
            List<ClsEquipamento> eqps = new List<ClsEquipamento>();
            List<ClsMonitoracao> listMonitoracoes = new List<ClsMonitoracao>();
            ClsMonitoracao monit;
            DataSet dt;
            bool erroEncontrado;

            try
            {
                eqps = eb.getEquipamentos();
                foreach (ClsEquipamento e in eqps)
                {
                    dt = new DataSet();
                    dt = md.getMonitoracaoImpressora(e.IdEqp);
                    if (dt != null)
                    {
                        if (dt.Tables.Count > 0)
                        {
                            if (dt.Tables[0].Rows.Count > 0)
                            {
                                monit = new ClsMonitoracao();
                                monit.DataHoraAlerta = DateTime.Now;
                                monit.IdEquipamento = e.IdEqp;
                                monit.IdAlerta = 4;
                                monit.DescAlerta = string.Empty;
                                erroEncontrado = false;
                                foreach (DataRow row in dt.Tables[0].Rows)
                                {
                                    if (int.Parse(row["CAM_ID"].ToString().Trim()) == 1234)
                                    {
                                        if (row["CAV_DESC"] != string.Empty && row["CAV_DESC"] != null)
                                            if ((row["CAV_DESC"].ToString().Trim()) != "COM Papel")
                                            {
                                                monit.DescricaoAlerta += "Nível de papel: " + row["CAV_DESC"].ToString().Trim();
                                            }
                                    }
                                    else if (int.Parse(row["CAM_ID"].ToString().Trim()) == 1235)
                                    {
                                        if (row["CAV_DESC"] != string.Empty && row["CAV_DESC"] != null)
                                            if ((row["CAV_DESC"].ToString().Trim()) != "Ok")
                                            {
                                                erroEncontrado = true;
                                                monit.DescricaoAlerta += "Status Impressora: " + row["VEA_DATA"].ToString().Trim();
                                            }
                                    }
                                    else if (int.Parse(row["CAM_ID"].ToString().Trim()) == 1236)
                                    {
                                        if (erroEncontrado)
                                        {
                                            monit.DescricaoAlerta += "Erro Impressora: " + row["VEA_DATA"].ToString().Trim();
                                        }
                                    }

                                }
                                if (monit.DescricaoAlerta != string.Empty && monit.DescricaoAlerta != null)
                                    if (!this.getMonitoracaoExistenteENaoResolvida(e.IdEqp, 4))
                                        listMonitoracoes.Add(monit);
                            }
                        }
                    }
                }

                foreach (ClsMonitoracao mon in listMonitoracoes)
                {
                    this.setNewMonitoracao(mon);
                }
                return listMonitoracoes;
            }
            catch
            {
                return null;
            }
        }

        public  List<ClsMonitoracao> getMonitoracaoLeitorCartoes()
        {
            ClsMonitoracaoDao md = new ClsMonitoracaoDao();
            ClsEquipamentoBus eb = new ClsEquipamentoBus();
            List<ClsEquipamento> eqps = new List<ClsEquipamento>();
            List<ClsMonitoracao> listMonitoracoes = new List<ClsMonitoracao>();
            ClsMonitoracao monit;
            DataSet dt;
            bool erroEncontrado;

            try
            {
                eqps = eb.getEquipamentos();
                foreach (ClsEquipamento e in eqps)
                {
                    dt = new DataSet();
                    dt = md.getMonitoracaoLeitorCartoes(e.IdEqp);
                    if (dt != null)
                    {
                        if (dt.Tables.Count > 0)
                        {
                            if (dt.Tables[0].Rows.Count > 0)
                            {
                                monit = new ClsMonitoracao();
                                monit.DataHoraAlerta = DateTime.Now;
                                monit.IdEquipamento = e.IdEqp;
                                monit.IdAlerta = 7;
                                monit.DescAlerta = string.Empty;
                                erroEncontrado = false;
                                foreach (DataRow row in dt.Tables[0].Rows)
                                {
                                    if (int.Parse(row["CAM_ID"].ToString().Trim()) == 1338)
                                    {
                                        if (row["CAV_DESC"] != string.Empty && row["CAV_DESC"] != null)
                                            if ((row["CAV_DESC"].ToString().Trim()) != "Ok")
                                            {
                                                erroEncontrado = true;
                                                monit.DescricaoAlerta += "Status Impressora: " + row["VEA_DATA"].ToString().Trim();
                                            }
                                    }
                                    else if (int.Parse(row["CAM_ID"].ToString().Trim()) == 1339)
                                    {
                                        if (erroEncontrado)
                                        {
                                            monit.DescricaoAlerta += "Erro Impressora: " + row["VEA_DATA"].ToString().Trim();
                                        }
                                    }

                                }
                                if (monit.DescricaoAlerta != string.Empty && monit.DescricaoAlerta != null)
                                    if (!this.getMonitoracaoExistenteENaoResolvida(e.IdEqp, 7))
                                        listMonitoracoes.Add(monit);
                            }
                        }
                    }
                }

                foreach (ClsMonitoracao mon in listMonitoracoes)
                {
                    this.setNewMonitoracao(mon);
                }
                return listMonitoracoes;
            }
            catch
            {
                return null;
            }
        }

        private bool setCorrecaoRejected(long idEqp)
        {
            ClsMonitoracaoDao md = new ClsMonitoracaoDao();
            try
            {

                return md.setCorrecaoRejected(idEqp);
            }
            catch
            {
                return false;
            }
        }


    }
}
