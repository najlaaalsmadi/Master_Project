using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace backend_Master.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<HandmadeProduct> HandmadeProducts { get; set; }

    public virtual DbSet<LearningEquipment> LearningEquipments { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Newsletter> Newsletters { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Trainer> Trainers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-UO3GGAT;Database=project_Master_core;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__43AA414174D44FC3");

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.AccessLevel)
                .HasMaxLength(50)
                .HasColumnName("access_level");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Admins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Admins__user_id__619B8048");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__D54EE9B4FE06BA44");

            entity.HasIndex(e => e.Name, "UQ__Categori__72E12F1B9B95A5BC").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__E795768792085702");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.CommentText).HasColumnName("comment_text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.ItemType)
                .HasMaxLength(50)
                .HasColumnName("item_type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Comments__user_i__656C112C");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__8F1EF7AEF8C0E1DB");

            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.AllowedStudents).HasColumnName("allowed_students");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Rating).HasDefaultValue(5);
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Syllabus).HasColumnName("syllabus");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Tools).HasColumnName("tools");
            entity.Property(e => e.TrainerId).HasColumnName("trainer_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Courses__categor__48CFD27E");

            entity.HasOne(d => d.Trainer).WithMany(p => p.Courses)
                .HasForeignKey(d => d.TrainerId)
                .HasConstraintName("FK__Courses__trainer__47DBAE45");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Events__2370F7274759972C");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<HandmadeProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Handmade__47027DF5FF49C077");

            entity.ToTable("Handmade_Products");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Stock).HasColumnName("stock");
        });

        modelBuilder.Entity<LearningEquipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("PK__Learning__197068AFB2FFC079");

            entity.ToTable("Learning_Equipment");

            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.Course).WithMany(p => p.LearningEquipments)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Learning___cours__52593CB8");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PK__Lessons__6421F7BE2D6E5666");

            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.VideoUrl)
                .HasMaxLength(255)
                .HasColumnName("video_url");

            entity.HasOne(d => d.Course).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Lessons__course___4BAC3F29");
        });

        modelBuilder.Entity<Newsletter>(entity =>
        {
            entity.HasKey(e => e.NewsletterId).HasName("PK__Newslett__57628D302A51D783");

            entity.ToTable("Newsletter");

            entity.HasIndex(e => e.Email, "UQ__Newslett__AB6E61646C6C11FE").IsUnique();

            entity.Property(e => e.NewsletterId).HasColumnName("newsletter_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.SubscribedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("subscribed_at");

            entity.HasOne(d => d.Event).WithMany(p => p.Newsletters)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__Newslette__event__5DCAEF64");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__4659622946E374DA");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_amount");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Orders__user_id__571DF1D5");
        });

        modelBuilder.Entity<Trainer>(entity =>
        {
            entity.HasKey(e => e.TrainerId).HasName("PK__Trainers__65A4B6296C13BE77");

            entity.Property(e => e.TrainerId).HasColumnName("trainer_id");
            entity.Property(e => e.Bio).HasColumnName("bio");
            entity.Property(e => e.Experience)
                .HasMaxLength(255)
                .HasColumnName("experience");
            entity.Property(e => e.Specialization)
                .HasMaxLength(255)
                .HasColumnName("specialization");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Trainers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Trainers__user_i__3C69FB99");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370F35DD3C1F");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E616477D4B3D8").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
