using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChapelHill.Data;
using SupplyChain.Models;

namespace ChapelHill.Services
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
