using System;
using System.Collections.Generic;
using System.Text;

namespace nocon.Data
{
    public class ClsIncidente
    {
        public long IncId { get; set; }
        public int IncCliId { get; set; }
        public long IncEquipamento{ get; set; }
        public long IncCartao { get; set; }
        public DateTime IncDtAbert { get; set; }
        public string IncStatus { get; set; }
        public decimal IncValor { get; set; }
        public string IncNsuOrigem { get; set; }
        public string IncNsuResolucao{ get; set; }
        public string IncMotivo { get; set; }
    }
}
