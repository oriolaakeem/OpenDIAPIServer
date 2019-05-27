using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EkoApiCore.Data;
using SupplyChain.Models;

namespace EkoApi.Services
{
    public class GRNServices
    {
        private SAPB1 _context { get; set; }

        public List<GRN> Get()
        {
            return _context.GRNs.ToList();
        }
    }
}
