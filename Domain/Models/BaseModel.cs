using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class BaseModel
    {
        [Key]
        public Guid Id { get; set; }= Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }= DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}
