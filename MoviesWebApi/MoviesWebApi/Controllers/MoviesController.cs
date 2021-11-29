using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesWebApi.DTOs;
using MoviesWebApi.Entities;
using MoviesWebApi.Servicios;

namespace MoviesWebApi.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorage fileStorage;
        private readonly string container = "movies";

        public MoviesController(ApplicationDbContext context, IMapper mapper, IFileStorage fileStorage)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieDTO>>> Get()
        {
            var movies = await context.Movies.ToListAsync();
            return mapper.Map<List<MovieDTO>>(movies);
        }

        [HttpGet("{id:int}", Name = "getMovie")]
        public async Task<ActionResult<MovieDTO>> Get(int id)
        {
            var movie = await context.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return mapper.Map<MovieDTO>(movie);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] CreationMovieDTO creationMovieDTO)
        {
            var movie = mapper.Map<Movie>(creationMovieDTO);
            if (creationMovieDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await creationMovieDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extention = Path.GetExtension(creationMovieDTO.Poster.FileName);
                    movie.Poster = await fileStorage.SaveFile(content, extention, container, creationMovieDTO.Poster.ContentType);
                }
            }
            context.Add(movie);
            await context.SaveChangesAsync();
            var movieDTO = mapper.Map<MovieDTO>(movie);
            return new CreatedAtRouteResult("getMovie", new { id = movie.Id }, movieDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] CreationMovieDTO creationMovieDTO)
        {
            var actorDB = await context.Actors.FirstOrDefaultAsync(actor => actor.Id == id);
            if (actorDB == null)
            {
                return NotFound();
            }

            actorDB = mapper.Map(creationMovieDTO, actorDB);
            if (creationMovieDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await creationMovieDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extention = Path.GetExtension(creationMovieDTO.Poster.FileName);
                    actorDB.Picture = await fileStorage.EditFile(content, extention, container, actorDB.Picture, creationMovieDTO.Poster.ContentType);
                }
            }
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")] //To make patch we need to install JsonPatchDocument
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<PatchMovieDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var entityDB = await context.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (entityDB == null)
            {
                return NotFound();
            }

            //from db to patchDTO
            var entityDTO = mapper.Map<PatchMovieDTO>(entityDB);
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
            var exist = await context.Movies.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }
            context.Remove(new Movie() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
