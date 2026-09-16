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

    }
}
