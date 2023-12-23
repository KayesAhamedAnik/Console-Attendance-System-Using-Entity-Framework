using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConsoleAttendanceSystem.Entities;
using System.Reflection.Emit;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleAttendanceSystem.EntityFramework
{
    public class TrainingDbContext:DbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public TrainingDbContext()
        {
            _connectionString = @"Server=DESKTOP-K69KQB2;Database=C#Projects; Trusted_Connection=True;";
            _migrationAssembly = Assembly.GetExecutingAssembly().GetName().Name;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString, (x) => x.MigrationsAssembly(_migrationAssembly));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Teacher>().ToTable("Teachers");
            modelBuilder.Entity<Course>().ToTable("Courses");
            modelBuilder.Entity<Enroll>().ToTable("Enrolls");
            modelBuilder.Entity<Attendance>().ToTable("Attendance");

            modelBuilder.Entity<Admin>().HasKey((x) => new { x.AdminId});
            modelBuilder.Entity<Student>().HasKey((x) => new { x.StudentId});
            modelBuilder.Entity<Teacher>().HasKey((x) => new { x.TeacherId});
            modelBuilder.Entity<Course>().HasKey((x) => new { x.CourseId });
            modelBuilder.Entity<Enroll>().HasKey((x) => new {x.EnrollId });
            modelBuilder.Entity<Attendance>().HasKey((x) => new { x.AttendanceId });
            //Course enroll relationship for Course, Student and Teacher
            modelBuilder.Entity<Enroll>()
                .HasOne<Student>(sc => sc.Student)
                .WithMany(s => s.EnrolledCourses)
                .HasForeignKey(sc => sc.StudentId)
                 .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Enroll>()
                .HasOne<Course>(sc => sc.Course)
                .WithMany(s => s.EnrolledCourses)
                .HasForeignKey(sc => sc.CourseId)
                 .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Enroll>()
                .HasOne<Teacher>(sc => sc.Teacher)
                .WithMany(s => s.EnrolledCourses)
                .HasForeignKey(sc => sc.TeacherId)
                 .OnDelete(DeleteBehavior.ClientCascade);

            //Attendance
            modelBuilder.Entity<Attendance>()
                .HasOne<Student>(sc => sc.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<Attendance>()
                .HasOne<Course>(sc => sc.Course)
                .WithMany(s => s.Attendances)
                .HasForeignKey(sc => sc.CourseId);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enroll> Enrolls { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
    }
}
