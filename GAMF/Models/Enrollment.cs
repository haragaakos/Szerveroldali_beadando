using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAMF.Models
{
    public enum Grade { A, B, C, D, F }

    public class Enrollment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Jelentkezés azonosító")]
        public int EnrollmentId { get; set; }
        [Display(Name = "Kurzus azonosító")]
        public int CourseId { get; set; }
        [Display(Name = "Hallgató azonosító")]
        public int StudentId { get; set; }
        [Display(Name = "Osztályzat")]
        public Grade? Grade { get; set; }
        [Display(Name = "Kurzus")]
        public virtual Course? Course { get; set; }
        [Display(Name = "Hallgató")]
        public virtual Student? Student { get; set; }
    }
}