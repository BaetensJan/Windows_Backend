using Windows_Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Windows_Backend.Data.Configurations
{
    public class BusinessConfiguration: IEntityTypeConfiguration<Business>

    {
        public void Configure(EntityTypeBuilder<Business> builder)
        {
            builder.ToTable("Business");
            builder.ToTable("Business");
        }
    }
}