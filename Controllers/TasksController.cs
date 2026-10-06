using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListMVC.Data;
using ToDoListMVC.Models;

namespace ToDoListMVC.Controllers;

public class TasksController : Controller
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string status = "all")
    {
        var query = _context.Tasks.AsQueryable();

        query = status switch
        {
            "active" => query.Where(t => !t.IsCompleted),
            "done"   => query.Where(t => t.IsCompleted),
            _        => query
        };

        ViewBag.Status = status;
        ViewBag.ActiveCount = await _context.Tasks.CountAsync(t => !t.IsCompleted);

        var tasks = await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
        return View(tasks);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task == null) return NotFound();

        return View(task);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description")] TaskItem task)
    {
        if (!ModelState.IsValid) return View(task);

        task.CreatedAt = DateTime.Now;
        task.IsCompleted = false;

        _context.Add(task);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        return View(task);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsCompleted,CreatedAt")] TaskItem task)
    {
        if (id != task.Id) return NotFound();
        if (!ModelState.IsValid) return View(task);

        try
        {
            _context.Update(task);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Tasks.Any(e => e.Id == task.Id)) return NotFound();
            throw;
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task == null) return NotFound();

        return View(task);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int id, string? returnUrl = null)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task != null)
        {
            task.IsCompleted = !task.IsCompleted;
            await _context.SaveChangesAsync();
        }
        return Redirect(returnUrl ?? Url.Action(nameof(Index))!);
    }
}