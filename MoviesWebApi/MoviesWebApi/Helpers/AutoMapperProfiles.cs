using AutoMapper;
using MoviesWebApi.DTOs;
using MoviesWebApi.Entities;

namespace MoviesWebApi.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            //From Gender to GenderDTO and reverse map from GenderDTO to Gender
            CreateMap<Gender, GenderDTO>().ReverseMap();
            //I will received CreationGenderDTO to Gender
            CreateMap<CreationGenderDTO, Gender>();

        }
    }
}
