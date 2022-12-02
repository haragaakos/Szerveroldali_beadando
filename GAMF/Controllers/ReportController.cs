using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GAMF.Data;
using GAMF.ViewModels;

namespace GAMF.Controllers
{
    public class ReportController : Controller
    {
        private readonly GAMFDbContext _context;

        public ReportController(GAMFDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IActionResult EnrollmentDateReport()
        {
            var result = _context.Students.GroupBy(s => s.EnrollmentDate)
                .Select(s => new EnrollmentDateVM
                {
                    EnrollmentDate = s.Key,
                    StudentCount = s.Count()
                });


            return View(result.ToList());
        }

        [HttpGet("/Report/StudentsCredit")]
        public async Task<IActionResult> StudentsCredits()
        {
            var students = await _context.Students
                .Include(x => x.Enrollments)
                    .ThenInclude(z => z.Course).ToListAsync();

            var result = students.Select(x => new StudentCreditVM
            {
                Student = x,
                Credits = x.Enrollments.Sum(x => x.Course.Credits)
            });

            return View("StudentsCreditReport", result);
        }
    }
    }