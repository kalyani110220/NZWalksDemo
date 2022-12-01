using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();
            //return DTO regions
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Name = region.Name,
            //        Code = region.Code,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,
            //    };
            //    regionsDTO.Add(regionDTO);
            //});
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);

        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Validate request
            if(!ValidateAddRegionAsync(addRegionRequest))
            {
                return BadRequest(ModelState);
            }
            //Request(DTO) to Domain Model
            var region = new Models.Domain.Region()
            {
                Name = addRegionRequest.Name,
                Code = addRegionRequest.Code,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population,
            };
            //Pass details to Repository
           region = await regionRepository.AddAsync(region);
            //Covert back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Name = region.Name,
                Lat = region.Lat,
                Code = region.Code,
                Long = region.Long,
                Population = region.Population
            };
            return CreatedAtAction(nameof(GetRegionAsync),new { id = regionDTO.Id }, regionDTO);

        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get region from Database
            var region = await regionRepository.DeleteAsync(id);
            //if null NotFound
            if(region == null)
            {
                return NotFound();
            }
            //Covert response back to DTO
            //var regionDTO = mapper.Map<Models.DTO.Region>(region);
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Name = region.Name,
                Lat = region.Lat,
                Code = region.Code,
                Long = region.Long,
                Population = region.Population
            };
            //return ok response
            return Ok(regionDTO);


        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id,[FromBody]Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //Validate incoming request
            if(!ValidateUpdateRegionAsync(updateRegionRequest))
            {
                return BadRequest(ModelState);
            }
            //Covert DTO to Domain Model
            var region = new Models.Domain.Region()
            {
                Name = updateRegionRequest.Name,
                Code = updateRegionRequest.Code,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population,
            };
            //Update Region using Repository
            region = await regionRepository.UpdateAsync(id, region);
            //if null then NotFound
            if( region == null)
            { 
                return NotFound();
            }
            //Covert Domain back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Name = region.Name,
                Lat = region.Lat,
                Code = region.Code,
                Long = region.Long,
                Population = region.Population
            };
            //Return ok response
            return Ok(regionDTO);

        }

        #region PrivateMethods
        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if(addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest),
                  $"Add Region Data is Requried.");
                    return false;
            }
            
            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code),
                    $"{nameof(addRegionRequest.Code)} canot be null or empty or white space.");
            }
            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name),
                    $"{nameof(addRegionRequest.Name)} canot be null or empty or white space.");
            }
            //if(addRegionRequest.Lat<=0)
            //{
            //    ModelState.AddModelError(nameof(addRegionRequest.Lat),
            //        $"{nameof(addRegionRequest.Lat)} canot be lessthan or equal to zero.");
            //}
            //if (addRegionRequest.Long <= 0)
            //{
            //    ModelState.AddModelError(nameof(addRegionRequest.Long),
            //        $"{nameof(addRegionRequest.Long)} canot be lessthan or equal to zero.");
            //}
            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population),
                    $"{nameof(addRegionRequest.Population)} canot be lessthan  zero.");
            }
            if(ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest),
                  $"Add Region Data is Requried.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                    $"{nameof(updateRegionRequest.Code)} canot be null or empty or white space.");
            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name),
                    $"{nameof(updateRegionRequest.Name)} canot be null or empty or white space.");
            }
            //if (updateRegionRequest.Lat <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateRegionRequest.Lat),
            //        $"{nameof(updateRegionRequest.Lat)} canot be lessthan or equal to zero.");
            //}
            //if (updateRegionRequest.Long <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateRegionRequest.Long),
            //        $"{nameof(updateRegionRequest.Long)} canot be lessthan or equal to zero.");
            //}
            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population),
                    $"{nameof(updateRegionRequest.Population)} canot be lessthan  zero.");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
            #endregion

    }

}
