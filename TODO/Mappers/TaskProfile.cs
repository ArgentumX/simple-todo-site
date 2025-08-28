using AutoMapper;
using TODO.Models.Task;
using TODO.Models.Task.view;

namespace TODO.Mappers
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            // Create 
            CreateMap<TodoTask, CreateTaskViewModel>();
            CreateMap<CreateTaskViewModel, TodoTask>();

            // Update
            CreateMap<TodoTask, EditTaskViewModel>();
            CreateMap<EditTaskViewModel, TodoTask>();

            // Details
            CreateMap<TodoTask, TaskViewModel>();
        }
    }
}
