﻿// <auto-generated />
using System;
using JwtUser.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JwtUser.Migrations
{
    [DbContext(typeof(LearnDataContext))]
    partial class LearnDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JwtUser.Repos.Models.TblCustomer", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal?>("Creditlimit")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("Taxcode")
                        .HasColumnType("int");

                    b.HasKey("Code");

                    b.ToTable("tbl_customer");
                });

            modelBuilder.Entity("JwtUser.Repos.Models.TblImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("image_data");

                    b.Property<string>("ImageName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("image_name");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("product_id");

                    b.HasKey("Id")
                        .HasName("PK__tbl_imag__3213E83FB3E1F904");

                    b.ToTable("tbl_images");
                });

            modelBuilder.Entity("JwtUser.Repos.Models.TblProduct", b =>
                {
                    b.Property<string>("Productid")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("productid");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("Price")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("price");

                    b.HasKey("Productid");

                    b.ToTable("tbl_products");
                });

            modelBuilder.Entity("JwtUser.Repos.Models.TblRefreshtoken", b =>
                {
                    b.Property<string>("Userid")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("userid");

                    b.Property<string>("Tokenid")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("tokenid");

                    b.Property<DateTime?>("Expiretime")
                        .HasColumnType("datetime")
                        .HasColumnName("expiretime");

                    b.Property<string>("Refreshtoken")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("refreshtoken");

                    b.Property<string>("TblUserUserId")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Userid", "Tokenid");

                    b.HasIndex(new[] { "TblUserUserId" }, "IX_tbl_refreshtoken_TblUserUserId");

                    b.ToTable("tbl_refreshtoken");
                });

            modelBuilder.Entity("JwtUser.Repos.Models.TblUser", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("user_id");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("email");

                    b.Property<bool?>("Isactive")
                        .HasColumnType("bit")
                        .HasColumnName("isactive");

                    b.Property<string>("Password")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("password");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("phone");

                    b.Property<string>("Role")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("role");

                    b.Property<string>("Username")
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("username");

                    b.HasKey("UserId");

                    b.ToTable("tbl_users");
                });

            modelBuilder.Entity("JwtUser.Repos.Models.TblRefreshtoken", b =>
                {
                    b.HasOne("JwtUser.Repos.Models.TblUser", "TblUserUser")
                        .WithMany("TblRefreshtokens")
                        .HasForeignKey("TblUserUserId");

                    b.Navigation("TblUserUser");
                });

            modelBuilder.Entity("JwtUser.Repos.Models.TblUser", b =>
                {
                    b.Navigation("TblRefreshtokens");
                });
#pragma warning restore 612, 618
        }
    }
}
