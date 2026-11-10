using HearUS.Domain.Constants;
using HearUS.Domain.Entities;
using HearUS.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HearUS.Infrastructure.Persistence.Seed
{
    public static class DbSeeder
    {
        private static readonly (string Email, string FullName, string Password, string Role)[] SeedUsers =
        {
            ("admin@hearus.local", "مدير المنصة", "Admin@12345", AppRoles.Admin),
            ("expert@hearus.local", "سارة صاحبة تجربة", "Expert@12345", AppRoles.Expert),
            ("member@hearus.local", "أحمد عضو المجتمع", "Member@12345", AppRoles.Member)
        };

        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var environment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (environment.IsDevelopment())
            {
                await context.Database.MigrateAsync();
            }

            foreach (var role in AppRoles.All)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            foreach (var (email, fullName, password, role) in SeedUsers)
            {
                await EnsureUserAsync(userManager, email, fullName, password, role);
            }

            await SeedContentAsync(context, userManager);
        }

        private static async Task SeedContentAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            var admin = await userManager.FindByEmailAsync("admin@hearus.local");
            var expert = await userManager.FindByEmailAsync("expert@hearus.local");
            var member = await userManager.FindByEmailAsync("member@hearus.local");

            if (admin is null || expert is null || member is null)
            {
                return;
            }

            var now = DateTime.UtcNow;

            if (!await context.Jobs.AnyAsync())
            {
                context.Jobs.AddRange(
                    new Job
                    {
                        Title = "مطوّر ويب (عمل عن بُعد)",
                        CompanyName = "شركة أفق التقنية",
                        Description = "نبحث عن مطوّر ويب للعمل عن بُعد بدوام كامل. نوفّر بيئة عمل داعمة ومرنة، وأدوات مساعدة تقنية، وجدولاً مرناً يراعي الظروف الصحية.",
                        Location = "عن بُعد",
                        IsRemote = true,
                        IsAccessibilityFriendly = true,
                        ContactEmail = "jobs@ufuq.example",
                        PostedAt = now.AddDays(-3)
                    },
                    new Job
                    {
                        Title = "أخصائي دعم فني",
                        CompanyName = "مؤسسة خطوة",
                        Description = "مطلوب أخصائي دعم فني للرد على استفسارات المستخدمين عبر القنوات المكتوبة. المكان مهيأ بالكامل للكراسي المتحركة، مع ساعات عمل مرنة.",
                        Location = "الرياض",
                        IsRemote = false,
                        IsAccessibilityFriendly = true,
                        ApplyUrl = "https://example.com/careers/support",
                        PostedAt = now.AddDays(-8)
                    },
                    new Job
                    {
                        Title = "مصمم جرافيك",
                        CompanyName = "استوديو بيان",
                        Description = "فرصة تصميم إبداعية بدوام جزئي، مناسبة لمن يفضل العمل من المنزل مع اجتماعات أسبوعية قصيرة.",
                        Location = "عن بُعد",
                        IsRemote = true,
                        IsAccessibilityFriendly = true,
                        ContactEmail = "design@bayan.example",
                        PostedAt = now.AddDays(-14)
                    });
            }

            if (!await context.Insights.AnyAsync())
            {
                context.Insights.AddRange(
                    new Insight
                    {
                        AuthorId = expert.Id,
                        Title = "كيف بدأت رحلتي مع التقنية من داخل غرفتي",
                        Summary = "تجربة شخصية عن تحويل العزلة إلى فرصة للتعلّم والعمل عن بُعد.",
                        Content = "لم تكن البداية سهلة. كنت أقضي ساعات طويلة أمام الحاسوب أتعلم البرمجة من مصادر مجانية، وأواجه صعوبات في الوصول لبعض الأدوات.\r\n\r\nالشيء الذي غيّر مساري هو إيماني بأن التقنية هي اللغة التي لا تتطلب حضوراً جسدياً كاملاً. بدأت بمشاريع صغيرة، ثم عملت عن بُعد مع فرق مختلفة.\r\n\r\nنصيحي لكل من يبدأ: اختر مهارة واحدة، وخصص لها ساعة يومياً، ولا تخجل من طلب المساعدة.",
                        Category = InsightCategory.SuccessStory,
                        IsPublished = true,
                        CreatedAt = now.AddDays(-10),
                        PublishedAt = now.AddDays(-10)
                    },
                    new Insight
                    {
                        AuthorId = expert.Id,
                        Title = "تهيئة المنزل لبيئة عمل مريحة",
                        Summary = "خطوات عملية بسيطة تجعل مساحة العمل من المنزل أكثر راحة وإنتاجية.",
                        Content = "ابدأ بترتيب الإضاءة: ضوء طبيعي من الجانب أفضل من الضوء المباشر أمام الشاشة.\r\n\r\nثانياً، ضبط ارتفاع الطاولة والكرسي مهم جداً لتقليل إجهاد الظهر والرقبة.\r\n\r\nثالثاً، اجعل كل ما تحتاجه في متناول يدك: كوب الماء، الشاحن، أداة الإدخال المناسبة لك. وأخيراً، خذ فترات راحة قصيرة كل 45 دقيقة.",
                        Category = InsightCategory.Guidance,
                        IsPublished = true,
                        CreatedAt = now.AddDays(-5),
                        PublishedAt = now.AddDays(-5)
                    },
                    new Insight
                    {
                        AuthorId = expert.Id,
                        Title = "مسودة: أدوات مساعدة يجب أن تعرفها",
                        Summary = "قائمة قيد الإعداد بأهم الأدوات التقنية المساعدة.",
                        Content = "هذه مسودة غير منشورة — تظهر فقط لصاحبها وللإدارة.",
                        Category = InsightCategory.Awareness,
                        IsPublished = false,
                        CreatedAt = now.AddDays(-1)
                    });
            }

            if (!await context.Posts.AnyAsync())
            {
                var welcomePost = new Post
                {
                    AuthorId = expert.Id,
                    Content = "أهلاً بكم في صوتنا! هذه مساحة نشارك فيها التجارب ونتعلم من بعضنا. لا تترددوا في طرح أسئلتكم، فكل سؤال يفتح باباً لشخص آخر.",
                    CreatedAt = now.AddHours(-6),
                    Comments = new List<Comment>
                    {
                        new()
                        {
                            AuthorId = member.Id,
                            Content = "فكرة رائعة! سعيد جداً بالانضمام إلى هذا المجتمع.",
                            CreatedAt = now.AddHours(-4)
                        }
                    },
                    Likes = new List<PostLike>
                    {
                        new() { UserId = member.Id, CreatedAt = now.AddHours(-5) },
                        new() { UserId = admin.Id, CreatedAt = now.AddHours(-3) }
                    }
                };

                var experiencePost = new Post
                {
                    AuthorId = member.Id,
                    Content = "اليوم أنهيت أول مشروع برمجي لي ضمن دورة مجانية! التجربة كانت صعبة في البداية لكن الاستمرار يصنع الفرق.",
                    CreatedAt = now.AddHours(-2),
                    Comments = new List<Comment>
                    {
                        new()
                        {
                            AuthorId = expert.Id,
                            Content = "أحسنت! استمر، وأول مشروع يكون دائماً نقطة البداية الحقيقية.",
                            CreatedAt = now.AddHours(-1)
                        }
                    }
                };

                context.Posts.AddRange(welcomePost, experiencePost);
            }

            await context.SaveChangesAsync();
        }

        private static async Task EnsureUserAsync(
            UserManager<ApplicationUser> userManager,
            string email,
            string fullName,
            string password,
            string role)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(user, password);
                if (!createResult.Succeeded)
                {
                    return;
                }
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
