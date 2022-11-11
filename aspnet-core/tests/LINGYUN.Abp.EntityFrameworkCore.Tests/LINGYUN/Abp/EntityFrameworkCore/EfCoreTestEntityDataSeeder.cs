using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.EntityFrameworkCore;
public class EfCoreTestEntityDataSeeder
{
    private readonly EfCoreTestDbContext _dbContext;

    public EfCoreTestEntityDataSeeder(
        EfCoreTestDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async virtual Task SeedAsync()
    {
        //_dbContext.GetService<IRelationalDatabaseCreator>().CreateTables();

        await _dbContext.TestEntities.AddAsync(
            new EfCoreTestEntity(Guid.NewGuid(), "1223", 1024, 1024L, new DateTime(2021, 10, 1, 0, 0, 0)));

        await _dbContext.TestEntities.AddAsync(
            new EfCoreTestEntity(Guid.NewGuid(), null, 2048, 2048L, new DateTime(2022, 10, 1, 12, 0, 0)));

        await _dbContext.TestEntities.AddAsync(
            new EfCoreTestEntity(Guid.NewGuid(), "3221", null, 4096L, null));

        await _dbContext.TestEntities.AddAsync(
            new EfCoreTestEntity(Guid.NewGuid(), null, null, null, new DateTime(2022, 1, 1, 12, 0, 0)));
    }
}
