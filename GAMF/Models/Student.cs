using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GAMF.Models
{
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Display(Name = "Családnév")]
        [Required(ErrorMessage = "Családnév megadása kötelező!")]
        public string LastName { get; set; } = "";
        [Display(Name = "Keresztnév")]
        [Required(ErrorMessage = "Keresztnév megadása kötelező!")]
        public string FirstName { get; set; } = "";
        [Display(Name = "Első jelentkezés")]
        [Required(ErrorMessage = "Első jelentkezés mező kitöltése kötelező!")]
        public DateTimeOffset EnrollmentDate { get; set; }
        [Display(Name = "Jelentkezések")]
        public virtual ICollection<Enrollment>? Enrollments { get; set; }
    }
}

