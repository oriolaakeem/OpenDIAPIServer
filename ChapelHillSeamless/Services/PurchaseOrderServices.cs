using System.Collections.Generic;
using System.Linq;
using ChapelHill.Data;
using SupplyChain.Models;

namespace ChapelHill.Services
{
    public class PurchaseOrderServices
    {
        private SAPB1 _context { get; set; }

        public List<PurchaseOrder> Get()
        {
            return _context.PurchaseOrders.ToList();
        }
    }
}
