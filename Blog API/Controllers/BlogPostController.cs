using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Blog_API.Models;
using MySqlConnector;

namespace Blog_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        public readonly string ConnectionString = "server=localhost; Database=blog; userid=root; password=";
        [HttpGet]
        public IEnumerable<Blogpost> GetBlogPosts()
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = "SELECT * FROM blogpost";
            var cmd = new MySqlCommand(sql, connector);
            var dr = cmd.ExecuteReader();
            var blogPosts = new List<Blogpost>();
            while (dr.Read())
            {
                var blogPost = new Blogpost
                {
                    Id = dr.GetInt32(0),
                    Title = dr.GetString(1),
                    Content = dr.GetString(2),
                    postTime = dr.GetDateTime(3),
                    updateTime = dr.GetDateTime(4),
                    blogId = dr.GetInt32(5)
                };
                blogPosts.Add(blogPost);
            }
            connector.Close();
            return blogPosts;
        }
        [HttpPost]
        public object AddNewBlogPost(Blogpost blogPost)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = "INSERT INTO blogpost (Title, Content, postTime, updateTime, blogId) VALUES (@title, @content, @posttime, @updatetime, @blogid)";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@title", blogPost.Title);
            cmd.Parameters.AddWithValue("@content", blogPost.Content);
            cmd.Parameters.AddWithValue("@posttime", blogPost.postTime);
            cmd.Parameters.AddWithValue("@updatetime", blogPost.updateTime);
            cmd.Parameters.AddWithValue("@blogid", blogPost.blogId);
            cmd.ExecuteNonQuery();
            connector.Close();
            return blogPost;
        }
        [HttpPut]
        public object UpdateBlogPost(int id, Blogpost blogPost)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = "UPDATE blogpost SET Title=@title, Content=@content, postTime=@posttime, updateTime=@updatetime, blogId=@blogid WHERE Id=@id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@title", blogPost.Title);
            cmd.Parameters.AddWithValue("@content", blogPost.Content);
            cmd.Parameters.AddWithValue("@posttime", blogPost.postTime);
            cmd.Parameters.AddWithValue("@updatetime", blogPost.updateTime);
            cmd.Parameters.AddWithValue("@blogid", blogPost.blogId);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return blogPost;
        }
        [HttpDelete]
        public object DeleteBlogPost(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = "DELETE FROM blogpost WHERE Id=@id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return null;
        }

        [HttpGet("byId")]
        public object GetBloggerById(int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var sql = @"SELECT `name`, `email` FROM `blogger`
                        WHERE `id` = @id;";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();
            datareader.Read();
            var blogger = new
            {
                Name = datareader.GetString(0),
                Email = datareader.GetString(1)
            };

            connector.Close();

            return blogger;
        }

        [HttpGet("bloggerOwnPost")]
        public List<object> GetBloggerWithPost(int id)
        {
            List<object> ownPost = new List<object>();
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var sql = @"SELECT blogger.name, blogpost.title, blogpost.content  
                        FROM `blogger` 
                        INNER JOIN blogpost ON blogger.id = blogpost.blogId
                        WHERE blogger.`id` = @id;";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            while (datareader.Read())
            {
                var bloggerOwnPosts = new
                {
                    Name = datareader.GetString(0),
                    Title = datareader.GetString(1),
                    Content = datareader.GetString(2)
                };

                ownPost.Add(bloggerOwnPosts);
            }

            connector.Close();

            return ownPost;
        }

        [HttpGet("NumberOfPosts")]
        public object GetNumerOfPosts()
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var sql = @"SELECT COUNT(*) FROM blogpost";

            var cmd = new MySqlCommand(sql, connector);

            var db = cmd.ExecuteScalar();

            connector.Close();

            return new { message = $"Posztok száma : {db}" };
        }
        [HttpGet("BloggerPostsNumber")]
        public object GetBloggerPostsNumber(int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var sql = @"SELECT blogger name, COUNT(*)
                        FROM `blogger` 
                        INNER JOIN blogpost ON blogger.id = blogpost.blogId
                        GROUP BY blogger.id
                        HAVING `id` = @id;"; 

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var dr = cmd.ExecuteReader();

            connector.Close();

            return new { message = $"A blogger posztjainak száma : {dr}" };
        }
    }
}
