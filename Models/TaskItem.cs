using System.ComponentModel.DataAnnotations;

namespace ToDoListMVC.Models;

public class TaskItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Название обязательно")]
    [StringLength(200)]
    [Display(Name = "Название")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    [Display(Name = "Описание")]
    public string? Description { get; set; }

    [Display(Name = "Выполнено")]
    public bool IsCompleted { get; set; }

    [Display(Name = "Дата создания")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}