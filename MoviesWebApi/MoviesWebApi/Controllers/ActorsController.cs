using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesWebApi.DTOs;
using MoviesWebApi.Entities;

namespace MoviesWebApi.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorsController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ActorsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get()
        {
            var entities = await context.Actors.ToListAsync();
            return mapper.Map<List<ActorDTO>>(entities);
        }
        [HttpGet("{id:int}", Name = "getActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var entity = await context.Actors.FirstOrDefaultAsync(actor => actor.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = mapper.Map<ActorDTO>(entity);
            return (dto);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreationActorDTO creationActorDTO)
        {
            var entity = mapper.Map<Actor>(creationActorDTO);
            context.Add(entity);
            await context.SaveChangesAsync();

            var dto = mapper.Map<ActorDTO>(entity);

            return new CreatedAtRouteResult("getActor", new { id = entity.Id }, dto);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromBody] CreationActorDTO creationActorDTO, int id)
        {
            var entity = mapper.Map<Actor>(creationActorDTO);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Actors.AnyAsync(actor => actor.Id == id);
            if (!exist)
            {
                return NotFound();
            }
            context.Remove(new Actor() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
