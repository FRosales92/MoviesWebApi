using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesWebApi.DTOs;
using MoviesWebApi.Entities;
using MoviesWebApi.Servicios;

namespace MoviesWebApi.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorsController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorage fileStorage;
        private readonly string container = "actors";

        public ActorsController(ApplicationDbContext context, IMapper mapper, IFileStorage fileStorage)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get()
        {
            var entities = await context.Actors.ToListAsync();
            return mapper.Map<List<ActorDTO>>(entities);
        }
        [HttpGet("{id:int}", Name="getActor")]
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
        public async Task<ActionResult> Post([FromForm] CreationActorDTO creationActorDTO)
        {
            var entity = mapper.Map<Actor>(creationActorDTO);
            if (creationActorDTO.Picture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await creationActorDTO.Picture.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extention = Path.GetExtension(creationActorDTO.Picture.FileName);
                    entity.Picture = await fileStorage.SaveFile(content, extention, container, creationActorDTO.Picture.ContentType);
                }
            }

            context.Add(entity);
            await context.SaveChangesAsync();
            var actorDto = mapper.Map<ActorDTO>(entity);
            return new CreatedAtRouteResult("getActor", new { id = actorDto.Id }, actorDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromForm] CreationActorDTO creationActorDTO, int id)
        {
            //var entity = mapper.Map<Actor>(creationActorDTO);
            //entity.Id = id;
            //context.Entry(entity).State = EntityState.Modified;

            var actorDB = await context.Actors.FirstOrDefaultAsync(actor => actor.Id == id);
            if(actorDB == null)
            {
                return NotFound();
            }

            actorDB = mapper.Map(creationActorDTO, actorDB);
            if (creationActorDTO.Picture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await creationActorDTO.Picture.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extention = Path.GetExtension(creationActorDTO.Picture.FileName);
                    actorDB.Picture = await fileStorage.EditFile(content, extention, container,actorDB.Picture, creationActorDTO.Picture.ContentType);
                }
            }

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
