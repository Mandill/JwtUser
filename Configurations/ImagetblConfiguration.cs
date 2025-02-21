using System.Reflection.Emit;
using JwtUser.Repos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HuntElectric.DAL.Configurations
{
    public class ImagetblConfiguration : IEntityTypeConfiguration<TblImage>
    {
        public void Configure(EntityTypeBuilder<TblImage> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__tbl_imag__3213E83FB3E1F904");

            builder.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        }
    }
}