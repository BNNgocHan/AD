using Microsoft.Data.SqlClient;
using TrainingFPT.Migrations;

namespace TrainingFPT.Models.Queries
{
    public class CourseQuery
    {
        // HÀM LẤY DANH SÁCH KHÓA HỌC VÀ THÔNG TIN CHI TIẾT
        public List<CourseDetail> GetAllDataCourses()
        {
            List<CourseDetail> courses = new List<CourseDetail>(); 
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "SELECT [co].*, [ca].[Name] FROM [Courses] AS [co] INNER JOIN [Categories] AS [ca] ON [co].[CategoryId] = [ca].[Id] WHERE [co].[DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CourseDetail detail = new CourseDetail();
                        detail.CourseId = Convert.ToInt32(reader["Id"]);
                        detail.NameCourse = reader["NameCourse"].ToString();
                        detail.Description = reader["Description"].ToString();
                        detail.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                        detail.StartDate = Convert.ToDateTime(reader["StartDate"]);
                        detail.EndDate = Convert.ToDateTime(reader["EndDate"]);
                        detail.ViewImageCourse = reader["Image"].ToString();
                        detail.Status = reader["Status"].ToString();
                        detail.viewCategoryName = reader["Name"].ToString();
                        courses.Add(detail);
                    }
                    connection.Close();
                }
                
            }
            return courses;
        }
        
        // INSERT
        public int InsertDataCourse(
            string nameCourse,
            int categoryId,
            string? description,
            DateTime startDate,
            DateTime? endDate,
            string status,
            string imageCourse
        )
        {
            string valEnddate = DBNull.Value.ToString();
            if ( valEnddate != null )
            {
                valEnddate = endDate.ToString();
            }

            int idCourse = 0;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "INSERT INTO [Courses]([CategoryId], [NameCourse], [Description], [Image], [Status], [StartDate], [EndDate], [CreatedAt]) VALUES(@CategoryId, @NameCourse, @Description, @Image, @Status, @StartDate, @EndDate, @CreatedAt) SELECT SCOPE_IDENTITY()";
                //SELECT SCOPE_IDENTITY(): lấy ra ID vừa thêm
                connection.Open();
                SqlCommand cmd = new SqlCommand( sqlQuery, connection );
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@NameCourse", nameCourse);
                cmd.Parameters.AddWithValue("@Description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("Image", imageCourse);
                cmd.Parameters.AddWithValue("Status", status);
                cmd.Parameters.AddWithValue("StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", valEnddate);
                cmd.Parameters.AddWithValue("CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                idCourse = Convert.ToInt32( cmd.ExecuteScalar() );
                connection.Close();
            }
            return idCourse;
        }

        // DELETE
        public bool DeleteItemCourse(int id = 0)
        {
            bool statusDelete = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "UPDATE[Courses] SET [DeletedAt] = @deletedAt WHERE [Id] = @id";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@deletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                statusDelete = true;
                connection.Close();
            }
            //false: kh xoa dc - true: xoa thanh cong
            return statusDelete;
        }

        // UPDATE
        public bool UpdateCourseById(
            string nameCourse,
            int categoryId,
            string description,
            DateTime startDate,
            DateTime? endDate,
            string viewImageCourse,
            string status,
            int courseId
        )
        {
            bool checkUpdate = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlUpdate = "UPDATE[Courses] SET[NameCourse] = @NameCourse, [CategoryId] = @CategoryId, [Description] = @Description, [StartDate] = @StartDate, [EndDate] = @EndDate, [Image] = @Image, [Status] = @Status, [UpdatedAt] = @updatedAt WHERE[Id] = @CourseId AND[DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlUpdate, connection);
                cmd.Parameters.AddWithValue("@NameCourse", nameCourse ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@Description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@StartDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Status", status ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Image", viewImageCourse ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@CourseId", courseId);

                cmd.ExecuteNonQuery();
                connection.Close();
                checkUpdate = true;
            }
            return checkUpdate;
        }

        // HÀM LẤY THÔNG TIN CHI TIẾT CỦA CATEGORY
        public CourseDetail GetDataCourseById(int id = 0)
        {
            CourseDetail courseDetail = new CourseDetail();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [Courses] WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courseDetail.CourseId = Convert.ToInt32(reader["Id"]);
                        courseDetail.NameCourse = reader["NameCourse"].ToString();
                        courseDetail.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                        courseDetail.Description = reader["Description"].ToString();
                        courseDetail.StartDate = Convert.ToDateTime(reader["StartDate"]);
                        courseDetail.EndDate = reader["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["EndDate"]);
                        courseDetail.ViewImageCourse = reader["Image"].ToString();
                        courseDetail.Status = reader["Status"].ToString();
                    }
                }
                connection.Close(); //ngắt kết nối
            }
            return courseDetail;
        }
    }
}
