﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DungeonManager.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DungeonManagerEntities : DbContext
    {
        public DungeonManagerEntities()
            : base("name=DungeonManagerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Classes> Classes { get; set; }
        public virtual DbSet<Perks> Perks { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Skills> Skills { get; set; }
        public virtual DbSet<Stats> Stats { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Characters> Characters { get; set; }
    }
}
