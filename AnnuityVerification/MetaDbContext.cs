using Microsoft.EntityFrameworkCore;

namespace AnnuityVerification
{
    public class MetaDbContext: DbContext
    {
        public MetaDbContext()
        {

        }

        public MetaDbContext(DbContextOptions<MetaDbContext> options) : base(options)
        {

        }


        public DbSet<VerificationPayLoad> tbl_Verification { get; set; }

    }
}
