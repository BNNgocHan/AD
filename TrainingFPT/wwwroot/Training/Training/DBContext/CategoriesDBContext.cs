using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Training.DBContext
{
    public class CategoriesDBContext
    {
        //tạo bảng Categories trong DB TestTraining
        [Key]
        public int Id { get; set; }

        [Column("NameCategory", TypeName = "Varchar(50)"), Required]
        public required string NameCategory { get; set; }

        [Column("Description", TypeName = "Varchar(200)"), AllowNull]
        public string? Description { get; set; }

        [Column("PosterImage", TypeName = "Varchar(200)"), Required]
        public required string PosterImage { get; set; }

        [Column("ParentID", TypeName = "integer"), Required]
        public required string ParentID { get; set; }

        [Column("Status", TypeName = "Varchar(20)"), Required]
        public required string RoleName { get; set; }

        [AllowNull]
        public DateTime? CreatedAt { get; set; }

        [AllowNull]
        public DateTime? UpdatedAt { get; set; }

        [AllowNull]
        public DateTime? LastUpdatedAt { get; set; }
    }
}