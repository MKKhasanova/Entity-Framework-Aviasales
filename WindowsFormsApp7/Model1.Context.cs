﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsFormsApp7
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Database1Entities : DbContext
    {
        public Database1Entities()
            : base("name=Database1Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<Runways> Runways { get; set; }
        public virtual DbSet<Tickets> Tickets { get; set; }
        public virtual DbSet<Transplantations> Transplantations { get; set; }
        public virtual DbSet<Istory> Istorys { get; set; }
        public virtual DbSet<Istoriya> Istoriya { get; set; }
    }
}
