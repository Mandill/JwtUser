using System;
using System.Collections.Generic;
using JwtUser.Repos.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtUser.Repos;

public partial class LearnDataContext : DbContext
{
    public LearnDataContext()
    {
    }

    public LearnDataContext(DbContextOptions<LearnDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblImage> TblImages { get; set; }

    public virtual DbSet<TblMenu> TblMenus { get; set; }

    public virtual DbSet<TblOtpManager> TblOtpManagers { get; set; }

    public virtual DbSet<TblProduct> TblProducts { get; set; }

    public virtual DbSet<TblPwdManger> TblPwdMangers { get; set; }

    public virtual DbSet<TblRefreshtoken> TblRefreshtokens { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRolepermission> TblRolepermissions { get; set; }

    public virtual DbSet<TblTempuser> TblTempusers { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_imag__3213E83FB3E1F904");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<TblTempuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tbl_tempuser1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
