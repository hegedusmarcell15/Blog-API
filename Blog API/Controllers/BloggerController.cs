using Blog_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using MySqlConnector;
using Blog_API.Models.DTOs;

namespace Blog_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        public readonly string ConnectionString = "server=localhost:3306; Database=blog; userid=root; password=";
        [HttpGet]
        public List<Blogger> GetAllBlogger()
        {
            List<Blogger> bloggers = new();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = $"SELECT * FROM blogger;";


            var cmd = new MySqlCommand(sql, connector);
            var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var blogger = new Blogger
                {
                    Id = dr.GetInt32(0),
                    Name = dr.GetString(1),
                    Email = dr.GetString(2),
                    Age = dr.GetInt32(3),
                    Password = dr.GetString(4),
                    RegistrationTime = dr.GetDateTime(5),
                };


            }

            connector.Close();
            return null;
        }
        [HttpPost]
        public object AddNewBlogger(Blogger blogger)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var blg = new Blogger
            {
                Name = blogger.Name,
                Email = blogger.Email,
                Age = blogger.Age,
                Password = blogger.Password,
                RegistrationTime = DateTime.Now,
            };
            var sql = $"INSERT INTO `blogger`(`Name`, `Email`, `Age`, `Password`, `RegistrationTime`) VALUES (@name,@email,@age,@password,@registrationtime)";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", blg.Name);
            cmd.Parameters.AddWithValue("@email", blg.Email);
            cmd.Parameters.AddWithValue("@age", blg.Age);
            cmd.Parameters.AddWithValue("@password", blg.Password);
            cmd.Parameters.AddWithValue("@registrationtime", blg.RegistrationTime);
            
            cmd.ExecuteNonQuery();
            connector.Close();
            return blg;
        }
        [HttpPut]
        public object UpdateBlogger([FromQuery]int id,[FromBody]UpdateBloggerDto updateBloggerDto)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = $"UPDATE `blogger` SET `Name`=@name,`Email`=@email,`Age`=@age,`Password`=@password WHERE id = @id";
            
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", updateBloggerDto.Name);
            cmd.Parameters.AddWithValue("@email", updateBloggerDto.Email);
            cmd.Parameters.AddWithValue("@age", updateBloggerDto.Age);
            cmd.Parameters.AddWithValue("@password", updateBloggerDto.Password);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
            connector.Close();
            return updateBloggerDto;
        }
        [HttpDelete]
        public object DeleteBlogger(int id, Blogger blogger)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = $"DELETE FROM `blogger` WHERE id = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return null;
        }
    }
}
