using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChapelHill.models
{
    public class PayrollBatch
    {
        public string Code { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PayrollRun { get; set; }
    }
}
