using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository:IWalkRepository
    {
        private readonly NZWalkDbContext ctx;

        public WalkRepository(NZWalkDbContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            // Assign New ID
            walk.Id = Guid.NewGuid();
            await ctx.Walks.AddAsync(walk);
            await ctx.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await ctx.Walks.FindAsync(id);

            if (existingWalk == null)
            {
                return null;
            }

            ctx.Walks.Remove(existingWalk);
            await ctx.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await
                ctx.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public Task<Walk> GetAsync(Guid id)
        {
            return ctx.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await ctx.Walks.FindAsync(id);

            if (existingWalk != null)
            {
                existingWalk.Length = walk.Length;
                existingWalk.Name = walk.Name;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                existingWalk.RegionId = walk.RegionId;
                await ctx.SaveChangesAsync();
                return existingWalk;
            }

            return null;
        }
    }
}
