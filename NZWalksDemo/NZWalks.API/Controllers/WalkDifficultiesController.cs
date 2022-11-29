using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }
        [HttpGet]

        public async Task<IActionResult> GetAllWalksDifficultyAsync()
        {
            //fetch data from database-domain walks
            var walksDifficultyDomain = await walkDifficultyRepository.GetAllAsync();
            //convert domain walks to DTOWalksdifficulty
            var walkDifficutyDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walksDifficultyDomain);
            //return response
            return Ok(walkDifficutyDTO);

        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            //get walkDiffulty domain object from database
            var walkDifficultyDomain = await walkDifficultyRepository.GetAsync(id);
            if(walkDifficultyDomain == null)
            {
                return NotFound();
            }
            //Convert Domain object to DTO
            var walkDifficutyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            //Return response
            return Ok(walkDifficutyDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddtWalksDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //Convert DTO to domain object
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code,
            };  
            //pass domain object to repository to persist this
            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);
            //Covert domain object back to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            //var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            //{
            //    Id = walkDifficultyDomain.Id,
            //    Code = walkDifficultyDomain.Code,
            //};
            //Send DTO response back to client
            return CreatedAtAction(nameof(GetWalkDifficultyAsync), 
                new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync( Guid id,
             Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //Convert DTO to Domain object
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code,
            };
            //Call Repository to update
            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);
             if(walkDifficultyDomain==null)
            {
                return NotFound();
            }
             //Convert Domain to DTO
             var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            //Return response
            return Ok(walkDifficultyDTO);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            //Call Repository  to delete walk
            var walkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);
            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }
            //Convert  to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            return Ok(walkDifficultyDTO);
        }



    }
}

