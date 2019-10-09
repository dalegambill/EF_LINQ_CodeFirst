using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using EF_Tables;

namespace EF_LINQ_CodeFirst
{
    public partial class EF_LINQ_WinForm : Form
    {
         BloggingContext bloggingContext = new BloggingContext();

        public EF_LINQ_WinForm()
        {
            InitializeComponent();
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            this.Text = "EF_LINQ_CodeFirst v" + version;
        }

        private void BtnAddBlog_Click(object sender, EventArgs e)
        {
            SetWorkingState();
            if ( tbBlogName.Text == "" )
            {
                tbBlogsInfo.Text = "Blog Name cannot be blank\r\n";
                SetIdleState();
                return;
            }
            Blog blog = new Blog();
            blog.Name = tbBlogName.Text;
            var queryBlog = from aBlog in bloggingContext.Blogs
                            where aBlog.Name == blog.Name
                            select aBlog;
            bool foundBlogName = false;
            foreach (var aBlog in queryBlog)
            {
                blog.BlogId = aBlog.BlogId;
                foundBlogName = true;
                break;
            }
            if (foundBlogName)
            {
                tbBlogsInfo.Text += string.Format("Blog Name '{0}' already exists: ", blog.Name);
            }
            else
            {
                blog.BlogId = Guid.NewGuid();
                try
                {
                    bloggingContext.Blogs.Add(blog);
                    bloggingContext.SaveChanges();
                    tbBlogsInfo.Text += string.Format("New Blog:    Name: {0}    ID: {1}\r\n", blog.Name, blog.BlogId.ToString());
                    ScrollUpBlogsInfo();
                }
                catch (Exception error)
                {
                    tbBlogsInfo.Text = error.Message;
                }
            }
            SetIdleState();
        }

        private void BtnListBlogs_Click(object sender, EventArgs e)
        {
            SetWorkingState();           
            var queryBlogs = from aBlog in bloggingContext.Blogs
                            orderby aBlog.Name
                            select aBlog;

            // Display all Blogs from the database
            foreach (var aBlog in queryBlogs)
            {
                tbBlogsInfo.Text += "BlogId: " + aBlog.BlogId.ToString() + "    Name: " + aBlog.Name + "\r\n";
                ScrollUpBlogsInfo();
            }
            SetIdleState();
        }

        private void BtnAddPost_Click(object sender, EventArgs e)
        {
            SetWorkingState();
            if (tbBlogName.Text == "")
            {
                tbPostsInfo.Text = "Blog Name cannot be blank.\r\n";
                SetIdleState();
                return;
            }
            if (tbPostTitle.Text == "")
            {
                tbPostsInfo.Text = "Post Title cannot be blank.\r\n";
                SetIdleState();
                return;
            }
            if (tbPostContent.Text == "")
            {
                tbPostsInfo.Text = "Post Content cannot be blank.\r\n";
                SetIdleState();
                return;
            }
            // get Blog ID for the post
            Blog blog = new Blog();
            blog.Name = tbBlogName.Text;
            var queryBlog = from aBlog in bloggingContext.Blogs
                            where aBlog.Name == blog.Name
                            select aBlog;
            bool foundBlogName = false;
            foreach (var aBlog in queryBlog)
            {
                blog.BlogId = aBlog.BlogId;
                foundBlogName = true;
                break;
            }
            if (!foundBlogName)
            {
                tbPostsInfo.Text = "The Blog name entered does not exist.\r\n";
            }
            else
            {
                Post post = new Post();
                post.BlogId = blog.BlogId;
                post.PostId = Guid.NewGuid();
                post.Title = tbPostTitle.Text;
                post.Content = tbPostContent.Text;
                post.BlogId = blog.BlogId;
                try
                {
                    bloggingContext.Posts.Add(post);
                    bloggingContext.SaveChanges();
                    tbPostsInfo.Text += string.Format("New Post:    ID: {0}   Title: {1}    Content: {2}    Blog ID: {3}\r\n",
                                                       post.PostId.ToString(), post.Title, post.Content, post.BlogId.ToString());
                    ScrollUpPostsInfo();
                }
                catch ( Exception error )
                {
                    tbPostsInfo.Text = error.Message;
                }
            }
            SetIdleState();
        }

        private void BtnListPosts_Click(object sender, EventArgs e)
        {
            SetWorkingState();
            var queryPosts = from aPost in bloggingContext.Posts
                            orderby aPost.Title
                            select aPost;

            // Display all Blogs from the database
            foreach (var aPost in queryPosts)
            {
                tbPostsInfo.Text += "PostId: " + aPost.PostId.ToString() + "    Title: " + aPost.Title + "    Content: " + aPost.Content  + 
                                    "    Blog ID: " + aPost.BlogId.ToString() +  "\r\n";
                ScrollUpPostsInfo();
            }
            SetIdleState();
        }

        void ScrollUpBlogsInfo()
        {
            tbBlogsInfo.SelectionStart = tbBlogsInfo.Text.Length;
            tbBlogsInfo.ScrollToCaret();
        }

        void ScrollUpPostsInfo()
        {
            tbPostsInfo.SelectionStart = tbPostsInfo.Text.Length;
            tbPostsInfo.ScrollToCaret();
        }

        private void BtnClearBlogsInfo_Click(object sender, EventArgs e)
        {
            tbBlogsInfo.Clear();
        }

        private void BtnClearPostsInfo_Click(object sender, EventArgs e)
        {
            tbPostsInfo.Clear();
        }

        private void btnUpdatePost_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbPostContent.Text) || String.IsNullOrEmpty(tbPostTitle.Text))
            {
                tbPostsInfo.Text += "'Post Title' and 'Post Content' cannot be empty.\r\n";
                ScrollUpPostsInfo();
                return;
            }

            try
            {
                var postToUpdate = bloggingContext.Posts.First(a => a.Title == tbPostTitle.Text);
                if (postToUpdate != null)
                {
                    tbPostsInfo.Text += "Title: " + postToUpdate.Title + "    Post ID: " + postToUpdate.PostId + "\r\n";
                    ScrollUpPostsInfo();
                    tbPostsInfo.Text += "Content: " + postToUpdate.Content + "    Changed to: " + tbPostContent.Text + "\r\n";
                    ScrollUpPostsInfo();
                    postToUpdate.Content = tbPostContent.Text;
                    bloggingContext.SaveChanges();
                }
            }
            catch( Exception exception )
            {
                tbPostsInfo.Text += "Error: " + exception.Message + "\r\n";
                ScrollUpPostsInfo();
            }
        }

        private void btnDeletePost_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbPostTitle.Text))
            {
                tbPostsInfo.Text += "'Post Title' cannot be empty.\r\n";
                ScrollUpPostsInfo();
                return;
            }

            try
            {
                var postToDelete = bloggingContext.Posts.Where(p => p.Title == tbPostTitle.Text).FirstOrDefault();
                if (postToDelete != null)
                {
                    bloggingContext.Posts.Remove(postToDelete);
                    bloggingContext.SaveChanges();
                    tbPostsInfo.Text += "Record deleted:    " + "Title: " + postToDelete.Title + "    Post ID: " + postToDelete.PostId + "\r\n";
                    ScrollUpPostsInfo();
                }
            }
            catch( Exception exception )
            {
                tbPostsInfo.Text += "Error: " + exception.Message + "\r\n";
                ScrollUpPostsInfo();
            }
        }
        private void SetWorkingState()
        {
            laStatus.ForeColor = System.Drawing.Color.Red;
            laStatus.Text = "Working";
            laStatus.Refresh();
            btnAddBlog.Enabled = false;
            btnListBlogs.Enabled = false;
            btnAddPost.Enabled = false;
            btnListPosts.Enabled = false;
            //btnUpdateBlog.Enabled = false;
            //btnDeleteBlog.Enabled = false;
            btnUpdatePost.Enabled = false;
            btnDeletePost.Enabled = false;
        }
        private void SetIdleState()
        {
            laStatus.ForeColor = System.Drawing.Color.Black;
            laStatus.Text = "Idle";
            laStatus.Refresh();
            btnAddBlog.Enabled = true;
            btnListBlogs.Enabled = true;
            btnAddPost.Enabled = true;
            btnListPosts.Enabled = true;
            //btnUpdateBlog.Enabled = true;
            //btnDeleteBlog.Enabled = true;
            btnUpdatePost.Enabled = true;
            btnDeletePost.Enabled = true;
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show( "This is a C# working code tutorial. It uses Entity Framework 6.2, LINQ, and code-first to create " +
                             "a database in SQL Server 2017. It was developed using Visual Studio Community 2019." +
                             "The entire project is at https://github.com/dalegambill/EF_LINQ_CodeFirst.", "Working Code Tutorials" );

        }

        private void btnVideo_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCouhHzMMU9c-Qh-TkZl5GDg");
        }
    }
}
