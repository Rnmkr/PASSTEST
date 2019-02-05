namespace PASSTEST.DataAccessLayer
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PRDB : DbContext
    {
        public PRDB()
            : base("data source=DESKTOP;initial catalog=PRODUCCION;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework")
        {
        }

        public virtual DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuarios>()
                .Property(e => e.LegajoUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.ApellidoUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.NombreUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.HashedRFID)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.HashedPassword)
                .IsUnicode(false);
        }
    }
}
