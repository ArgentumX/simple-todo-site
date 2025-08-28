using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TODO.Models.Task.view
{
    public class TaskViewModel
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public string? AuthorId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
