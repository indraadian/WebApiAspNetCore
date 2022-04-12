using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiFromScract.Models
{
    public class Blog
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid(); 
        public string Title { get; set; }
        public string Content { get; set; }

        public Blog(string title, string content)
        {
            this.Title = title;
            this.Content = content;
        }
    }
}
