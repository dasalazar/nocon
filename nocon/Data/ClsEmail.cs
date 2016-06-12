using System;
using System.Collections.Generic;
using System.Text;

namespace nocon.Data
{
    public class ClsEmail
    {
        public string Subjetc { get; set; }
        public string Body { get; set; }
        public List<string> To { get; set; }
        public ClsMonitoracao Monitoracao { get; set; }

    }
}
