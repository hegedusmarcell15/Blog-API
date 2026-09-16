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
    }
}
