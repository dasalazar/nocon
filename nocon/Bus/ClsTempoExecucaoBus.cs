using System;
using System.Collections.Generic;
using System.Text;

namespace nocon.Bus
{
    public static class ClsTempoExecucaoBus
    {
        public static int TEMPO_INTERVALO_RIOCARD_RESOLVEVE_REEMBOLSOS_EN = 3;
        public static string NOVA_EXEC_RIOCARD_RESOLVE_REEMBOLSOS_EN;

        public static int TEMPO_INTERVALO_WEBSERVICE_VERIFICA = 1;
        public static string NOVA_EXEC_WEBSERVICE_VERIFICA;

        public static int TEMPO_INTERVALO_RIOCARD_REEMBOLSOS = 2;
        public static string NOVA_EXEC_RIOCARD_REEMBOLSOS;

        public static int TEMPO_INTERVALO_EMAILS_ALERTAS = 2;
        public static string NOVA_EXEC_EMAILS_ALERTAS;

        public static int TEMPO_INTERVALO_ERROS_ACEITADOR = 3;
        public static string NOVA_EXEC_ERROS_ACEITADOR;

        public static int TEMPO_INTERVALO_ERROS_IMPRESSORA = 10;
        public static string NOVA_EXEC_ERROS_IMPRESSORA;

        public static int TEMPO_INTERVALO_ERROS_LEITOR_CARTOES = 5;
        public static string NOVA_EXEC_ERROS_LEITOR_CARTOES;

        public static int TEMPO_INTERVALO_CORRECAO_ACEITADOR = 4;
        public static string NOVA_EXEC_CORRECAO_ACEITADOR;

        public static int TEMPO_INTERVALO_ATM_OFF = 3;
        public static string NOVA_EXEC_ATM_OFF;
    }
}
