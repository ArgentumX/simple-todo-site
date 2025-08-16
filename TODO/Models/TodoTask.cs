using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TODO.Models
{
    public class TodoTask
    {
        [BsonId]
        [BindNever]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name field cannot exceed 100 characters.")]
        public string Name { get; set; } = null!;

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("isCompleted")]
        public bool IsCompleted { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public void Update(TodoTask source)
        {
            this.Name = source.Name;
            this.Description = source.Description;
        }

    }
}
