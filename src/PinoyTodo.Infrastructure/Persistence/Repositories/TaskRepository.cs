namespace PinoyTodo.Infrastructure.Persistence.Repositories;

public sealed class TaskRepository : BaseRepository
{
    public TaskRepository(EventStoreDbContext dbContext)
        : base(dbContext)
    {
    }
}