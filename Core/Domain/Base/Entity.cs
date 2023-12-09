namespace Core.Domain.Base
{
    public abstract class Entity<TId>
    {
        public virtual TId Id { get; set; } = default!;
    }
}
