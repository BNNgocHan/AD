using System.ComponentModel.DataAnnotations;
using TrainingFPT.Migrations;
using TrainingFPT.Validation;

namespace TrainingFPT.Models
{
    public class TopicsViewModel
    {
        public List<TopicDetail> TopicDetailList { get; set; }
    }
    public class TopicDetail
    {
        [Key]
        public int TopicId { get; set; }
        [Required(ErrorMessage = "Enter Topic's name, please")]
        public string NameTopic { get; set; }

        [Required(ErrorMessage = "Choose Course, please")]
        public int CourseId { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Choose video, please")]
        [AllowExtensionFile(new string[] { ".mp4", ".avi", ".mov", ".mkv", ".wmv", ".flv", ".mpeg" })]
        [AllowMaxSizeFile(500 * 1024 * 1024)]
        public IFormFile Video { get; set; }
        public string? ViewVideo { get; set; }

        [Required(ErrorMessage = "Choose audio, please")]
        [AllowExtensionFile(new string[] { ".mp3", ".wav", ".ogg", ".flac", ".aac", ".m4a", ".wma" })]
        [AllowMaxSizeFile(500 * 1024 * 1024)]
        public IFormFile Audio { get; set; }
        public string? ViewAudio { get; set; }

        [Required(ErrorMessage = "Choose file, please")]
        [AllowExtensionFile(new string[] { "docx", ".pdf" })]
        [AllowMaxSizeFile(5 * 1024 * 1024)]
        public IFormFile DocumentTopic { get; set; }
        public string? ViewDocumentTopic { get; set; }

        public int? LikeTopic { get; set; }
        public int? StarTopic { get; set; }
        public string? NameCourse { get; set; }

        [Required(ErrorMessage = "Choose Status, please")]
        public string Status { get; set; }
        public string? viewCourseName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
