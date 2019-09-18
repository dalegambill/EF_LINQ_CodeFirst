using System;
using System.Data.Entity;
using System.Collections.Generic;

namespace EF_Tables
{
    public class Blog
    {
        public Guid BlogId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }

        public class Post
    {
        public Guid PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }

    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}

