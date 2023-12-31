using AutoMapper;
using toDoAPI.Models;

namespace toDoAPI.Dto
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<UserSignInDto, User>();
            CreateMap<UserChangeDetailsDto, User>();
            CreateMap<User, UserChangeDetailsDto>();
            CreateMap<Todo, TodoDto>();
            CreateMap<TodoDto, Todo>();
            CreateMap<TodoCreateDto, Todo>();
            CreateMap<Todo, TodoDetailsDto>();
        }
    }
}
