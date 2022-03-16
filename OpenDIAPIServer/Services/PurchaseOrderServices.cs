using System.Collections.Generic;
using System.Linq;
using OpenDIAPIServer.Data;
using SupplyChain.Models;

namespace OpenDIAPIServer.Services
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
