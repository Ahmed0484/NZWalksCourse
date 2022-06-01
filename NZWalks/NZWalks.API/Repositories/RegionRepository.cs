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
        public async Task<Region> GetAsync(Guid id)
        {
            return await ctx.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();//important
            await ctx.AddAsync(region);
            await ctx.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await ctx.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (region == null)
            {
                return null;
            }

            // Delete the region
            ctx.Regions.Remove(region);
            await ctx.SaveChangesAsync();
            return region;
        }


        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await ctx.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.Area = region.Area;
            existingRegion.Lat = region.Lat;
            existingRegion.Long = region.Long;
            existingRegion.Population = region.Population;

            await ctx.SaveChangesAsync();

            return existingRegion;
        }
    }
}

