using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesWebApi.DTOs;
using MoviesWebApi.Entities;

namespace MoviesWebApi.Controllers
{
    [ApiController]
    [Route("api/genders")]
    public class GendersController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GendersController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenderDTO>>> Get()
        {
            var entities = await context.Genders.ToListAsync();
            var dtos = mapper.Map<List<GenderDTO>>(entities);
            return dtos;
        }

        [HttpGet("{id:int}",Name ="getGender")]
        public async Task<ActionResult<GenderDTO>> Get(int id)
        {
            var entyti = await context.Genders.FirstOrDefaultAsync(gender => gender.Id == id);
            if (entyti == null)
            {
                return NotFound();
            }
            var dto = mapper.Map<GenderDTO>(entyti);
            return dto; 
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreationGenderDTO crationGenderDTO)
        {
            var entity = mapper.Map<Gender>(crationGenderDTO);
            context.Add(entity);
            await context.SaveChangesAsync();
            var genderDTO = mapper.Map<GenderDTO>(entity);
            return new CreatedAtRouteResult("getGender", new {id = genderDTO.Id }, genderDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CreationGenderDTO crationGenderDTO)
        {
            var entity = mapper.Map<Gender>(crationGenderDTO);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Genders.AnyAsync( gender => gender.Id == id);
            if (!exist)
            {
                return NotFound();
            }
            context.Remove(new Gender() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
