using Core.common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.common;

public abstract class BaseConfiguration<T>:IEntityTypeConfiguration<T> where T:BaseEntity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);
        Configure(builder,typeof(T).Name);
    }

    protected abstract void Configure(EntityTypeBuilder<T> builder, string tableName);
}