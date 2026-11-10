using HearUS.Application.Common.Interfaces;
using HearUS.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HearUS.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts => Set<Post>();

        public DbSet<Comment> Comments => Set<Comment>();

        public DbSet<PostLike> PostLikes => Set<PostLike>();

        public DbSet<Insight> Insights => Set<Insight>();

        public DbSet<Job> Jobs => Set<Job>();

        public DbSet<ChatConversation> ChatConversations => Set<ChatConversation>();

        public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

        public DbSet<SignTerm> SignTerms => Set<SignTerm>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
