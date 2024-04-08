using Microsoft.Data.SqlClient;

namespace TrainingFPT.Models.Queries
{
    public class TopicQuery
    {
        // LẤY TOÀN BỘ DANH SÁCH VÀ THÔNG TIN CHI TIẾT
        public List<TopicDetail> GetAllDataTopics(string? keyword, string? filter)
        {
            string dataKeyword = "%" + keyword + "%";
            List<TopicDetail> topics = new List<TopicDetail>();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = string.Empty;
                if (filter != null)
                {
                    sqlQuery = "SELECT * FROM [Topics] WHERE [NameTopic] LIKE @keyword AND [DeletedAt] IS NULL AND [Status] = @status";
                }
                else
                {
                    sqlQuery = "SELECT * FROM [Topics] WHERE [NameTopic] LIKE @keyword AND [DeletedAt] IS NULL";
                }

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@keyword", dataKeyword ?? DBNull.Value.ToString());
                if (filter != null)
                {
                    cmd.Parameters.AddWithValue("@status", filter ?? DBNull.Value.ToString());
                }


                string sql = "SELECT [to].*, [co].[NameCourse] FROM [Topics] AS [to] INNER JOIN [Courses] AS [co] ON [to].[CourseId] = [co].[Id] WHERE [co].[DeletedAt] IS NULL";
                connection.Open();
                //SqlCommand cmd = new SqlCommand(sql, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TopicDetail detail = new TopicDetail();
                        detail.TopicId = Convert.ToInt32(reader["Id"]);
                        detail.NameTopic = reader["NameTopic"].ToString();
                        detail.CourseId = Convert.ToInt32(reader["CourseId"]);
                        detail.Description = reader["Description"].ToString();
                        detail.ViewDocumentTopic = reader["DocumentTopic"].ToString();
                        detail.Status = reader["Status"].ToString();
                        detail.ViewVideo = reader["Video"].ToString();
                        detail.ViewAudio = reader["Audio"].ToString();
                        detail.viewCourseName = reader["CourseId"].ToString();
                        topics.Add(detail);
                    }
                }
                connection.Close();
            }
            return topics;
        }

        // HÀM LẤY THÔNG TIN
        public TopicDetail GetDataTopicById(int id = 0)
        {
            TopicDetail topicDetail = new TopicDetail();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [Topics] WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        topicDetail.TopicId = Convert.ToInt32(reader["Id"]);
                        topicDetail.NameTopic = reader["NameTopic"].ToString();
                        topicDetail.CourseId = Convert.ToInt32(reader["CourseId"]);
                        topicDetail.Description = reader["Description"].ToString();
                        topicDetail.ViewVideo = reader["Video"].ToString();
                        topicDetail.ViewAudio = reader["Audio"].ToString();
                        topicDetail.ViewDocumentTopic = reader["DocumentTopic"].ToString();
                        topicDetail.viewCourseName = reader["CourseId"].ToString();
                        topicDetail.Status = reader["Status"].ToString();
                    }
                    connection.Close(); // ngat ket noi
                }
            }
            return topicDetail;
        }

        // INSERT
        public int InsertDataTopic(
            string nameTopic,
            int courseId,
            string? description,
            string? videoTopic,
            string? audioTopic,
            string? documentTopic,
            string status
        )
        {
            int idTopic = 0;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "INSERT INTO [Topics]([NameTopic], [CourseId], [Description], [Video], [Audio], [DocumentTopic], [Status], [CreatedAt]) VALUES(@NameTopic, @CourseId, @Description, @Video, @Audio, @DocumentTopic, @Status, @CreatedAt) SELECT SCOPE_IDENTITY()";
                // SELECT SCOPE_IDENTITY() : lay ra ID vua moi them.
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@NameTopic", nameTopic);
                cmd.Parameters.AddWithValue("@CourseId", courseId);
                cmd.Parameters.AddWithValue("@Description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Video", videoTopic ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Audio", audioTopic ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@DocumentTopic", documentTopic ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                idTopic = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return idTopic;
        }

        // DELETE
        public bool DeleteItemTopic(int id = 0)
        {
            bool statusDelete = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "UPDATE[Topics] SET [DeletedAt] = @deletedAt WHERE [Id] = @id";
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
        public bool UpdateTopicById(
            string nameTopic,
            int courseId,
            string? description,
            string? videoTopic,
            string? audioTopic,
            string? documentTopic,
            string status,
            int id
        )
        {
            bool checkUpdate = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlUpdate = "UPDATE [Topics] SET [NameTopic] = @nameTopic, [CourseId] = @Courses, [Description] = @description, [Video] = @video, [Audio] = @audio, [DocumentTopic] = @documentTopic, [Status] = @status, [UpdatedAt] = @updatedAt WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlUpdate, connection);
                cmd.Parameters.AddWithValue("@nameTopic", nameTopic ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Courses", courseId);
                cmd.Parameters.AddWithValue("@description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@video", videoTopic ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@audio", audioTopic ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@documentTopic", documentTopic ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@status", status ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                connection.Close();
                checkUpdate = true;
            }
            return checkUpdate;
        }
    }
}
