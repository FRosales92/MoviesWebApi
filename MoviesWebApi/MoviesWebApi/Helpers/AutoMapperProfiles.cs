using AutoMapper;
using MoviesWebApi.DTOs;
using MoviesWebApi.Entities;

namespace MoviesWebApi.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
            
        {

            // GENDER 
            //From Gender to GenderDTO and reverse map from GenderDTO to Gender
            CreateMap<Gender, GenderDTO>().ReverseMap();
            //I will received CreationGenderDTO to Gender
            CreateMap<CreationGenderDTO, Gender>();


            // --- ACTORS -----

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<CreationActorDTO, Actor>().ForMember(x => x.Picture, options => options.Ignore());
            CreateMap<PatchActorDTO, Actor>().ReverseMap();

            // --- MOVIES ----

            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<CreationMovieDTO, Movie>().ForMember(x => x.Poster, options => options.Ignore());
            CreateMap<PatchMovieDTO, Movie>().ReverseMap();

        }
    }
}
