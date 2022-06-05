using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository :IWalkDifficultyRepository
    {
        private readonly NZWalkDbContext ctx;

    public WalkDifficultyRepository(NZWalkDbContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
    {
        walkDifficulty.Id = Guid.NewGuid();
        await ctx.WalkDifficulty.AddAsync(walkDifficulty);
        await ctx.SaveChangesAsync();
        return walkDifficulty;
    }

    public async Task<WalkDifficulty> DeleteAsync(Guid id)
    {
        var existingWalkDifficulty = await ctx.WalkDifficulty.FindAsync(id);
        if (existingWalkDifficulty != null)
        {
            ctx.WalkDifficulty.Remove(existingWalkDifficulty);
            await ctx.SaveChangesAsync();
            return existingWalkDifficulty;
        }
        return null;
    }

    public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
    {
        return await ctx.WalkDifficulty.ToListAsync();
    }

    public async Task<WalkDifficulty> GetAsync(Guid id)
    {
        return await ctx.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
    {
        var existingWalkDifficulty = await ctx.WalkDifficulty.FindAsync(id);

        if (existingWalkDifficulty == null)
        {
            return null;
        }

        existingWalkDifficulty.Code = walkDifficulty.Code;
        await ctx.SaveChangesAsync();
        return existingWalkDifficulty;
    }
}
}
