using MongoDB.Driver;
using TODO.Models;

namespace TODO.Services
{
    public class TaskService
    {
        private readonly IMongoCollection<TodoTask> _todoTasks;

        public TaskService(IMongoDatabase database)
        {
            _todoTasks = database.GetCollection<TodoTask>("TodoTasks");
        }

        public async Task<List<TodoTask>> GetAllAsync()
        {
            return await _todoTasks.Find(todoTask => true).ToListAsync();
        }

        public async Task<TodoTask> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            return await _todoTasks.Find(task => task.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(TodoTask todoTask)
        {
            if (todoTask == null)
                throw new ArgumentNullException(nameof(todoTask));

            await _todoTasks.InsertOneAsync(todoTask);
        }

        public async Task<bool> UpdateAsync(string id, TodoTask updatedTask)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            if (updatedTask == null)
                throw new ArgumentNullException(nameof(updatedTask));

            var result = await _todoTasks.ReplaceOneAsync(task => task.Id == id, updatedTask);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            var result = await _todoTasks.DeleteOneAsync(task => task.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}