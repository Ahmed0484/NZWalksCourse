using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalkDbContext ctx;

        public RegionRepository(NZWalkDbContext ctx)
        {
            this.ctx = ctx;
        }
        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await this.ctx.Regions.ToListAsync();
        }
    }
}
