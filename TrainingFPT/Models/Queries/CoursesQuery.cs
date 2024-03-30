using Microsoft.Data.SqlClient;
using TrainingFPT.Migrations;

namespace TrainingFPT.Models.Queries
{
    public class CoursesQuery
    {
        public List<CourseDetail> GetAllDateCourses()
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
                }
                connection.Close();
            }
            return courses;
        }

        // INSERT
        public int InsetDataCourse(
            string nameCourse,
            int CategoryId,
            string? description,
            DateTime startDate,
            DateTime? endDate,
            string status,
            string imageCourse
        )
        {
            string valEnddate = DBNull.Value.ToString();
            if (valEnddate != null)
            {
                valEnddate = endDate.ToString();
            }

            int idCourse = 0;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "INSERT INTO [Courses]([CategoryId], [NameCourse], [Description], [Image], [Status], [StartDate], [EndDate], [CreatedAt]) VALUES(@CategoryId, @NameCourse, @Description, @Image, @Status, @StartDate, @EndDate, @CreatedAt) SELECT SCOPE_IDENTITY()";
                //SELECT SCOPE_IDENTITY(): lấy ra ID vừa thêm
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
                cmd.Parameters.AddWithValue("@NameCourse", nameCourse);
                cmd.Parameters.AddWithValue("@Description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("Image", imageCourse);
                cmd.Parameters.AddWithValue("Status", status);
                cmd.Parameters.AddWithValue("StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", valEnddate);
                cmd.Parameters.AddWithValue("CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                idCourse = Convert.ToInt32(cmd.ExecuteScalar());
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

        // HÀM LẤY THÔNG TIN CHI TIẾT CỦA COURSE
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
                        courseDetail.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                        courseDetail.NameCourse = reader["NameCourse"].ToString();
                        courseDetail.Description = reader["Description"].ToString();
                        courseDetail.ViewImageCourse = reader["Image"].ToString();
                        courseDetail.Status = reader["Status"].ToString();
                        courseDetail.StartDate = Convert.ToDateTime(reader["StartDate"]);
                        courseDetail.EndDate = Convert.ToDateTime(reader["EndDate"]);
                    }
                    connection.Close(); //ngắt kết nối
                }
            }
            return courseDetail;
        }

        //UPDATE
        public bool UpdateCourseById
        (
            string nameCourse,
            int CategoryId,
            string? description,
            DateTime startDate,
            DateTime? endDate,
            string status,
            string imageCourse,
            int CourseId
        )
        {
            bool checkUpdate = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlUpdate = "UPDATE [Courses] SET [CategoryId] = @categoryId, [NameCourse] = @nameCourse, [Description] = @description, [Image] = @image, [Status] = @status, [StartDate] = @startDate, [EndDate] = @endDate, [UpdatedAt] = @updatedAt WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlUpdate, connection);
                cmd.Parameters.AddWithValue("@categoryId", CategoryId);
                cmd.Parameters.AddWithValue("@nameCourse", nameCourse ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@image", imageCourse ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@status", status ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@id", CourseId);
                cmd.ExecuteNonQuery();
                connection.Close();
                checkUpdate = true;
            }
            return checkUpdate;
        }
    }
}
