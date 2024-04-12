using System.ComponentModel.DataAnnotations;
using TrainingFPT.Migrations;
using TrainingFPT.Validation;

namespace TrainingFPT.Models
{
    public class CoursesViewModel
    {
        public List<CourseDetail> CourseDetailList { get; set; }
    }

    public class CourseDetail
    {
        [Key]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Choose Category, please")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Enter Course's name, please")]
        public string NameCourse { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Enter Start date, please")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        [Required(ErrorMessage = "Choose file image, please")]
        [AllowExtensionFile(new string[] { ".png", ".jpg", ".jpeg", "gif" })]
        [AllowMaxSizeFile(5 * 1024 * 1024)]
        public IFormFile Image { get; set; }

        public string? ViewImageCourse { get; set; }
        public int? LikeCourse { get; set; }
        public int? StarCourse { get; set; }

        public string? NameCategory { get; set; }

        [Required(ErrorMessage = "Choose Status, please")]
        public string Status { get; set; }

        public string? viewCategoryName { get; set; } 

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
