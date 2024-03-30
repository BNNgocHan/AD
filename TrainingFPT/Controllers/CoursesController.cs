using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingFPT.Helpers;
using TrainingFPT.Models;
using TrainingFPT.Models.Queries;

namespace TrainingFPT.Controllers
{
    public class CoursesController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            //if (String.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            //{
            //    return RedirectToAction(nameof(LoginController.Index), "login");
            //}

            CoursesViewModel course = new CoursesViewModel();
            course.CourseDetailList = new List<CourseDetail>();
            var dataCourses = new CourseQuery().GetAllDataCourses();
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
                    viewCategoryName = data.viewCategoryName,
                    CreatedAt = data.CreatedAt,
                    UpdatedAt = data.UpdatedAt
                });
            }
            return View(course);
        }

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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Add(CourseDetail course, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string imageCourse = UploadFileHelper.UploadFile(Image);
                    int idCourse = new CourseQuery().InsertDataCourse(
                        course.NameCourse,
                        course.CategoryId,
                        course.Description,
                        course.StartDate,
                        course.EndDate,
                        course.Status,
                        imageCourse
                    );

                    if ( idCourse > 0 )
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
                    //nếu có lỗi
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
            return View();
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

        

        [HttpGet]
        public IActionResult Edit(int id)
        {
            CourseDetail courseDetail = new CourseQuery().GetDataCourseById(id);

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseDetail courseDetail, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var detail = new CourseQuery().GetDataCourseById(courseDetail.CourseId);
                    string uniqueImage = detail.ViewImageCourse; //lấy lại tên ảnh cũ trước khi thay ảnh mới (nếu có)

                    //kiểm tra người dùng có muốn thay ảnh poster không?
                    if (courseDetail.ViewImageCourse != null)
                    {
                        //có thay ảnh
                        uniqueImage = UploadFileHelper.UploadFile(Image);
                    }

                    bool update = new CourseQuery().UpdateCourseById(
                        courseDetail.NameCourse,
                        courseDetail.CategoryId,
                        courseDetail.Description,
                        courseDetail.StartDate,
                        courseDetail.EndDate,
                        uniqueImage,
                        courseDetail.Status,
                        courseDetail.CourseId
                    );

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
            return View(courseDetail);
        }
    }
}