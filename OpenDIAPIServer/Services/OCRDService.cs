using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenDIAPIServer.Data;
using SupplyChain.Models;

namespace OpenDIAPIServer.Services
{
    public class OCRDService
    {
        private SAPB1 _context { get; set; }

        public List<BusinessPatner> Get()
        {
           return _context.BusinessPatners.ToList();
        }
    }
}
