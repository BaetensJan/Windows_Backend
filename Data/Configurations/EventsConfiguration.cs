using Windows_Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Windows_Backend.Data.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>

    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");
        }
    }
}