using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository repo;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository repo,IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet]   
        public async  Task<IActionResult> Index()
        {
            var regions = await this.repo.GetAllAsync();
            /*var regionsDTO = new List<RegionDTO>();
            regions.ToList().ForEach(region => regionsDTO.Add(
                new RegionDTO
                {
                    Id=region.Id,
                    Name=region.Name,
                    Area=region.Area,
                    Code=region.Code,
                    Lat=region.Lat, 
                    Long=region.Long,
                    Population=region.Population
                }
                ));*/
            var regionsDTO=mapper.Map<List<RegionDTO>>(regions);
            return Ok(regionsDTO);
        }
    }
}
