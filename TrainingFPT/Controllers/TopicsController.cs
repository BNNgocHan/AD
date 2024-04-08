using Microsoft.AspNetCore.Mvc;
using TrainingFPT.Models.Queries;
using TrainingFPT.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingFPT.Migrations;
using TrainingFPT.Helpers;

namespace TrainingFPT.Controllers
{
    public class TopicsController : Controller
    {
        // INDEX
        [HttpGet]   
        public IActionResult Index(string SearchString, string Status)
        {
            //if (String.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            //{
            //    return RedirectToAction(nameof(LoginController.Index), "login");
            //}

            TopicsViewModel topic = new TopicsViewModel();
            topic.TopicDetailList = new List<TopicDetail>();
            var dataTopics = new TopicQuery().GetAllDataTopics(SearchString, Status);
            foreach (var data in dataTopics)
            {
                topic.TopicDetailList.Add(new TopicDetail
                {
                    TopicId = data.TopicId,
                    NameTopic = data.NameTopic,
                    CourseId = data.CourseId,
                    Description = data.Description,
                    ViewVideo = data.ViewVideo,
                    ViewAudio = data.ViewAudio,
                    ViewDocumentTopic = data.ViewDocumentTopic,
                    Status = data.Status,
                    viewCourseName = data.viewCourseName
                });
            }
            ViewData["keyword"] = SearchString;
            ViewBag.Status = Status;
            return View(topic);
        }

        // ADD
        [HttpGet]
        public IActionResult Add()
        {
            TopicDetail topic = new TopicDetail();
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCourses = new CourseQuery().GetAllDataCourses(null, null);
            foreach (var course in dataCourses)
            {
                items.Add(new SelectListItem
                {
                    Value = course.CourseId.ToString(),
                    Text = course.NameCourse
                });
            }
            ViewBag.Courses = items;
            return View(topic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TopicDetail topic, IFormFile Video, IFormFile Audio, IFormFile DocumentTopic)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string videoTopic = UploadFileHelper.UploadFile(Video);
                    string audioTopic = UploadFileHelper.UploadFile(Audio);
                    string documentTopic = UploadFileHelper.UploadFile(DocumentTopic);
                    int idTopic = new TopicQuery().InsertDataTopic(
                        topic.NameTopic,
                        topic.CourseId,
                        topic.Description,
                        videoTopic,
                        audioTopic,
                        documentTopic,
                        topic.Status
                    );
                    if (idTopic > 0)
                    {
                        TempData["saveStatus"] = true;
                    }
                    else
                    {
                        TempData["saveStatus"] = false;
                    }
                    return RedirectToAction(nameof(TopicsController.Index), "Topics");
                }
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                }
            }
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCourses = new CourseQuery().GetAllDataCourses(null, null);
            foreach (var course in dataCourses)
            {
                items.Add(new SelectListItem
                {
                    Value = course.CourseId.ToString(),
                    Text = course.NameCourse
                });
            }
            ViewBag.Courses = items;
            return View(topic);
        }

        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            bool del = new TopicQuery().DeleteItemTopic(id);
            if (del)
            {
                TempData["statusDel"] = true;
            }
            else
            {
                TempData["statusDel"] = false;
            }
            return RedirectToAction(nameof(TopicsController.Index), "Topics");
        }

        // UPDATE
        [HttpGet]
        public IActionResult Edit(int id = 0)
        {
            TopicDetail topicDetail = new TopicQuery().GetDataTopicById(id);
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCourses = new CourseQuery().GetAllDataCourses(null, null);
            foreach (var course in dataCourses)
            {
                items.Add(new SelectListItem
                {
                    Value = course.CourseId.ToString(),
                    Text = course.NameCourse
                });
            }
            ViewBag.Courses = items;
            return View(topicDetail);
        }
        [HttpPost]
        public IActionResult Edit(TopicDetail topicDetail, IFormFile Video, IFormFile Audio, IFormFile DocumentTopic)
        {
            try
            {
                //courseDetail.CategoryId = 2;
                var detail = new TopicQuery().GetDataTopicById(topicDetail.TopicId);
                // return Ok(courseDetail);
                string uniqueVideo = detail.ViewVideo;
                if (topicDetail.Video != null)
                {
                    uniqueVideo = UploadFileHelper.UploadFile(Video);
                }
                string uniqueAudio = detail.ViewAudio;
                if (topicDetail.Audio != null)
                {
                    uniqueAudio = UploadFileHelper.UploadFile(Audio);
                }
                string uniqueDocument = detail.ViewDocumentTopic;
                if (topicDetail.DocumentTopic != null)
                {
                    uniqueDocument = UploadFileHelper.UploadFile(DocumentTopic);
                }
                bool update = new TopicQuery().UpdateTopicById(
                    topicDetail.NameTopic,
                    topicDetail.CourseId,
                    topicDetail.Description,
                    uniqueVideo,
                    uniqueAudio,
                    uniqueDocument,
                    topicDetail.Status,
                    topicDetail.TopicId);
                if (update)
                {
                    TempData["updateStatus"] = true;
                }
                else
                {
                    TempData["updateStatus"] = false;
                }
                return RedirectToAction(nameof(TopicsController.Index), "Topics");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                // return View(topicDetail);
            }

        }
    }
}
