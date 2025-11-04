namespace PinoyTodo.Infrastructure.Persistence.Repositories;

public abstract class BaseRepository
{
    protected readonly EventStoreDbContext Context;

    protected BaseRepository(EventStoreDbContext context)
    {
        Context = context;
    }
}