using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {

        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        [HttpGet]

        public async Task<IActionResult> GetAllWalksAsync()
        {
            //fetch data from database-domain walks
            var walksDomain = await walkRepository.GetAllAsync();
            //convert domain walks to DTOWalks
            //var walksDTO = new List<Models.DTO.Region>();
            //walksDomain.ToList().ForEach(walksDomain =>
            //{
            //    var walkDTO = new Models.DTO.Walk()
            //    {
            //        Id = region.Id,
            //        Name = region.Name,
            //        Code = region.Code,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,
            //    };
            //    walksDTO.Add(walkDTO);
            //});
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);
            //return response

            return Ok(walksDTO);

        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //get walk domain object from database
            var walkDomain = await walkRepository.GetAsync(id);
            //Convert Domain object to DTO
            var walksDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            //Return response
            return Ok(walksDTO);
        
          }
        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody]Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Convert DTO to domain object
            var walksDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId,
            };
            //pass domain object to repository to persist this
            walksDomain = await walkRepository.AddAsync(walksDomain);
            //Covert domain object back to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walksDomain.Id,
                Length = walksDomain.Length,
                Name = walksDomain.Name,
                RegionId = walksDomain.RegionId,
                WalkDifficultyId = walksDomain.WalkDifficultyId,
            };
            //Send DTO response back to client
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute]Guid id,
            [FromBody]Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Convert DTO to Domain object
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId,

            };
            //Pass derails to repository -Get domain object in response (or null)
            walkDomain = await walkRepository.UpdateAsync(id,walkDomain);
            //Convert back Domain to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId,
            };
            //return rexsponse
            return Ok(walkDTO);

        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //Call Repository  to delete walk
            var walkDomain = await walkRepository.DeleteAsync(id);
            if(walkDomain == null)
            {
                return NotFound();
            }
         var walkDTO =   mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(walkDTO);
        }
    }
}
