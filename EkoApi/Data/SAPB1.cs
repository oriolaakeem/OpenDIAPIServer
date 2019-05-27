using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EkoApiCore.models;
using Microsoft.EntityFrameworkCore;
using SupplyChain.Models;

namespace EkoApiCore.Data
{
    public class SAPB1 : DbContext
    {
        public SAPB1(DbContextOptions<SAPB1> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
        public DbSet<BusinessPatner> BusinessPatners { get; set; }
        public DbSet<BPLookup> BPLoookups { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<GRN> GRNs { get; set; }
        public DbSet<PurchaseRequest> PurchaseRequests { get; set; }
        public DbSet<Items> Items { get; set; }
    }
}
