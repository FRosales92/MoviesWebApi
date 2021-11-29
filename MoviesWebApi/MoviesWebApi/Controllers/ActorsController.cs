using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesWebApi.DTOs;
using MoviesWebApi.Entities;
using MoviesWebApi.Helpers;
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
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.Actors.AsQueryable();
            await HttpContext.InsertParameterPagination(queryable, paginationDTO.RecordPerPage);
            var entities = await queryable.Paginar(paginationDTO).ToListAsync();
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

        [HttpPatch("{id:int}")] //To make patch we need to install JsonPatchDocument
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<PatchActorDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var entityDB = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (entityDB == null)
            {
                return NotFound();
            }

            //from db to patchDTO
            var entityDTO = mapper.Map<PatchActorDTO>(entityDB);
            patchDocument.ApplyTo(entityDTO, ModelState); //Isntall Newtonsof
            var isValid = TryValidateModel(entityDTO);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(entityDTO, entityDB);
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
