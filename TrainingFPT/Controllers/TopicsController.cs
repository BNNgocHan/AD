using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingFPT.Helpers;
using TrainingFPT.Models;
using TrainingFPT.Models.Queries;

namespace TrainingFPT.Controllers
{
    public class TopicsController : Controller
    {
        [HttpGet]
        public IActionResult Index(string SearchString, string Status)
        {
            TopicsViewModel topicViewModel = new TopicsViewModel();
            topicViewModel.TopicDetailList = new List<TopicDetail>();
            var dataTopics = new TopicQuery().GetAllDataTopics(SearchString, Status);
            foreach (var data in dataTopics)
            {
                topicViewModel.TopicDetailList.Add(new TopicDetail
                {
                    TopicId = data.TopicId,
                    NameTopic = data.NameTopic,
                    CourseId = data.CourseId,
                    NameCourse = data.NameCourse,
                    Description = data.Description,
                    ViewVideo = data.ViewVideo,
                    ViewAudio = data.ViewAudio,
                    ViewDocumentTopic = data.ViewDocumentTopic,
                    Status = data.Status,
                    CreatedAt = data.CreatedAt,
                    UpdatedAt = data.UpdatedAt,
                    viewCourseName = data.viewCourseName
                });
            }
            ViewData["keyword"] = SearchString;
            ViewBag.Status = Status;
            return View(topicViewModel);
        }

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
            string videoTopic = "Null";
            string audioTopic = "Null";
            string documentTopic = "Null";
            if (Video == null && Audio == null && DocumentTopic == null)
            {
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
            if (Video == null && Audio == null && DocumentTopic == null && topic.NameTopic == null)
            {
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
            if (Video == null)
            {
                ModelState.Remove("Video");
            }
            else
            {
                videoTopic = UploadFileHelper.UploadFile(Video);
            }
            if (Audio == null)
            {
                ModelState.Remove("Audio");
            }
            else
            {
                audioTopic = UploadFileHelper.UploadFile(Audio);
            }

            if (DocumentTopic == null)
            {
                ModelState.Remove("DocumentTopic");
            }
            else
            {
                documentTopic = UploadFileHelper.UploadFile(DocumentTopic);
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                    //neu co loi
                    return Ok(ex.Message);
                }
            }

            if (Video == null && Audio == null && DocumentTopic == null)
            {
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

        [HttpGet]
        public IActionResult Edit(int id = 0)
        {
            TopicDetail detail = new TopicQuery().GetDataTopicById(id);
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
            return View(detail);
        }
        [HttpPost]
        public IActionResult Edit(TopicDetail topicDetail, IFormFile Video, IFormFile Audio, IFormFile DocumentTopic)
        {
            string videoTopic = "Null";
            string audioTopic = "Null";
            string documentTopic = "Null";
            
            if (Video == null)
            {
                ModelState.Remove("Video");
            }
            else
            {
                videoTopic = UploadFileHelper.UploadFile(Video);
            }
            if (Audio == null)
            {
                ModelState.Remove("Audio");
            }
            else
            {
                audioTopic = UploadFileHelper.UploadFile(Audio);
            }

            if (DocumentTopic == null)
            {
                ModelState.Remove("DocumentTopic");
            }
            else
            {
                documentTopic = UploadFileHelper.UploadFile(DocumentTopic);
            }
            if (ModelState.IsValid)
            {
                var detail = new TopicQuery().GetDataTopicById(topicDetail.TopicId);
                videoTopic = detail.ViewVideo;
                audioTopic = detail.ViewAudio;
                documentTopic = detail.ViewDocumentTopic;
                // kiem tra xe nguoi co muon thay doi anh ko?

                bool update = new TopicQuery().UpdateTopicById(
                    topicDetail.NameTopic,
                    topicDetail.CourseId,
                    topicDetail.Description,
                    videoTopic,
                    audioTopic,
                    documentTopic,
                    topicDetail.Status,
                    topicDetail.TopicId
                );
                
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
    }
}