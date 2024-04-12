using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;

namespace TrainingFPT.Models.Queries
{
    public class CourseQuery
    {
        // HÀM LẤY THÔNG TIN
        public CourseDetail GetDataCouseById(int id = 0)
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
                        courseDetail.Description = reader["Description"].ToString();
                        courseDetail.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                        courseDetail.StartDate = Convert.ToDateTime(reader["StartDate"]);
                        courseDetail.EndDate = Convert.ToDateTime(reader["EndDate"]);
                        courseDetail.ViewImageCourse = reader["Image"].ToString();
                        courseDetail.viewCategoryName = reader["CategoryId"].ToString();
                        courseDetail.Status = reader["Status"].ToString();
                    }
                    connection.Close(); // ngat ket noi
                }
            }
            return courseDetail;
        }

        // LẤY TOÀN BỘ DANH SÁCH VÀ THÔNG TIN CHI TIẾT
        public List<CourseDetail> GetAllDataCourses(string? keyword, string? filter)
        {
            string dataKeyword = "%" + keyword + "%";
            List<CourseDetail> courses = new List<CourseDetail>();
            Dictionary<int, string> categoryNames = new Dictionary<int, string>();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = string.Empty;
                if (filter != null)
                {
                    sqlQuery = "SELECT * FROM [Courses] WHERE [NameCourse] LIKE @keyword AND [DeletedAt] IS NULL AND [Status] = @status";
                }
                else
                {
                    sqlQuery = "SELECT * FROM [Courses] WHERE [NameCourse] LIKE @keyword AND [DeletedAt] IS NULL";
                }

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@keyword", dataKeyword ?? DBNull.Value.ToString());
                if (filter != null)
                {
                    cmd.Parameters.AddWithValue("@status", filter ?? DBNull.Value.ToString());
                }


                string sql = "SELECT [co].*, CONVERT(VARCHAR(50), [co].[StartDate], 101) AS ViewStartDate, CONVERT(VARCHAR(50), [co].[EndDate], 101) AS ViewEndDate, [ca].[Name] FROM [Courses] AS [co] INNER JOIN [Categories] AS [ca] ON [co].[CategoryId] = [ca].[Id] WHERE [co].[DeletedAt] IS NULL";
                connection.Open();
                using (SqlCommand cmdCategories = new SqlCommand("SELECT ID, Name FROM Categories", connection))
                {
                    using (SqlDataReader readerCategories = cmdCategories.ExecuteReader())
                    {
                        while (readerCategories.Read())
                        {
                            categoryNames.Add(Convert.ToInt32(readerCategories["ID"]), readerCategories["Name"].ToString());
                        }
                    }
                }
                connection.Close();
                connection.Open();
                //SqlCommand cmd = new SqlCommand(sql, connection);
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
                        detail.viewCategoryName = reader["CategoryId"].ToString();
                        if (categoryNames.ContainsKey(detail.CategoryId))
                        {
                            detail.NameCategory = categoryNames[detail.CategoryId];
                        }
                        courses.Add(detail);
                    }
                }
                connection.Close();
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
            string valStartTime = startDate.ToString("yyyy-MM-dd HH:mm:ss");
            string valEndate = DBNull.Value.ToString();
            if (endDate != null)
            {
                valEndate = endDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }

            int idCourse = 0;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "INSERT INTO [Courses]([CategoryId], [NameCourse], [Description], [Image], [Status], [StartDate], [EndDate], [CreatedAt]) VALUES(@CategoryId, @NameCourse, @Description, @Image, @Status, @StartDate, @EndDate, @CreatedAt) SELECT SCOPE_IDENTITY()";
                // SELECT SCOPE_IDENTITY() : lay ra ID vua moi them.
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@NameCourse", nameCourse);
                cmd.Parameters.AddWithValue("@Description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Image", imageCourse);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@StartDate", valStartTime);
                cmd.Parameters.AddWithValue("@EndDate", valEndate);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                idCourse = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return idCourse;
        }

        // UPDATE
        public bool UpdateCourseById(
          string nameCourse,
          string description,
          string image,
          int categoryId,
          DateTime startDate,
          DateTime? endDate,
          string status,
          int id
        )
        {
            bool checkUpdate = false;
            string valEndate = DBNull.Value.ToString();
            if (endDate != null)
            {
                valEndate = endDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }

            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlUpdate = "UPDATE [Courses] SET [NameCourse] = @nameCourse, [CategoryId] = @Categories, [Description] = @description, [Image] = @image, [StartDate] = @startDate, [EndDate] = @endDate, [Status] = @status, [UpdatedAt] = @updatedAt WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlUpdate, connection);
                cmd.Parameters.AddWithValue("@nameCourse", nameCourse ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Categories", categoryId);
                cmd.Parameters.AddWithValue("@description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@image", image ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", valEndate);
                cmd.Parameters.AddWithValue("@status", status ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                connection.Close();
                checkUpdate = true;

            }
            return checkUpdate;
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
    }
}