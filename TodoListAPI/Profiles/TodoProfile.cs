using AutoMapper;

namespace TodoListAPI.Profiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<Entities.Todo, Models.TodoDto>();
            CreateMap<Models.TodoForCreationDto, Entities.Todo>();
            CreateMap<Models.TodoForUpdateDto, Entities.Todo>()
                .ReverseMap();
        }
    }
}
