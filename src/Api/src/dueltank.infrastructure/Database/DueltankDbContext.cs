using dueltank.core.Models.Db;
using Microsoft.EntityFrameworkCore;
using Attribute = dueltank.core.Models.Db.Attribute;
using Type = dueltank.core.Models.Db.Type;

namespace dueltank.infrastructure.Database
{
    public class DueltankDbContext : DbContext
    {
        public DueltankDbContext()
        {
        }

        public DueltankDbContext(DbContextOptions<DueltankDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Archetype> Archetype { get; set; }
        public virtual DbSet<ArchetypeCard> ArchetypeCard { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<Attribute> Attribute { get; set; }
        public virtual DbSet<Banlist> Banlist { get; set; }
        public virtual DbSet<BanlistCard> BanlistCard { get; set; }
        public virtual DbSet<Card> Card { get; set; }
        public virtual DbSet<CardAttribute> CardAttribute { get; set; }
        public virtual DbSet<CardLinkArrow> CardLinkArrow { get; set; }
        public virtual DbSet<CardRuling> CardRuling { get; set; }
        public virtual DbSet<CardSubCategory> CardSubCategory { get; set; }
        public virtual DbSet<CardTip> CardTip { get; set; }
        public virtual DbSet<CardTrivia> CardTrivia { get; set; }
        public virtual DbSet<CardType> CardType { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Format> Format { get; set; }
        public virtual DbSet<Limit> Limit { get; set; }
        public virtual DbSet<LinkArrow> LinkArrow { get; set; }
        public virtual DbSet<SubCategory> SubCategory { get; set; }
        public virtual DbSet<Tip> Tip { get; set; }
        public virtual DbSet<TipSection> TipSection { get; set; }
        public virtual DbSet<Type> Type { get; set; }

        public DueltankDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Archetype>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(2083)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ArchetypeCard>(entity =>
            {
                entity.HasKey(e => new { e.ArchetypeId, e.CardId });
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Attribute>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Banlist>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ReleaseDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<BanlistCard>(entity =>
            {
                entity.HasKey(e => new { e.BanlistId, e.CardId });
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_Card")
                    .IsUnique();

                entity.Property(e => e.CardNumber).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<CardAttribute>(entity =>
            {
                entity.HasKey(e => new { e.AttributeId, e.CardId });
            });

            modelBuilder.Entity<CardLinkArrow>(entity =>
            {
                entity.HasKey(e => new { e.LinkArrowId, e.CardId });
            });

            modelBuilder.Entity<CardRuling>(entity =>
            {
                entity.Property(e => e.Ruling).IsRequired();
            });

            modelBuilder.Entity<CardSubCategory>(entity =>
            {
                entity.HasKey(e => new { e.SubCategoryId, e.CardId });
            });

            modelBuilder.Entity<CardTip>(entity =>
            {
                entity.Property(e => e.Tip).IsRequired();
            });

            modelBuilder.Entity<CardTrivia>(entity =>
            {
                entity.Property(e => e.Trivia).IsRequired();
            });

            modelBuilder.Entity<CardType>(entity =>
            {
                entity.HasKey(e => new { e.TypeId, e.CardId });
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Format>(entity =>
            {
                entity.Property(e => e.Acronym)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Limit>(entity =>
            {
                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<LinkArrow>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Tip>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipSection>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });
        }
    }
}
