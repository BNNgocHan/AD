using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingFPT.Helpers;
using TrainingFPT.Models;
using TrainingFPT.Models.Queries;

namespace TrainingFPT.Controllers
{
    public class CoursesController : Controller
    {
        // INDEX
        [HttpGet]
        public IActionResult Index(string SearchString, string Status)
        {
            //if (String.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            //{
            //    return RedirectToAction(nameof(LoginController.Index), "login");
            //}

            CoursesViewModel course = new CoursesViewModel();
            course.CourseDetailList = new List<CourseDetail>();
            var dataCourses = new CourseQuery().GetAllDataCourses(SearchString, Status);
            foreach (var data in dataCourses)
            {
                course.CourseDetailList.Add(new CourseDetail
                {
                    CourseId = data.CourseId,
                    NameCourse = data.NameCourse,
                    CategoryId = data.CategoryId,
                    Description = data.Description,
                    Status = data.Status,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    ViewImageCourse = data.ViewImageCourse,
                    viewCategoryName = data.viewCategoryName
                });
            }
            ViewData["keyword"] = SearchString;
            ViewBag.Status = Status;
            return View(course);
        }

        // ADD
        [HttpGet]
        public IActionResult Add()
        {
            CourseDetail course = new CourseDetail();
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCategories = new CategoryQuery().GetAllCategories(null, null);
            foreach (var category in dataCategories)
            {
                items.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                });
            }
            ViewBag.Categories = items;
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CourseDetail course, IFormFile Image)
        {
            // return Ok(course);
            if (ModelState.IsValid)
            {
                try
                {
                    // return Ok(course);
                    string imageCourse = UploadFileHelper.UploadFile(Image);
                    // return Ok(course);
                    int idCourse = new CourseQuery().InsertDataCourse(
                        course.NameCourse,
                        course.CategoryId,
                        course.Description,
                        course.StartDate,
                        course.EndDate,
                        course.Status,
                        imageCourse
                    );

                    if (idCourse > 0)
                    {
                        TempData["saveStatus"] = true;
                    }
                    else
                    {
                        TempData["saveStatus"] = false;
                    }
                    return RedirectToAction(nameof(CoursesController.Index), "Courses");
                }
                catch (Exception ex)
                {
                    // neu co loi
                    return Ok(ex.Message);
                }
            }
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCategories = new CategoryQuery().GetAllCategories(null, null);
            foreach (var category in dataCategories)
            {
                items.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                });
            }
            ViewBag.Categories = items;
            return View(course);
        }
        
        [HttpGet]
        public IActionResult Edit(int id = 0)
        {
            CourseDetail courseDetail = new CourseQuery().GetDataCouseById(id);
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCategories = new CategoryQuery().GetAllCategories(null, null);
            foreach (var category in dataCategories)
            {
                items.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                });
            }
            ViewBag.Categories = items;
            return View(courseDetail);
        }
        [HttpPost]
        public IActionResult Edit(CourseDetail courseDetail, IFormFile Image)
        {
            try
            {
                //courseDetail.CategoryId = 2;
                var detail = new CourseQuery().GetDataCouseById(courseDetail.CourseId);
                // return Ok(courseDetail);
                string uniqueImage = detail.ViewImageCourse;
                if (courseDetail.Image != null)
                {
                    uniqueImage = UploadFileHelper.UploadFile(Image);
                }
                bool update = new CourseQuery().UpdateCourseById(
                    courseDetail.NameCourse,
                    courseDetail.Description,
                    uniqueImage,
                    courseDetail.CategoryId,
                    courseDetail.StartDate,
                    courseDetail.EndDate,
                    courseDetail.Status,
                    courseDetail.CourseId);
                if (update)
                {
                    TempData["updateStatus"] = true;
                }
                else
                {
                    TempData["updateStatus"] = false;
                }
                return RedirectToAction(nameof(CoursesController.Index), "Courses");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                // return View(courseDetail);
            }

        }

        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            bool del = new CourseQuery().DeleteItemCourse(id);
            if (del)
            {
                TempData["statusDel"] = true;
            }
            else
            {
                TempData["statusDel"] = false;
            }
            return RedirectToAction(nameof(CoursesController.Index), "Courses");
        }

    }
}