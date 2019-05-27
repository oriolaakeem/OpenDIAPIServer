using System.Collections.Generic;
using System.Linq;
using EkoApiCore.Data;
using SupplyChain.Models;

namespace EkoApi.Services
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
