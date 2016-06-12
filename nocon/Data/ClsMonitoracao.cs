using System;
using System.Collections.Generic;
using System.Text;

namespace nocon.Data
{
    public class ClsMonitoracao
    {
        public long IdMonitoracao { get; set; }
        public long IdEquipamento { get; set; }
        public int IdAlerta { get; set; }

        public string DescricaoAlerta { get; set; }
        
        public DateTime DataHoraAlerta { get; set; }
        public int IndResolvido { get; set; }
        public DateTime DataHoraResolvido { get; set; }
        public int IndEmailEnviado { get; set; }
        public int IndEmailResolvido { get; set; }

        public string DescCliente { get; set; }
        public string DescAlerta { get; set; }
        public string DescLocal { get; set; }
        public string DescEquipamento { get; set; }
        
    }
}
