using Microsoft.EntityFrameworkCore;

namespace TaskFlowApi.Data;

public class TaskFlowDbContext : DbContext
{
    public TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options) : base(options)
    {
    }
}