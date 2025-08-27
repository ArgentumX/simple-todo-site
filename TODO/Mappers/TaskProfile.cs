using AutoMapper;
using TODO.Models.Task;
using TODO.Models.Task.view;

namespace TODO.Mappers
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TodoTask, CreateTaskViewModel>();
            CreateMap<CreateTaskViewModel, TodoTask>();
        }
    }
}
