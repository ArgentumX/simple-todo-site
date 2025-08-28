using AutoMapper;
using MongoDB.Driver;
using System.Threading.Tasks;
using TODO.Models.Task;
using TODO.Models.Task.view;

namespace TODO.Services
{
    public class TaskService
    {
        private readonly IMongoCollection<TodoTask> _todoTasks;
        private readonly IMapper _mapper;

        public TaskService(IMongoDatabase database, IMapper mapper)
        {
            _mapper = mapper;
            _todoTasks = database.GetCollection<TodoTask>("TodoTasks");
        }

        public async Task<List<TodoTask>> GetAllAsync(string userId)
        {
            return await _todoTasks.Find(todoTask => todoTask.AuthorId == userId).ToListAsync();
        }

        public async Task<TodoTask> GetByIdAsync(string id, string userId)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var task = await _todoTasks.Find(task => task.Id == id).FirstOrDefaultAsync();
            if (task == null)
                throw new KeyNotFoundException($"Task with ID {id} not found.");

            EnsureUserIsAuthor(task, userId);
            return task;
        }
        public async Task CreateAsync(CreateTaskViewModel model, string userId)
        {

            TodoTask task = _mapper.Map<TodoTask>(model);

            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            task.AuthorId = userId;
            await _todoTasks.InsertOneAsync(task);
        }

        public async Task<bool> UpdateAsync(string id, EditTaskViewModel source, string userId)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var oldTask = await GetByIdAsync(id, userId);

            oldTask.Name = source.Name;
            oldTask.Description = source.Description;

            var result = await _todoTasks.ReplaceOneAsync(task => task.Id == id, oldTask);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }


        public async Task<bool> DeleteAsync(string id, string userId)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var task = await GetByIdAsync(id, userId); 

            var result = await _todoTasks.DeleteOneAsync(task => task.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }


        public async Task<bool> CompleteAsync(string id, string userId)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var task = await GetByIdAsync(id, userId); 
            task.IsCompleted = true;

            var result = await _todoTasks.ReplaceOneAsync(task => task.Id == id, task);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        private void EnsureUserIsAuthor(TodoTask task, string userId)
        {
            if (task.AuthorId != userId)
                throw new UnauthorizedAccessException("User is not authorized to access this task.");
        }
    }
}