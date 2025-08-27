using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TODO.Models.Task;
using TODO.Models.Task.view;
using TODO.Services;

namespace TODO.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly TaskService _taskService;
        private readonly UserManager<MongoUser> _userManager;

        public TaskController(TaskService taskService, UserManager<MongoUser> userManager)
        {
            _taskService = taskService;
            _userManager = userManager; 
        }
        // GET: TaskController
        public async Task<ActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized("User must be authenticated.");

            var tasks = await _taskService.GetAllAsync(user.Id.ToString());
            return View(tasks);
        }

        // GET: TaskController/Details/{id}
        public async Task<ActionResult> Details(string id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized("User must be authenticated.");

                var task = await _taskService.GetByIdAsync(id, user.Id.ToString());
                return View(task);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

        }

        // GET: TaskController/Create
        public ActionResult Create()
        {
            return View(new CreateTaskViewModel());
        }

        // POST: TaskController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateTaskViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }

            await _taskService.CreateAsync(model, user.Id.ToString());
            return RedirectToAction(nameof(Index));
        }

        // GET: TaskController/Edit/{id}
        public async Task<ActionResult> Edit(string id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized("User must be authenticated.");

                var task = await _taskService.GetByIdAsync(id, user.Id.ToString());
                return View(task);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You are not authorized to edit this task.");
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
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized("User must be authenticated.");

                var updated = await _taskService.UpdateAsync(id, todoTask, user.Id.ToString());
                if (!updated)
                    return NotFound();
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            } 
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You are not authorized to edit this task.");
            }
        }

        // GET: TaskController/Delete/{id}
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized("User must be authenticated.");

                var task = await _taskService.GetByIdAsync(id, user.Id.ToString());
                return View(task);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You are not authorized to delete this task.");
            }
        }

        // POST: TaskController/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, TodoTask todoTask)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized("User must be authenticated.");

                var deleted = await _taskService.DeleteAsync(id, user.Id.ToString());
                if (!deleted)
                    return NotFound();
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You are not authorized to delete this task.");
            }
        }

        // POST: TaskController/Complete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Complete(string id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized("User must be authenticated.");

                var result = await _taskService.CompleteAsync(id, user.Id.ToString());
                if (!result)
                    return NotFound();
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You are not authorized to complete this task.");
            }
        }
    }

}
