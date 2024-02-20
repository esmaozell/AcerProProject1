using AcerProProject1.Models;
using System.Linq.Expressions;

namespace AcerProProject1.Repository.IRepository
{
    public interface ITargetAPIRepository : IRepository<TargetAPI>
    {
        Task<TargetAPI> UpdateAsync(TargetAPI entity);
    }
}
