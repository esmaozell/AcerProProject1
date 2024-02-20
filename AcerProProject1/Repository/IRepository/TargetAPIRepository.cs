using AcerProProject1.Data;
using AcerProProject1.Models;
using AcerProProject1.Repository.IRepository;
using AcerProProject1.Repository;

public class TargetAPIRepository : Repository<TargetAPI>, ITargetAPIRepository
{
    private readonly ApplicationDbContext _context;

    public TargetAPIRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<TargetAPI> UpdateAsync(TargetAPI entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _context.TargetApis.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
