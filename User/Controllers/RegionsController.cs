using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegionsUser.Models.Domains;
using RegionsUser.Models.Dto;
using User.Data;

namespace RegionsUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly UsersDbContext dbcontext;

        public RegionsController(UsersDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        //GET ALL METHOD
        [HttpGet]
        public IActionResult GetAll()
        {
            var region= dbcontext.Regions.ToList();
            return Ok(region);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionsDto addRegionsDto)
        {
            var Verify = await dbcontext.Regions.FirstOrDefaultAsync(c => c.Name == addRegionsDto.Name);
            if (Verify != null)
            {
                return BadRequest("Region already exists");
            }
            var rDomainModel = new Region
            {
                Name = addRegionsDto.Name,
                Code = addRegionsDto.Code,
                Description = addRegionsDto.Description,
            };
            dbcontext.Regions.Add(rDomainModel);
            dbcontext.SaveChanges();
            var regionDto= new RegionsDto
            {
                Id = addRegionsDto.Id,
                Name = addRegionsDto.Name,
                Code = addRegionsDto.Code,
                Description = addRegionsDto.Description,
            };
            return CreatedAtAction(nameof(GetById), new { Id = rDomainModel.Id }, rDomainModel );            
        }

        //get region by id
        [HttpGet]
        [Route("{Id:Guid}")]
        public IActionResult GetById(Guid Id)
        {
            var regions = dbcontext.Regions.Find(Id);
            if (regions == null)
            {
                return NotFound();
            }
            return Ok(regions);
        }
        [HttpPut("{Id}")]
        public IActionResult Update(Guid Id, [FromBody] UpdateRegionsDto updateRegionDto)
        {
            var region = dbcontext.Regions.FirstOrDefault(r => r.Id == Id);
            if (region == null)
            {
                return NotFound();
                // Return 404 if the region with the specified ID is not found
            }

            // Update properties of the existing region entity
            region.Code = updateRegionDto.Code;
            region.Name = updateRegionDto.Name;
            region.Description = updateRegionDto.Description;

            // Save the changes to the database
            dbcontext.SaveChanges();

            // Return the updated region DTO
            var updatedRegionDto = new RegionsDto
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Description = region.Description,
            };

            // Return 200 OK response with the updated region DTO
            return Ok(updatedRegionDto);
        }



        [HttpDelete]
        [Route("{Id:Guid}")]
        public IActionResult DeleteById(Guid Id)
        {
            var regions = dbcontext.Regions.Find(Id);
            if (regions == null)
            {
                return NotFound();
            }
            dbcontext.Regions.Remove(regions);
            dbcontext.SaveChanges();
            return NoContent();
        }
      /*  [HttpGet("{regionName}")]
        public IActionResult GetRegionalIdByName(string regionName)
        {
            var region = usersDbContext.Regions.FirstOrDefault(r => r.Name == regionName);
            if (region == null)
            {
                return NotFound("Region not found");
            }

            return Ok(region.Id);
        }*/


    }
}
