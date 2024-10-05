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

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<CardItemHandmadeProduct> CardItemHandmadeProducts { get; set; }

    public virtual DbSet<CardItemLearningEquipment> CardItemLearningEquipments { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CommentsCoursesBefore> CommentsCoursesBefores { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<EmailMessage> EmailMessages { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<HandmadeProduct> HandmadeProducts { get; set; }

    public virtual DbSet<LearningEquipment> LearningEquipments { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Newsletter> Newsletters { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<PaymentsCourse> PaymentsCourses { get; set; }

    public virtual DbSet<ResponsesCommentsCoursesBefore> ResponsesCommentsCoursesBefores { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Trainer> Trainers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCourse> UserCourses { get; set; }

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

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__5DE3A5B1763F4CA4");

            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.BookingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("booking_date");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("مؤكد")
                .HasColumnName("status");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Event).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__Bookings__EventI__3A4CA8FD");

            entity.HasOne(d => d.Student).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Bookings__studen__395884C4");
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("PK__Card__55FECDAEAD266917");

            entity.ToTable("Card");

            entity.Property(e => e.CardId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Cards)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Card__UserId__4F47C5E3");
        });

        modelBuilder.Entity<CardItemHandmadeProduct>(entity =>
        {
            entity.HasKey(e => e.CardItemId).HasName("PK__CardItem__60AC9AAFD81FE8FB");

            entity.ToTable("CardItem_Handmade_Products");

            entity.Property(e => e.AddedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.Card).WithMany(p => p.CardItemHandmadeProducts)
                .HasForeignKey(d => d.CardId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__CardItem___CardI__2F9A1060");

            entity.HasOne(d => d.Product).WithMany(p => p.CardItemHandmadeProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__CardItem___produ__308E3499");
        });

        modelBuilder.Entity<CardItemLearningEquipment>(entity =>
        {
            entity.HasKey(e => e.CardItemId).HasName("PK__CardItem__60AC9AAFF130D90C");

            entity.ToTable("CardItem_Learning_Equipment");

            entity.Property(e => e.AddedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EquipmentId).HasColumnName("equipmentID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.Card).WithMany(p => p.CardItemLearningEquipments)
                .HasForeignKey(d => d.CardId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__CardItem___CardI__3552E9B6");

            entity.HasOne(d => d.Equipment).WithMany(p => p.CardItemLearningEquipments)
                .HasForeignKey(d => d.EquipmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__CardItem___equip__36470DEF");
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

        modelBuilder.Entity<CommentsCoursesBefore>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__E7957687105CCE48");

            entity.ToTable("Comments_Courses_before");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.CommentText).HasColumnName("comment_text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Name).HasMaxLength(255);
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

        modelBuilder.Entity<EmailMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmailMes__3214EC07DDA8F63F");

            entity.ToTable("EmailMessage");

            entity.Property(e => e.DateSent)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Events__7944C87053C7EFCD");

            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.EventTitle).HasMaxLength(255);
            entity.Property(e => e.ImagePath).HasMaxLength(255);
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.MapUrl).HasColumnName("MapURL");
            entity.Property(e => e.Speaker).HasMaxLength(255);
            entity.Property(e => e.ZoomLink).HasMaxLength(255);
            entity.Property(e => e.ZoomPassword).HasMaxLength(50);
        });

        modelBuilder.Entity<HandmadeProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Handmade__47027DF5997B6656");

            entity.ToTable("Handmade_Products");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl1)
                .HasMaxLength(255)
                .HasColumnName("image_url_1");
            entity.Property(e => e.ImageUrl2)
                .HasMaxLength(255)
                .HasColumnName("image_url_2");
            entity.Property(e => e.ImageUrl3)
                .HasMaxLength(255)
                .HasColumnName("image_url_3");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Rating)
                .HasColumnType("decimal(2, 1)")
                .HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Category).WithMany(p => p.HandmadeProducts)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Handmade___categ__03F0984C");

            entity.HasOne(d => d.User).WithMany(p => p.HandmadeProducts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Handmade___user___04E4BC85");
        });

        modelBuilder.Entity<LearningEquipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("PK__Learning__197068AF893B431B");

            entity.ToTable("Learning_Equipment");

            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl1)
                .HasMaxLength(255)
                .HasColumnName("image_url_1");
            entity.Property(e => e.ImageUrl2)
                .HasMaxLength(255)
                .HasColumnName("image_url_2");
            entity.Property(e => e.ImageUrl3)
                .HasMaxLength(255)
                .HasColumnName("image_url_3");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Rating)
                .HasColumnType("decimal(2, 1)")
                .HasColumnName("rating");

            entity.HasOne(d => d.Category).WithMany(p => p.LearningEquipments)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Learning___categ__5AB9788F");

            entity.HasOne(d => d.Course).WithMany(p => p.LearningEquipments)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Learning___cours__59C55456");
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
            entity.HasKey(e => e.NewsletterId).HasName("PK__Newslett__57628D305ACE1B8E");

            entity.ToTable("Newsletter");

            entity.HasIndex(e => e.Email, "UQ__Newslett__AB6E616445B58B6A").IsUnique();

            entity.Property(e => e.NewsletterId).HasColumnName("newsletter_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.SubscribedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("subscribed_at");

            entity.HasOne(d => d.Event).WithMany(p => p.Newsletters)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__Newslette__Event__3F115E1A");
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

        modelBuilder.Entity<PaymentsCourse>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A380E23BE56");

            entity.ToTable(tb => tb.HasTrigger("trg_AfterPaymentInsert"));

            entity.Property(e => e.AddressLine1).HasMaxLength(255);
            entity.Property(e => e.AddressLine2).HasMaxLength(255);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CardName).HasMaxLength(100);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CountryCallingCode).HasMaxLength(10);
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.PasswordSalt).HasMaxLength(255);
            entity.Property(e => e.PayerId).HasMaxLength(100);
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TransactionId).HasMaxLength(100);
            entity.Property(e => e.ZipCode).HasMaxLength(50);

            entity.HasOne(d => d.Course).WithMany(p => p.PaymentsCourses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__PaymentsC__cours__41B8C09B");

            entity.HasOne(d => d.User).WithMany(p => p.PaymentsCourses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__PaymentsC__UserI__40C49C62");
        });

        modelBuilder.Entity<ResponsesCommentsCoursesBefore>(entity =>
        {
            entity.HasKey(e => e.ResponseId).HasName("PK__Response__EBECD896AB553EA1");

            entity.ToTable("Responses_Comments_Courses_before");

            entity.Property(e => e.ResponseId).HasColumnName("response_id");
            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.ResponseText).HasColumnName("response_text");

            entity.HasOne(d => d.Comment).WithMany(p => p.ResponsesCommentsCoursesBefores)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("FK__Responses__comme__123EB7A3");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__4550733FA5488CB1");

            entity.ToTable("Service");

            entity.Property(e => e.ServiceId).HasColumnName("serviceID");
            entity.Property(e => e.ServiceDescription).HasMaxLength(500);
            entity.Property(e => e.ServiceName).HasMaxLength(50);
        });

        modelBuilder.Entity<Trainer>(entity =>
        {
            entity.HasKey(e => e.TrainerId).HasName("PK__Trainers__65A4B6296C13BE77");

            entity.Property(e => e.TrainerId).HasColumnName("trainer_id");
            entity.Property(e => e.Bio).HasColumnName("bio");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Cv).HasColumnName("cv");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Experience)
                .HasMaxLength(255)
                .HasColumnName("experience");
            entity.Property(e => e.ImageProfile)
                .HasMaxLength(255)
                .HasColumnName("image_profile");
            entity.Property(e => e.JobTitle)
                .HasMaxLength(255)
                .HasColumnName("job_title");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PasswordSalt).HasMaxLength(255);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.Satas)
                .HasDefaultValue(false)
                .HasColumnName("satas");
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
            entity.Property(e => e.BiographicaldetailsCv).HasColumnName("biographicaldetailsCV");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Imageprofile).HasColumnName("imageprofile");
            entity.Property(e => e.JobTitle).HasMaxLength(50);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Otp).HasMaxLength(10);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PasswordSalt).HasMaxLength(255);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasColumnName("role");
        });

        modelBuilder.Entity<UserCourse>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__user_cou__6D24AA7AE06712F9");

            entity.ToTable("user_course");

            entity.Property(e => e.EnrollmentId).HasColumnName("enrollment_id");
            entity.Property(e => e.Completed)
                .HasDefaultValue(false)
                .HasColumnName("completed");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.EnrollmentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("enrollment_date");
            entity.Property(e => e.Progress)
                .HasDefaultValue(0)
                .HasColumnName("progress");
            entity.Property(e => e.TrainerId).HasColumnName("trainer_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Course).WithMany(p => p.UserCourses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__user_cour__cours__2CF2ADDF");

            entity.HasOne(d => d.Trainer).WithMany(p => p.UserCourses)
                .HasForeignKey(d => d.TrainerId)
                .HasConstraintName("FK_UserCourse_Trainers");

            entity.HasOne(d => d.User).WithMany(p => p.UserCourses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__user_cour__user___2BFE89A6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
