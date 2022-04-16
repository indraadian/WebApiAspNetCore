using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebApiFromScract.Data;
using WebApiFromScract.Models;

namespace WebApiFromScract.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BlogController : Controller
    {
        public readonly AppDbContext _context;
        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("api/blogs")]
        public IActionResult Get()
        {
            var blogs = _context.Blogs.ToList();
            if (blogs.Count == 0)
                return Ok(new { message = "No Data" });

            return Ok(blogs);
        }

        [HttpGet("api/blogs/{id}")]
        public IActionResult Get([FromRoute] Guid id)
        {
            var blogs = _context.Blogs.Find(id);
            if (blogs == null)
                return NotFound(new { message = "Blog Not Found" });

            return Ok(blogs);
        }

        [HttpPost("api/blogs")]
        public IActionResult Post([FromBody] Blog blog)
        {
            _context.Add(blog);
            _context.SaveChanges();
            return Ok(blog);
        }

        [HttpPut("api/blogs/{id}")]
        public IActionResult Put([FromRoute] Guid id, [FromBody] Blog blog)
        {
            var currentBlog = _context.Blogs.Find(id);
            if (currentBlog == null)
                return NotFound(new { message = "Blog Not Found" });

            currentBlog.Title = blog.Title;
            currentBlog.Content = blog.Content;

            _context.Blogs.Update(currentBlog);
            _context.SaveChanges();

            return Ok(blog);
        }

        [HttpDelete("api/blogs/{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var blog = _context.Blogs.Find(id);
            if (blog == null) 
                return NotFound(new { message = "Blog Not Found" });
            
            _context.Blogs.Remove(blog);
            _context.SaveChanges();
            return Ok(new {status = "delete blog successes" });
        }
    }
}
