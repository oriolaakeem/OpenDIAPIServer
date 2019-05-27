using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EkoApiCore.Data;
using SupplyChain.Models;

namespace EkoApi.Services
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
