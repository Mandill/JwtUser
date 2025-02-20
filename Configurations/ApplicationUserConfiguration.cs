using JwtUser.Repos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HuntElectric.DAL.Configurations
{
    public class TblRefreshtokenConfiguration : IEntityTypeConfiguration<TblRefreshtoken>
    {
        public void Configure(EntityTypeBuilder<TblRefreshtoken> builder)
        {
            // Defining Composite Primary Key
            builder.HasKey(rt => new { rt.Userid, rt.Tokenid });
        }
    }
}
