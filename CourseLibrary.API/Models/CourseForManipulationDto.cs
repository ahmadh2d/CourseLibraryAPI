using CourseLibrary.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLibrary.API.Models
{
    [CourseTitleMustBeDifferentFromDescription(ErrorMessage = "Title and Descriptions should be different!")]
    public abstract class CourseForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out title")]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1500)]
        public virtual string Description { get; set; }
    }
}
