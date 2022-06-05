using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository repo;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            // Fetch data from database - domain walks
            var walksDomain = await repo.GetAllAsync();
            
            // Convert domain walks to DTO Walks
            var walksDTO = mapper.Map<List<WalkDTO>>(walksDomain);

            // Return response
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            // Get Walk Domain object from database
            var walkDomain = await repo.GetAsync(id);
            if (walkDomain == null) return NotFound();
            // Convert Domain object to DTO
            var walkDTO = mapper.Map<WalkDTO>(walkDomain);

            // Return response
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] AddWalkRequest addWalkRequest)
        {
            // Convert DTO to Domain Object
            var walkDomain = new Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            // Pass domain object to Repository to persist this
            walkDomain = await repo.AddAsync(walkDomain);

            // Convert the Domain object back to DTO
            var walkDTO = new WalkDTO
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            // Send DTO response back to Client
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id,
            [FromBody] UpdateWalkRequest updateWalkRequest)
        {
            // Convert DTO to Domain object
            var walkDomain = new Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            // Pass details to Repository - Get Domain object in response (or null)
            walkDomain = await repo.UpdateAsync(id, walkDomain);

            // Handle Null (not found)
            if (walkDomain == null)
            {
                return NotFound();
            }

            // Convert back Domain to DTO
            var walkDTO = new WalkDTO
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            // Return Response
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            // call Repository to delete walk
            var walkDomain = await repo.DeleteAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<WalkDTO>(walkDomain);

            return Ok(walkDTO);
        }

    }
}
