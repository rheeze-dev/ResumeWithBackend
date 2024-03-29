﻿namespace Server.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base version of this method as well, else we get an error later on.
            base.OnModelCreating(modelBuilder);

            #region Categories seed

            Category[] categoriesToSeed = new Category[3];

            for (int i = 1; i < 4; i++)
            {
                categoriesToSeed[i - 1] = new Category
                {
                    CategoryId = i,
                    ThumbnailImagePath = "uploads/placeholder.jpg",
                    Name = $"Category {i}",
                    Description = $"A description of category {i}"
                };
            }

            modelBuilder.Entity<Category>().HasData(categoriesToSeed);

            #endregion

            modelBuilder.Entity<Post>(
                entity =>
                {
                    entity.HasOne(post => post.Category)
                    .WithMany(category => category.Posts)
                    .HasForeignKey("CategoryId");
                });

            #region Posts seed

            Post[] postsToSeed = new Post[6];

            for (int i = 1; i < 7; i++)
            {
                string postTitle = string.Empty;
                int categoryId = 0;

                switch (i)
                {
                    case 1:
                        postTitle = "First post";
                        categoryId = 1;
                        break;
                    case 2:
                        postTitle = "Second post";
                        categoryId = 2;
                        break;
                    case 3:
                        postTitle = "Third post";
                        categoryId = 3;
                        break;
                    case 4:
                        postTitle = "Fourth post";
                        categoryId = 1;
                        break;
                    case 5:
                        postTitle = "Fifth post";
                        categoryId = 2;
                        break;
                    case 6:
                        postTitle = "Sixth post";
                        categoryId = 3;
                        break;
                    default:
                        break;
                }

                postsToSeed[i - 1] = new Post
                {
                    PostId = i,
                    ThumbnailImagePath = "uploads/placeholder.jpg",
                    Title = postTitle,
                    Excerpt = $"This is the excerpt for post {i}. An excerpt is a little extraction from a larger piece of text. Sort of like a preview.",
                    Content = string.Empty,
                    PublishDate = DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm"),
                    Published = true,
                    Author = "Rheeze Gyver Kalahi",
                    CategoryId = categoryId
                };
            }

            modelBuilder.Entity<Post>().HasData(postsToSeed);

            #endregion

            #region Administrator role seed

            const string administratorRoleName = "Administrator";

            IdentityRole administratorRoleToSeed = new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = administratorRoleName,
                NormalizedName = administratorRoleName.ToUpperInvariant()
            };

            modelBuilder.Entity<IdentityRole>().HasData(administratorRoleToSeed);

            #endregion

            #region Administrator user seed

            const string administratorUserEmail = "admin@johndoe.com";

            var passwordHasher = new PasswordHasher<IdentityUser>();

            IdentityUser administratorUserToSeed = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = administratorUserEmail,
                NormalizedUserName = administratorUserEmail.ToUpperInvariant(),
                Email = administratorUserEmail,
                NormalizedEmail = administratorUserEmail.ToUpperInvariant(),
                PasswordHash = string.Empty,
            };

            string hashedPassword = passwordHasher.HashPassword(administratorUserToSeed, "Password1!");

            administratorUserToSeed.PasswordHash = hashedPassword;

            modelBuilder.Entity<IdentityUser>().HasData(administratorUserToSeed);

            #endregion

            #region Add the administrator user to the administrator role

            IdentityUserRole<string> identityUserRoleToSeed = new()
            {
                RoleId = administratorRoleToSeed.Id,
                UserId = administratorUserToSeed.Id
            };

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(identityUserRoleToSeed);

            #endregion
        }
    }
}