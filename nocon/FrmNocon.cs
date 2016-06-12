using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using nocon.Bus;
using nocon.Data;
using System.Threading;

namespace nocon
{
    public partial class FrmNocon : Form
    {
        public FrmNocon()
        {
            InitializeComponent();
        }

        #region Variáveis e Objetos
        public int contadorReembolsosResolvidos = 0;

        public enum TipoThread
        {
            RIOCARD_RESOLVE_REEMB_EN,
            WEBSERVICES_VERIFICA,
            RIOCARD_REEMBOLSOS,
            ENVIO_EMAIL_ALERTAS,
            ERROS_ACEITADOR,
            ERROS_IMPRESSORA,
            ERROS_LEITOR_CARTOES,
            CORRECAO_ACEITADOR,
            ATM_ONLINE
        }

        #region Declaração de Threads

        Thread RIOCARD_RESOLVE_REEMB_EN_Thread = null;
        Thread WEBSERVICES_VERIFICA_Thread = null;
        Thread RIOCARD_REEMBOLSOS_Thread = null;
        Thread ENVIO_EMAIL_ALERTAS_Thread = null;
        Thread ERROS_ACEITADOR_Thread = null;
        Thread ERROS_IMPRESSORA_Thread = null;
        Thread ERROS_LEITOR_CARTOES_Thread = null;
        Thread CORRECAO_ACEITADOR_Thread = null;
        Thread ATM_ONLINE_Thread = null;

        #endregion

        #endregion

        #region Eventos Form

        private void FrmNocon_Load(object sender, EventArgs e)
        {
            #region Grid Alertas

            trataTempos(TipoThread.RIOCARD_REEMBOLSOS);
            gridAlertas.Rows.Add("RIOCARD - NOVOS REEMBOLSOS.", "TODOS", "Nova execução " + ClsTempoExecucaoBus.NOVA_EXEC_RIOCARD_REEMBOLSOS);

            trataTempos(TipoThread.ENVIO_EMAIL_ALERTAS);
            gridAlertas.Rows.Add("EMAILS - ALERTA", "TODOS", "Nova execução " + ClsTempoExecucaoBus.NOVA_EXEC_EMAILS_ALERTAS);

            trataTempos(TipoThread.ERROS_ACEITADOR);
            gridAlertas.Rows.Add("ERROS ACEITADOR", "TODOS", "Nova execução " + ClsTempoExecucaoBus.NOVA_EXEC_ERROS_ACEITADOR);

            trataTempos(TipoThread.ERROS_IMPRESSORA);
            gridAlertas.Rows.Add("ERROS IMPRESSORA", "TODOS", "Nova execução " + ClsTempoExecucaoBus.NOVA_EXEC_ERROS_IMPRESSORA);

            trataTempos(TipoThread.ERROS_LEITOR_CARTOES);
            gridAlertas.Rows.Add("ERROS LEITOR DE CARTÕES", "TODOS", "Nova execução " + ClsTempoExecucaoBus.NOVA_EXEC_ERROS_LEITOR_CARTOES);
            
            trataTempos(TipoThread.ERROS_LEITOR_CARTOES);
            gridAlertas.Rows.Add("ERROS ATM OFF", "TODOS", "Nova execução " + ClsTempoExecucaoBus.NOVA_EXEC_ATM_OFF);

            #endregion

            #region Grid Resolvidos

            trataTempos(TipoThread.RIOCARD_RESOLVE_REEMB_EN);
            gridAlertasResolvidos.Rows.Add("RIOCARD - REEMB.", "TODOS", "Nova execução " + ClsTempoExecucaoBus.NOVA_EXEC_RIOCARD_RESOLVE_REEMBOLSOS_EN);

            trataTempos(TipoThread.CORRECAO_ACEITADOR);
            gridAlertasResolvidos.Rows.Add("ACEITADOR REJECTED", "TODOS", "Nova execução " + ClsTempoExecucaoBus.NOVA_EXEC_CORRECAO_ACEITADOR);

            #endregion

            #region Grid Servidores

            trataTempos(TipoThread.WEBSERVICES_VERIFICA);
            ClsWebserviceBus wsb = new ClsWebserviceBus();
            foreach (ClsWebservice ws in wsb.getWebservices())
            {
                gridWS.Rows.Add(ws.Nome, ws.Endereco, ws.Status, ClsTempoExecucaoBus.NOVA_EXEC_WEBSERVICE_VERIFICA);
            }

            #endregion
        }

        private void timerExecucao_Tick(object sender, EventArgs e)
        {
            #region Grid Resolução

            #region Thread Resolve Reembolsos RIOCARD
            if (DateTime.Now.ToString("dd/MM/yyyy HH:mm") == ClsTempoExecucaoBus.NOVA_EXEC_RIOCARD_RESOLVE_REEMBOLSOS_EN)
            {
                if (RIOCARD_RESOLVE_REEMB_EN_Thread == null)
                {
                    ClsTempoExecucaoBus.NOVA_EXEC_RIOCARD_RESOLVE_REEMBOLSOS_EN = "";
                    int indLineGrid = this.getLineGrid(this.gridAlertasResolvidos, "RIOCARD - REEMB.");
                    gridAlertasResolvidos[2, indLineGrid].Value = "Executando " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");


                    RIOCARD_RESOLVE_REEMB_EN_Thread = new Thread(new ThreadStart(this.RIOCARD_resolveReembolsoEN));
                    RIOCARD_RESOLVE_REEMB_EN_Thread.Start();
                    Application.DoEvents();
                }
                else
                {
                    if (!RIOCARD_RESOLVE_REEMB_EN_Thread.IsAlive)
                    {
                        RIOCARD_RESOLVE_REEMB_EN_Thread = null;
                    }
                }
            }

            if (DateTime.Now.ToString("dd/MM/yyyy HH:mm") == ClsTempoExecucaoBus.NOVA_EXEC_CORRECAO_ACEITADOR)
            {
                if (CORRECAO_ACEITADOR_Thread == null)
                {
                    ClsTempoExecucaoBus.NOVA_EXEC_CORRECAO_ACEITADOR = "";
                    int indLineGrid = this.getLineGrid(this.gridAlertasResolvidos, "ACEITADOR REJECTED");
                    gridAlertasResolvidos[2, indLineGrid].Value = "Executando " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");


                    CORRECAO_ACEITADOR_Thread = new Thread(new ThreadStart(this.ACEITADOR_resolveRejected));
                    CORRECAO_ACEITADOR_Thread.Start();
                    Application.DoEvents();
                }
                else
                {
                    if (!CORRECAO_ACEITADOR_Thread.IsAlive)
                    {
                        CORRECAO_ACEITADOR_Thread = null;
                    }
                }
            }

            #endregion

            #endregion

            #region Grid Webservices

            if (DateTime.Now.ToString("dd/MM/yyyy HH:mm") == ClsTempoExecucaoBus.NOVA_EXEC_WEBSERVICE_VERIFICA)
            {
                if (WEBSERVICES_VERIFICA_Thread == null)
                {
                    ClsTempoExecucaoBus.NOVA_EXEC_WEBSERVICE_VERIFICA = "";
                    //int indLineGrid = this.getLineGrid(this.gridAlertasResolvidos, "RIOCARD - REEMB.");
                    //gridAlertasResolvidos[2, indLineGrid].Value = "Executando " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");


                    WEBSERVICES_VERIFICA_Thread = new Thread(new ThreadStart(this.WEBSERVICES_verifica));
                    WEBSERVICES_VERIFICA_Thread.Start();
                    Application.DoEvents();
                }
                else
                {
                    if (!WEBSERVICES_VERIFICA_Thread.IsAlive)
                    {
                        WEBSERVICES_VERIFICA_Thread = null;
                    }
                }
            }


            #endregion

            #region Grid Alertas

            if (DateTime.Now.ToString("dd/MM/yyyy HH:mm") == ClsTempoExecucaoBus.NOVA_EXEC_RIOCARD_REEMBOLSOS)
            {
                if (RIOCARD_REEMBOLSOS_Thread == null)
                {
                    ClsTempoExecucaoBus.NOVA_EXEC_RIOCARD_REEMBOLSOS = "";

                    RIOCARD_REEMBOLSOS_Thread = new Thread(new ThreadStart(this.RIOCARD_reembolsos));
                    RIOCARD_REEMBOLSOS_Thread.Start();
                    Application.DoEvents();
                }
                else
                {
                    if (!RIOCARD_REEMBOLSOS_Thread.IsAlive)
                    {
                        RIOCARD_REEMBOLSOS_Thread = null;
                    }
                }
            }

            if (DateTime.Now.ToString("dd/MM/yyyy HH:mm") == ClsTempoExecucaoBus.NOVA_EXEC_EMAILS_ALERTAS)
            {
                if (ENVIO_EMAIL_ALERTAS_Thread == null)
                {
                    ClsTempoExecucaoBus.NOVA_EXEC_EMAILS_ALERTAS = "";

                    ENVIO_EMAIL_ALERTAS_Thread = new Thread(new ThreadStart(this.ENVIO_alertas_email));
                    ENVIO_EMAIL_ALERTAS_Thread.Start();
                    Application.DoEvents();
                }
                else
                {
                    if (!ENVIO_EMAIL_ALERTAS_Thread.IsAlive)
                    {
                        ENVIO_EMAIL_ALERTAS_Thread = null;
                    }
                }
            }

            if (DateTime.Now.ToString("dd/MM/yyyy HH:mm") == ClsTempoExecucaoBus.NOVA_EXEC_ERROS_ACEITADOR)
            {
                if (ERROS_ACEITADOR_Thread == null)
                {
                    ClsTempoExecucaoBus.NOVA_EXEC_ERROS_ACEITADOR = "";

                    ERROS_ACEITADOR_Thread = new Thread(new ThreadStart(this.ERROS_aceitador));
                    ERROS_ACEITADOR_Thread.Start();
                    Application.DoEvents();
                }
                else
                {
                    if (!ERROS_ACEITADOR_Thread.IsAlive)
                    {
                        ERROS_ACEITADOR_Thread = null;
                    }
                }
            }

            if (DateTime.Now.ToString("dd/MM/yyyy HH:mm") == ClsTempoExecucaoBus.NOVA_EXEC_ERROS_IMPRESSORA)
            {
                if (ERROS_IMPRESSORA_Thread == null)
                {
                    ClsTempoExecucaoBus.NOVA_EXEC_ERROS_IMPRESSORA = "";

                    ERROS_IMPRESSORA_Thread = new Thread(new ThreadStart(this.ERROS_impressora));
                    ERROS_IMPRESSORA_Thread.Start();
                    Application.DoEvents();
                }
                else
                {
                    if (!ERROS_IMPRESSORA_Thread.IsAlive)
                    {
                        ERROS_IMPRESSORA_Thread = null;
                    }
                }
            }

            if (DateTime.Now.ToString("dd/MM/yyyy HH:mm") == ClsTempoExecucaoBus.NOVA_EXEC_ERROS_LEITOR_CARTOES)
            {
                if (ERROS_LEITOR_CARTOES_Thread == null)
                {
                    ClsTempoExecucaoBus.NOVA_EXEC_ERROS_LEITOR_CARTOES = "";

                    ERROS_LEITOR_CARTOES_Thread = new Thread(new ThreadStart(this.ERROS_leitor_cartoes));
                    ERROS_LEITOR_CARTOES_Thread.Start();
                    Application.DoEvents();
                }
                else
                {
                    if (!ERROS_LEITOR_CARTOES_Thread.IsAlive)
                    {
                        ERROS_LEITOR_CARTOES_Thread = null;
                    }
                }
            }


            #endregion
        }


        #endregion

        #region Métodos Invocados pelas Threads

        private void RIOCARD_resolveReembolsoEN()
        {
            int indLineGrid = 0;
            try
            {
                ClsIncidenteBus incb = new ClsIncidenteBus();
                Application.DoEvents();
                List<ClsIncidente> list = incb.setReembolsoStatusAP();
                contadorReembolsosResolvidos = contadorReembolsosResolvidos + list.Count;
                this.trataTempos(TipoThread.RIOCARD_RESOLVE_REEMB_EN);



                indLineGrid = this.getLineGrid(this.gridAlertasResolvidos, "RIOCARD - REEMB.");
                gridAlertasResolvidos[2, indLineGrid].Value = "Executado " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " " + contadorReembolsosResolvidos + " resolvidos | ";
                gridAlertasResolvidos[2, indLineGrid].Value += "Próxima execução " + ClsTempoExecucaoBus.NOVA_EXEC_RIOCARD_RESOLVE_REEMBOLSOS_EN;

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                gridAlertasResolvidos[2, indLineGrid].Value = ex.Message.ToString();
                Application.DoEvents();
            }

        }

        private void WEBSERVICES_verifica()
        {
            int indLineGrid = 0;
            try
            {
                this.trataTempos(TipoThread.WEBSERVICES_VERIFICA);
                ClsWebserviceBus wsb = new ClsWebserviceBus();
                Application.DoEvents();

                List<ClsWebservice> wss = wsb.getWebservices();

                foreach (ClsWebservice ws in wss)
                {
                    string disp = "";
                    bool ativo = wsb.getKeepALiveWS(ws);
                    indLineGrid = this.getLineGrid(this.gridWS, ws.Nome);

                    if (ativo)
                        disp = "ON";
                    else
                        disp = "OFF";

                    gridWS[2, indLineGrid].Value = disp;
                    gridWS[3, indLineGrid].Value = ClsTempoExecucaoBus.NOVA_EXEC_WEBSERVICE_VERIFICA;
                }

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                gridWS[3, indLineGrid].Value = ex.Message.ToString();
            }

        }

        public void RIOCARD_reembolsos()
        {
            int indLineGrid = 0;
            try
            {
                this.trataTempos(TipoThread.RIOCARD_REEMBOLSOS);
                ClsIncidenteBus ib = new ClsIncidenteBus();

                List<ClsIncidente> list = ib.getInsertNewsIncidentesForMonitoracao();

                int contador = 0;
                if (list != null)
                    if (list.Count > 0)
                        contador = list.Count;

                gridAlertas[2, indLineGrid].Value = "Executado " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " " + contador.ToString() + " reembolsos | ";
                gridAlertas[2, indLineGrid].Value += "Próxima execução " + ClsTempoExecucaoBus.NOVA_EXEC_RIOCARD_REEMBOLSOS;

            }
            catch (Exception ex)
            {
                gridAlertas[3, indLineGrid].Value = ex.Message.ToString();
            }
        }

        public void ENVIO_alertas_email()
        {
            int indLineGrid = 0;
            this.trataTempos(TipoThread.ENVIO_EMAIL_ALERTAS);
            ClsEmailBus eb = new ClsEmailBus();
            try
            {
                int emailsEnviados = eb.sendEmails();

                indLineGrid = this.getLineGrid(this.gridAlertas, "EMAILS - ALERTA");

                gridAlertas[2, indLineGrid].Value = "Executado " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " " + emailsEnviados + " emails | ";
                gridAlertas[2, indLineGrid].Value += "Próxima execução " + ClsTempoExecucaoBus.NOVA_EXEC_EMAILS_ALERTAS;
            }
            catch (Exception ex)
            {
                gridAlertas[2, indLineGrid].Value = ex.Message.ToString();
            }
        }

        public void ERROS_aceitador()
        {
            int indLineGrid = 0;
            this.trataTempos(TipoThread.ERROS_ACEITADOR);
            ClsMonitoracaoBus eb = new ClsMonitoracaoBus();
            List<ClsMonitoracao> list = new List<ClsMonitoracao>();
            try
            {
                list = eb.getMonitoracaoAceitador();
                indLineGrid = this.getLineGrid(this.gridAlertas, "ERROS ACEITADOR");

                gridAlertas[2, indLineGrid].Value = "Executado " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " " + list.Count + " aceitadores | ";
                gridAlertas[2, indLineGrid].Value += "Próxima execução " + ClsTempoExecucaoBus.NOVA_EXEC_ERROS_ACEITADOR;
            }
            catch (Exception ex)
            {
                gridAlertas[2, indLineGrid].Value = ex.Message.ToString();
            }

        }

        private void ERROS_impressora()
        {
            int indLineGrid = 0;
            this.trataTempos(TipoThread.ERROS_IMPRESSORA);
            ClsMonitoracaoBus mb = new ClsMonitoracaoBus();
            List<ClsMonitoracao> list = new List<ClsMonitoracao>();
            try
            {
                list = mb.getMonitoracaoImpressora();
                indLineGrid = this.getLineGrid(this.gridAlertas, "ERROS IMPRESSORA");

                gridAlertas[2, indLineGrid].Value = "Executado " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " " + list.Count + " impressoras | ";
                gridAlertas[2, indLineGrid].Value += "Próxima execução " + ClsTempoExecucaoBus.NOVA_EXEC_ERROS_IMPRESSORA;
            }
            catch (Exception ex)
            {
                gridAlertas[2, indLineGrid].Value = ex.Message.ToString();
            }
        }

        private void ERROS_leitor_cartoes()
        {
            int indLineGrid = 0;
            this.trataTempos(TipoThread.ERROS_LEITOR_CARTOES);
            ClsMonitoracaoBus mb = new ClsMonitoracaoBus();
            List<ClsMonitoracao> list = new List<ClsMonitoracao>();
            try
            {
                list = mb.getMonitoracaoLeitorCartoes();
                indLineGrid = this.getLineGrid(this.gridAlertas, "ERROS LEITOR DE CARTÕES");

                gridAlertas[2, indLineGrid].Value = "Executado " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " " + list.Count + " leitores de cartões | ";
                gridAlertas[2, indLineGrid].Value += "Próxima execução " + ClsTempoExecucaoBus.NOVA_EXEC_ERROS_LEITOR_CARTOES;
            }
            catch (Exception ex)
            {
                gridAlertas[2, indLineGrid].Value = ex.Message.ToString();
            }
        }

        private void ACEITADOR_resolveRejected()
        {
            int indLineGrid = 0;
            this.trataTempos(TipoThread.CORRECAO_ACEITADOR);
            ClsMonitoracaoBus mb = new ClsMonitoracaoBus();
            List<ClsMonitoracao> list = new List<ClsMonitoracao>();
            bool booExecutado = false;
            try
            {
                
                list = mb.getMonitoracaoAceitadorRejected();
                indLineGrid = this.getLineGrid(this.gridAlertasResolvidos, "ACEITADOR REJECTED");

                gridAlertasResolvidos[2, indLineGrid].Value = "Executado " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " " + list.Count.ToString() + " REJECTED ";
                gridAlertasResolvidos[2, indLineGrid].Value += "Próxima execução " + ClsTempoExecucaoBus.NOVA_EXEC_CORRECAO_ACEITADOR;
            }
            catch (Exception ex)
            {
                gridAlertas[2, indLineGrid].Value = ex.Message.ToString();
            }
        }

        #endregion

        #region Métodos Auxiliares

        private void trataTempos(TipoThread tipoThread)
        {
            DateTime novaData;
            switch (tipoThread)
            {
                case TipoThread.RIOCARD_RESOLVE_REEMB_EN:
                    novaData = DateTime.Now.AddMinutes(ClsTempoExecucaoBus.TEMPO_INTERVALO_RIOCARD_RESOLVEVE_REEMBOLSOS_EN);
                    ClsTempoExecucaoBus.NOVA_EXEC_RIOCARD_RESOLVE_REEMBOLSOS_EN = novaData.ToString("dd/MM/yyyy HH:mm");
                    break;
                case TipoThread.WEBSERVICES_VERIFICA:
                    novaData = DateTime.Now.AddMinutes(ClsTempoExecucaoBus.TEMPO_INTERVALO_WEBSERVICE_VERIFICA);
                    ClsTempoExecucaoBus.NOVA_EXEC_WEBSERVICE_VERIFICA = novaData.ToString("dd/MM/yyyy HH:mm");
                    Application.DoEvents();
                    break;
                case TipoThread.RIOCARD_REEMBOLSOS:
                    novaData = DateTime.Now.AddMinutes(ClsTempoExecucaoBus.TEMPO_INTERVALO_RIOCARD_REEMBOLSOS);
                    ClsTempoExecucaoBus.NOVA_EXEC_RIOCARD_REEMBOLSOS = novaData.ToString("dd/MM/yyyy HH:mm");
                    Application.DoEvents();
                    break;
                case TipoThread.ENVIO_EMAIL_ALERTAS:
                    novaData = DateTime.Now.AddMinutes(ClsTempoExecucaoBus.TEMPO_INTERVALO_EMAILS_ALERTAS);
                    ClsTempoExecucaoBus.NOVA_EXEC_EMAILS_ALERTAS = novaData.ToString("dd/MM/yyyy HH:mm");
                    Application.DoEvents();
                    break;
                case TipoThread.ERROS_ACEITADOR:
                    novaData = DateTime.Now.AddMinutes(ClsTempoExecucaoBus.TEMPO_INTERVALO_ERROS_ACEITADOR);
                    ClsTempoExecucaoBus.NOVA_EXEC_ERROS_ACEITADOR = novaData.ToString("dd/MM/yyyy HH:mm");
                    Application.DoEvents();
                    break;
                case TipoThread.ERROS_IMPRESSORA:
                    novaData = DateTime.Now.AddMinutes(ClsTempoExecucaoBus.TEMPO_INTERVALO_ERROS_IMPRESSORA);
                    ClsTempoExecucaoBus.NOVA_EXEC_ERROS_IMPRESSORA = novaData.ToString("dd/MM/yyyy HH:mm");
                    Application.DoEvents();
                    break;
                case TipoThread.ERROS_LEITOR_CARTOES:
                    novaData = DateTime.Now.AddMinutes(ClsTempoExecucaoBus.TEMPO_INTERVALO_ERROS_LEITOR_CARTOES);
                    ClsTempoExecucaoBus.NOVA_EXEC_ERROS_LEITOR_CARTOES = novaData.ToString("dd/MM/yyyy HH:mm");
                    Application.DoEvents();
                    break;
                case TipoThread.CORRECAO_ACEITADOR:
                    novaData = DateTime.Now.AddMinutes(ClsTempoExecucaoBus.TEMPO_INTERVALO_CORRECAO_ACEITADOR);
                    ClsTempoExecucaoBus.NOVA_EXEC_CORRECAO_ACEITADOR = novaData.ToString("dd/MM/yyyy HH:mm");
                    Application.DoEvents();
                    break;
            }
        }

        private int getLineGrid(DataGridView grid, string chaveThread)
        {
            int j = 0;

            for (int i = 0; i < grid.Rows.Count; i++)
            {
                j = i;
                if (grid.Rows[i].Cells[0].Value.ToString() == (chaveThread))
                    break;
            }
            return j;
        }

        #endregion
    }
}
