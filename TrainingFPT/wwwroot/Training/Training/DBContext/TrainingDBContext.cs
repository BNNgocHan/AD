using Microsoft.EntityFrameworkCore;

namespace Training.DBContext
{
    public class TrainingDBContext : DbContext
    {
        public TrainingDBContext(DbContextOptions<TrainingDBContext> options): base (options) { }

        //DBSet : tạo bảng dữ liệu từ RolesDBContext
        //Roles: tên bảng được tạo
        public DbSet<RolesDBContext> Roles { get; set;}
        public DbSet<CategoriesDBContext> Categories { get; set;}
        public DbSet<CourseDBContext> Courses { get; set;}
    }
}
