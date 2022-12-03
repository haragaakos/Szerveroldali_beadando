using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAMF.Data;
using GAMF.Models;

namespace GAMF.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly GAMFDbContext _context;

        public EnrollmentController(GAMFDbContext context)
        {
            _context = context;
        }

        public IActionResult Index2() => View();

        public JsonResult GetEnrollments()
        {
            var enrollments = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .Select(e => new ViewModels.EnrollmentListVM
                {
                    CourseTitle = e.Course.Title,
                    StudentFullName = $"{e.Student.LastName} {e.Student.FirstName}",
                    Grade = e.Grade.ToString()
                });

            return Json(enrollments.ToList());
        }


        // GET: Enrollment
        public IActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "NameDesc" : "";
            ViewBag.CourseSortParm = sortOrder == "Course" ? "CourseDesc" : "Course";

            var enrollments = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                enrollments = enrollments.Where(
                    s => s.Student.LastName.ToUpper().Contains(searchString.ToUpper()) ||
                    s.Course.Title.ToUpper().Contains(searchString.ToUpper()));
            }

            enrollments = sortOrder switch
            {
                "Course" => enrollments.OrderBy(e => e.Course.Title),
                "CourseDesc" => enrollments.OrderByDescending(e => e.Course.Title),
                "NameDesc" => enrollments.OrderByDescending(e => e.Student.LastName),
                _ => enrollments.OrderBy(e => e.Student.LastName)
            };

            return View(enrollments.ToList());

        }

        // GET: Enrollment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Enrollments == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollment/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FirstName");
            return View();
        }

        // POST: Enrollment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentId,CourseId,StudentId,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FirstName", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Enrollments == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FirstName", enrollment.StudentId);
            return View(enrollment);
        }

        // POST: Enrollment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollmentId,CourseId,StudentId,Grade")] Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.EnrollmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FirstName", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Enrollments == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Enrollments == null)
            {
                return Problem("Entity set 'GAMFDbContext.Enrollments'  is null.");
            }
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id)
        {
            return (_context.Enrollments?.Any(e => e.EnrollmentId == id)).GetValueOrDefault();
        }
    }
}

