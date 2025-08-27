using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TODO.Models.Task
{
    public class TodoTask
    {
        [BsonId]
        [BindNever]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name field cannot exceed 100 characters.")]
        public string Name { get; set; } = null!;

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("isCompleted")]
        [BindNever]
        public bool IsCompleted { get; set; }

        [BsonElement("authorId")]
        [BindNever]
        [BsonRepresentation(BsonType.ObjectId)] 
        public string? AuthorId { get; set; }

        [BsonElement("createdAt")]
        [BindNever]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
