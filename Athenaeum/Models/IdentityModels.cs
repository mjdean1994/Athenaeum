using System.Collections.Generic;
using Athenaeum.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Athenaeum.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Characters = new List<Character>();
            Guilds = new List<Guild>();
            Posts = new List<ForumPost>();
            Compositions = new List<Composition>();
            Pictures = new List<Picture>();
            Events = new List<Event>();
            Rsvps = new List<Rsvp>();
        }

        public string BattleTag { get; set; }
        public string Introduction { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Character> Characters { get; set; }
        public virtual ICollection<Guild> Guilds { get; set; }
        public virtual ICollection<ForumPost> Posts { get; set; }
        public virtual ICollection<Composition> Compositions { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; } 
        public virtual ICollection<Event> Events { get; set; } 

        public virtual ICollection<Rsvp> Rsvps { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<Athenaeum.Models.NewsCategory> NewsCategories { get; set; }
        public System.Data.Entity.DbSet<Athenaeum.Models.NewsArticle> NewsArticles { get; set; }
        public System.Data.Entity.DbSet<Athenaeum.Models.Comment> Comments { get; set; }
        public System.Data.Entity.DbSet<Athenaeum.Models.CommentType> CommentTypes { get; set; }
        public System.Data.Entity.DbSet<Athenaeum.Models.Character> Characters { get; set; }
        public System.Data.Entity.DbSet<Athenaeum.Models.Guild> Guilds { get; set; }
        public System.Data.Entity.DbSet<Athenaeum.Models.ForumCategory> ForumCategories { get; set; }
        public System.Data.Entity.DbSet<Athenaeum.Models.ForumSection> ForumSections { get; set; }
        public System.Data.Entity.DbSet<Athenaeum.Models.ForumThread> ForumThreads { get; set; }
        public System.Data.Entity.DbSet<Athenaeum.Models.ForumPost> ForumPosts { get; set; }
        public System.Data.Entity.DbSet<Athenaeum.Models.Composition> Compositions { get; set; }
        public System.Data.Entity.DbSet<Athenaeum.Models.Picture> Pictures { get; set; }
        public System.Data.Entity.DbSet<Athenaeum.Models.Event> Events { get; set; }
        public System.Data.Entity.DbSet<Athenaeum.Models.ContactMessage> ContactMessages { get; set; }
        public System.Data.Entity.DbSet<Rsvp> Rsvps { get; set; }
    }
}