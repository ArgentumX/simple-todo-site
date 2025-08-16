using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TODO.Models;
using TODO.Services;

namespace TODO.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }
        // GET: TaskController
        public async Task<ActionResult> Index()
        {
            var tasks = await _taskService.GetAllAsync();
            return View(tasks);
        }

        // GET: TaskController/Details/{id}
        public async Task<ActionResult> Details(string id)
        {
            try
            {
                var task = await _taskService.GetByIdAsync(id);
                if (task == null)
                    return NotFound();
                return View(task);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Task ID cannot be null or empty.");
            }
        }

        // GET: TaskController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaskController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TodoTask todoTask)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(todoTask);
            }

            try
            {
                await _taskService.CreateAsync(todoTask);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentNullException ex)
            {
                ModelState.AddModelError("", $"Task data cannot be null: {ex.Message}");
                return View(todoTask);
            }
        }

        // GET: TaskController/Edit/{id}
        public async Task<ActionResult> Edit(string id)
        {
            try
            {
                var task = await _taskService.GetByIdAsync(id);
                if (task == null)
                    return NotFound();
                return View(task);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Task ID cannot be null or empty.");
            }
        }

        // POST: TaskController/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, TodoTask todoTask)
        {
            if (!ModelState.IsValid)
                return View(todoTask);

            try
            {
                var updated = await _taskService.UpdateAsync(id, todoTask);
                if (!updated)
                    return NotFound();
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentNullException)
            {
                ModelState.AddModelError("", "Task ID or data cannot be null.");
                return View(todoTask);
            }
        }

        // GET: TaskController/Delete/{id}
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var task = await _taskService.GetByIdAsync(id);
                if (task == null)
                    return NotFound();
                return View(task);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Task ID cannot be null or empty.");
            }
        }

        // POST: TaskController/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, TodoTask todoTask)
        {
            try
            {
                var deleted = await _taskService.DeleteAsync(id);
                if (!deleted)
                    return NotFound();
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentNullException)
            {
                ModelState.AddModelError("", "Task ID cannot be null or empty.");
                return View(todoTask);
            }
        }

    }

}
