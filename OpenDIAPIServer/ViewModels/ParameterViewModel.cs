using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDIAPIServer.ViewModels
{
    public class ParameterViewModel
    {
        
        public DateTime StartDate { get; set; }        
        public DateTime EndDate { get; set; }        
        public string PayrollBatch { get; set; }
        //1 - No Employee Filter, 2 - Employee Filter, 3 - payrollbatch
        [Required]
        public int Type { get; set; }        
        public IEnumerable<int> Employees { get; set; }
        public string TestRun { get; set; }
        public string TestEmployees { get; set; }
    }
}
