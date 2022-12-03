using System.ComponentModel.DataAnnotations;
using System;
namespace GAMF.ViewModels
{
    public class EnrollmentListVM
    {
        [Display(Name = "Tantárgy")]
        public string CourseTitle { get; set; }
        [Display(Name = "Hallgató")]
        public string StudentFullName { get; set; }
        [Display(Name = "Érdemjegy")]
        public string Grade { get; set; }

        public EnrollmentListVM()
        {
        }
    }
}
