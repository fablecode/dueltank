﻿using dueltank.core.Models.Db;
using Microsoft.EntityFrameworkCore;
using Archetype = dueltank.core.Models.Db.Archetype;
using ArchetypeCard = dueltank.core.Models.Db.ArchetypeCard;
using AspNetRoleClaims = dueltank.core.Models.Db.AspNetRoleClaims;
using AspNetRoles = dueltank.core.Models.Db.AspNetRoles;
using AspNetUserClaims = dueltank.core.Models.Db.AspNetUserClaims;
using AspNetUserLogins = dueltank.core.Models.Db.AspNetUserLogins;
using AspNetUserRoles = dueltank.core.Models.Db.AspNetUserRoles;
using AspNetUsers = dueltank.core.Models.Db.AspNetUsers;
using AspNetUserTokens = dueltank.core.Models.Db.AspNetUserTokens;
using Attribute = dueltank.core.Models.Db.Attribute;
using Banlist = dueltank.core.Models.Db.Banlist;
using BanlistCard = dueltank.core.Models.Db.BanlistCard;
using Card = dueltank.core.Models.Db.Card;
using CardAttribute = dueltank.core.Models.Db.CardAttribute;
using CardLinkArrow = dueltank.core.Models.Db.CardLinkArrow;
using CardRuling = dueltank.core.Models.Db.CardRuling;
using CardSubCategory = dueltank.core.Models.Db.CardSubCategory;
using CardTip = dueltank.core.Models.Db.CardTip;
using CardTrivia = dueltank.core.Models.Db.CardTrivia;
using CardType = dueltank.core.Models.Db.CardType;
using Category = dueltank.core.Models.Db.Category;
using Deck = dueltank.core.Models.Db.Deck;
using DeckCard = dueltank.core.Models.Db.DeckCard;
using DeckType = dueltank.core.Models.Db.DeckType;
using Format = dueltank.core.Models.Db.Format;
using Limit = dueltank.core.Models.Db.Limit;
using LinkArrow = dueltank.core.Models.Db.LinkArrow;
using SubCategory = dueltank.core.Models.Db.SubCategory;
using Tip = dueltank.core.Models.Db.Tip;
using TipSection = dueltank.core.Models.Db.TipSection;
using Type = dueltank.core.Models.Db.Type;

namespace dueltank.infrastructure.Database
{
    public partial class DueltankDbContext : DbContext
    {
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
        public virtual DbSet<Deck> Deck { get; set; }
        public virtual DbSet<DeckCard> DeckCard { get; set; }
        public virtual DbSet<DeckType> DeckType { get; set; }
        public virtual DbSet<Format> Format { get; set; }
        public virtual DbSet<Limit> Limit { get; set; }
        public virtual DbSet<LinkArrow> LinkArrow { get; set; }
        public virtual DbSet<Ruling> Ruling { get; set; }
        public virtual DbSet<RulingSection> RulingSection { get; set; }
        public virtual DbSet<SubCategory> SubCategory { get; set; }
        public virtual DbSet<Tip> Tip { get; set; }
        public virtual DbSet<TipSection> TipSection { get; set; }
        public virtual DbSet<Trivia> Trivia { get; set; }
        public virtual DbSet<TriviaSection> TriviaSection { get; set; }
        public virtual DbSet<Type> Type { get; set; }


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

                entity.HasOne(d => d.Archetype)
                    .WithMany(p => p.ArchetypeCard)
                    .HasForeignKey(d => d.ArchetypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArchetypeCard_Archetype");

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.ArchetypeCard)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArchetypeCard_Card");
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

                entity.HasOne(d => d.Format)
                    .WithMany(p => p.Banlist)
                    .HasForeignKey(d => d.FormatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Banlist_Format");
            });

            modelBuilder.Entity<BanlistCard>(entity =>
            {
                entity.HasKey(e => new { e.BanlistId, e.CardId });

                entity.HasOne(d => d.Banlist)
                    .WithMany(p => p.BanlistCard)
                    .HasForeignKey(d => d.BanlistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BanlistCard_ToBanlist");

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.BanlistCard)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BanlistCard_ToCard");

                entity.HasOne(d => d.Limit)
                    .WithMany(p => p.BanlistCard)
                    .HasForeignKey(d => d.LimitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BanlistCard_ToLimit");
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_Card")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<CardAttribute>(entity =>
            {
                entity.HasKey(e => new { e.AttributeId, e.CardId });

                entity.HasOne(d => d.Attribute)
                    .WithMany(p => p.CardAttribute)
                    .HasForeignKey(d => d.AttributeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CardAttributes_Attributes");

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.CardAttribute)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CardAttributes_CardId");
            });

            modelBuilder.Entity<CardLinkArrow>(entity =>
            {
                entity.HasKey(e => new { e.LinkArrowId, e.CardId });

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.CardLinkArrow)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CardLinkArrow_Card");

                entity.HasOne(d => d.LinkArrow)
                    .WithMany(p => p.CardLinkArrow)
                    .HasForeignKey(d => d.LinkArrowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CardLinkArrow_LinkArrow");
            });

            modelBuilder.Entity<CardRuling>(entity =>
            {
                entity.Property(e => e.Ruling).IsRequired();

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.CardRuling)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CardRuling_ToCard");
            });

            modelBuilder.Entity<CardSubCategory>(entity =>
            {
                entity.HasKey(e => new { e.SubCategoryId, e.CardId });

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.CardSubCategory)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CardSubCategory_ToCard");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.CardSubCategory)
                    .HasForeignKey(d => d.SubCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CardSubCategory_ToSubCategory");
            });

            modelBuilder.Entity<CardTip>(entity =>
            {
                entity.Property(e => e.Tip).IsRequired();
            });

            modelBuilder.Entity<CardTrivia>(entity =>
            {
                entity.Property(e => e.Trivia).IsRequired();

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.CardTrivia)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CardTrivia_ToCard");
            });

            modelBuilder.Entity<CardType>(entity =>
            {
                entity.HasKey(e => new { e.TypeId, e.CardId });

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.CardType)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CardType_Card");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.CardType)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CardType_Type");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Deck>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.VideoUrl).HasMaxLength(2083);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Deck)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Deck_AspNetUsers");
            });

            modelBuilder.Entity<DeckCard>(entity =>
            {
                entity.HasKey(e => new { e.DeckTypeId, e.DeckId, e.CardId });

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.DeckCard)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeckCard_Card");

                entity.HasOne(d => d.Deck)
                    .WithMany(p => p.DeckCard)
                    .HasForeignKey(d => d.DeckId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeckCard_Deck");

                entity.HasOne(d => d.DeckType)
                    .WithMany(p => p.DeckCard)
                    .HasForeignKey(d => d.DeckTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeckCard_DeckType1");
            });

            modelBuilder.Entity<DeckType>(entity =>
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

            modelBuilder.Entity<Ruling>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.RulingSection)
                    .WithMany(p => p.Ruling)
                    .HasForeignKey(d => d.RulingSectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ruling_RulingSection");
            });

            modelBuilder.Entity<RulingSection>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.RulingSection)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RulingSection_Card");
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.SubCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubCategory_Archetype");
            });

            modelBuilder.Entity<Tip>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.TipSection)
                    .WithMany(p => p.Tip)
                    .HasForeignKey(d => d.TipSectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tip_TipSection");
            });

            modelBuilder.Entity<TipSection>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.TipSection)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TipSection_Card");
            });

            modelBuilder.Entity<Trivia>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.TriviaSection)
                    .WithMany(p => p.Trivia)
                    .HasForeignKey(d => d.TriviaSectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trivia_TriviaSection");
            });

            modelBuilder.Entity<TriviaSection>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.TriviaSection)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TriviaSection_Card");
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
