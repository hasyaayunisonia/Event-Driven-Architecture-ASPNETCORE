using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Entities
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        //public int UserId { get; set; }
        public int? UserId { get; set; } // Nullable foreign key
        public User User { get; set; }
    }
}
