using System;
using System.Collections.Generic;
using System.Text;
using nocon.Data;
using System.Net.Mail;
using nocon.Dao;
using System.Data;

namespace nocon.Bus
{
    public class ClsEmailBus
    {
        private bool sendEmail(ClsEmail email)
        {

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("zimbra.perto.com.br");
                SmtpServer.Port = 587;

                SmtpServer.Credentials = new System.Net.NetworkCredential("suporte.sw", "S.p2015!");
                SmtpServer.EnableSsl = false;
                mail.From = new MailAddress("suporte.sw@perto.com.br");

                foreach (string para in email.To)
                {
                    mail.To.Add(para);
                }
                mail.Subject = email.Subjetc;
                mail.IsBodyHtml = true;
                mail.Body = email.Body;
                SmtpServer.Send(mail);
                this.setEmailEnviadoMonitoracao(email.Monitoracao.IdMonitoracao);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private List<ClsEmail> getEmailMonitoracaoAlerta()
        {
            ClsEmail e;
            List<ClsEmail> emails;

            ClsMonitoracaoBus mb = new ClsMonitoracaoBus();
            List<ClsMonitoracao> lstMonitoracao = new List<ClsMonitoracao>();

            lstMonitoracao = mb.getMonitoracoesEmailAlerta();

            try
            {
                if (lstMonitoracao != null)
                    if (lstMonitoracao.Count > 0)
                    {
                        emails = new List<ClsEmail>();
                        foreach (ClsMonitoracao m in lstMonitoracao)
                        {

                            e = new ClsEmail();
                            e.Monitoracao = m;
                            e.Subjetc = m.DescCliente + " | " + m.DescAlerta + " | " + m.DescLocal;
                            e.Body = m.DescCliente + " | " + m.DescAlerta + " | " + m.DescLocal + Environment.NewLine;
                            e.Body += "Data: " + m.DataHoraAlerta.ToString() + Environment.NewLine;
                            e.Body += "Local: " + m.DescLocal + Environment.NewLine;
                            e.Body += "Equipamento: " + m.DescEquipamento + Environment.NewLine;
                            e.Body += "Descrição: " + m.DescricaoAlerta + Environment.NewLine;
                            e.To = this.getEnderecosEmails(m.IdMonitoracao);

                            emails.Add(e);
                        }
                        return emails;
                    }
                    else
                    {
                        return null;
                    }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private List<string> getEnderecosEmails(long idMonitoracao)
        {
            List<string> endEmails = new List<string>();
            DataSet dt = new DataSet();
            try
            {
                ClsEmailDao ed = new ClsEmailDao();
                dt = ed.getEmailsLocal(idMonitoracao);

                if (dt != null)
                {
                    if (dt.Tables.Count > 0)
                    {
                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Tables[0].Rows)
                            {
                                endEmails.Add(row["EMAIL"].ToString());
                            }
                        }
                    }
                
                }

                return endEmails;
            }
            catch (Exception ex)
            {
                return endEmails;
            }
            finally
            {

            }
        }

        private bool setEmailEnviadoMonitoracao(long idMonitoracao)
        {
            ClsMonitoracaoBus mb = new ClsMonitoracaoBus();
            try
            {
                return mb.setEmailEnviadoMonitoracao(idMonitoracao);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int sendEmails()
        {
            int ret = 0;
            try
            {
                List<ClsEmail> lstEmails = this.getEmailMonitoracaoAlerta();
                foreach (ClsEmail e in lstEmails)
                {
                    this.sendEmail(e);
                    ret++;
                }
                return ret;
            }
            catch
            {
                return ret;
            }

        }
    }
}
