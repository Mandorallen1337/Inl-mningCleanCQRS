using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Book
    {
        public Guid Id { get; private set; }
        [Required]
        [MaxLength(20)]
        public string? Title { get; set; }
        [Required]
        [MaxLength(20)]
        public string? Description { get; set; }
        [Required]
        public Guid AuthorId { get; set; }

        [JsonPropertyName("authorDetails")]
        public Author Author { get; set; }

        public Book(string title, string description, Guid authorId)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            AuthorId = authorId;            
        }
    }
}
