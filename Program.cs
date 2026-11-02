using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Data;
using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new PlatformDbContext())
            {
                context.Database.Migrate();
            }

            SeedStudents();
            SeedEnrollments();

            AddInstructor(
                "Sara Mohamed",
                "sara@mail.com",
                null);

            AddCourse(
                "SQL Basics",
                600m,
                CourseLevel.Beginner,
                1,
                2);

            AddSeveralStudents();

            int studentId;

            using (var context = new PlatformDbContext())
            {
                var student = context.Students
                    .FirstOrDefault(s => s.Email == "aisha@mail.com");

                if (student == null)
                {
                    Console.WriteLine("Demo student not found.");
                    return;
                }

                studentId = student.Id;
            }

            EnrollStudent(studentId, 4);

            FindCourse(1, "EF Core");

            FilterCourses(400m, 1000m);

            ShowCategories(1);

            ShowCourseRoster(1);

            ChangePrice(1, 550m);

            UpdateProgress(studentId, 1, 100);

            PublishCourse(3);

            Unenroll(studentId, 4);

            DeleteCategoryWithException(1);

            DeleteCategorySafely(1);

            DeleteCourse(6);

            BulkPriceRaise(CourseLevel.Beginner);

            BulkDeleteDropped();
        }

        static void SeedStudents()
        {
            using var context = new PlatformDbContext();

            if (context.Students.Any())
            {
                return;
            }
            //lw fe data bykhrog

            context.Students.AddRange(
                new Student
                {
                    FullName = "Aisha Ahmed",
                    Email = "aisha@mail.com",
                    DateOfBirth = new DateTime(2003, 1, 1)
                },
                new Student
                {
                    FullName = "Ali Hassan",
                    Email = "ali@mail.com",
                    DateOfBirth = new DateTime(2002, 2, 2)
                },
                new Student
                {
                    FullName = "Nour Adel",
                    Email = "nour@mail.com",
                    DateOfBirth = new DateTime(2001, 3, 3)
                },
                new Student
                {
                    FullName = "Salma Omar",
                    Email = "salma@mail.com",
                    DateOfBirth = new DateTime(2003, 4, 4)
                },
                new Student
                {
                    FullName = "Youssef Ahmed",
                    Email = "youssef@mail.com",
                    DateOfBirth = new DateTime(2002, 5, 5)
                }
            );

            context.SaveChanges();

            Console.WriteLine("5 students seeded.");
        }

        static void SeedEnrollments()
        {
            using var context = new PlatformDbContext();

            if (context.Enrollments.Any())
            {
                return;
            }

            var students = context.Students
                .OrderBy(s => s.Id)
                .Take(5)
                .ToList();

            if (students.Count < 5)
            {
                Console.WriteLine(
                    "Enrollment seed requires 5 students.");

                return;
            }

            int[] courseIds = { 1, 2, 4, 6 };

            int publishedCourses = context.Courses
                .Count(c => courseIds.Contains(c.Id)
                    && c.IsPublished);

            if (publishedCourses != 4)
            {
                Console.WriteLine(
                    "Required published courses are missing.");

                return;
            }

            context.Enrollments.AddRange(
                new Enrollment
                {
                    StudentId = students[0].Id,
                    CourseId = 1,
                    EnrollmentDate = new DateTime(2026, 3, 1),
                    Progress = 30,
                    Status = EnrollmentStatus.Active
                },
                new Enrollment
                {
                    StudentId = students[0].Id,
                    CourseId = 2,
                    EnrollmentDate = new DateTime(2026, 3, 1),
                    Progress = 100,
                    Status = EnrollmentStatus.Completed,
                    FinalGrade = 95m
                },
                new Enrollment
                {
                    StudentId = students[1].Id,
                    CourseId = 1,
                    EnrollmentDate = new DateTime(2026, 3, 2),
                    Progress = 20,
                    Status = EnrollmentStatus.Active
                },
                new Enrollment
                {
                    StudentId = students[1].Id,
                    CourseId = 4,
                    EnrollmentDate = new DateTime(2026, 3, 2),
                    Progress = 40,
                    Status = EnrollmentStatus.Dropped
                },
                new Enrollment
                {
                    StudentId = students[2].Id,
                    CourseId = 2,
                    EnrollmentDate = new DateTime(2026, 3, 3),
                    Progress = 100,
                    Status = EnrollmentStatus.Completed,
                    FinalGrade = 88m
                },
                new Enrollment
                {
                    StudentId = students[2].Id,
                    CourseId = 6,
                    EnrollmentDate = new DateTime(2026, 3, 3),
                    Progress = 50,
                    Status = EnrollmentStatus.Active
                },
                new Enrollment
                {
                    StudentId = students[3].Id,
                    CourseId = 4,
                    EnrollmentDate = new DateTime(2026, 3, 4),
                    Progress = 10,
                    Status = EnrollmentStatus.Dropped
                },
                new Enrollment
                {
                    StudentId = students[4].Id,
                    CourseId = 6,
                    EnrollmentDate = new DateTime(2026, 3, 5),
                    Progress = 100,
                    Status = EnrollmentStatus.Completed,
                    FinalGrade = 92m
                }
            );

            context.SaveChanges();

            Console.WriteLine("8 enrollments seeded.");
        }

        static void AddInstructor(
            string fullName,
            string email,
            string? bio)
        {
            if (string.IsNullOrWhiteSpace(fullName)
                || fullName.Length > 100
                || string.IsNullOrWhiteSpace(email)
                || email.Length > 150
                || (bio != null && bio.Length > 500))
            {
                Console.WriteLine("Invalid instructor data.");
                return;
            }

            using var context = new PlatformDbContext();

            var instructor = new Instructor
            {
                FullName = fullName,
                Email = email,
                Bio = bio,
                JoinedAt = DateTime.Now
            };
            //kda hytkhzno hnak w kol ma adef hd gded hytkhzn tlka2i 3shan el method 
            context.Instructors.Add(instructor);
            

            context.SaveChanges();

            Console.WriteLine(
                $"Instructor added. Id: {instructor.Id}");
        }

        static void AddCourse(
            string title,
            decimal price,
            CourseLevel level,
            int instructorId,
            int categoryId)
        {
            if (string.IsNullOrWhiteSpace(title)
                || title.Length > 150
                || price < 0
                || price > 99999999.99m   //3shan howa 10,2
                || !Enum.IsDefined(typeof(CourseLevel), level))  //lazem yltzm b ely ana hateto
            {
                Console.WriteLine("Invalid course data.");
                return;
            }

            using var context = new PlatformDbContext();

            bool instructorExists = context.Instructors
                .Any(i => i.Id == instructorId);
            //byshof howa mawgod wla la

            if (!instructorExists)
            {
                Console.WriteLine("Instructor not found.");
                return;
            }

            bool categoryExists = context.Categories
                .Any(c => c.Id == categoryId);

            if (!categoryExists)
            {
                Console.WriteLine("Category not found.");
                return;
            }

            var course = new Course
            {
                Title = title,
                Price = price,
                Level = level,
                InstructorId = instructorId,
                CategoryId = categoryId,
                IsPublished = false
            };

            context.Courses.Add(course);

            context.SaveChanges();

            Console.WriteLine(
                $"Course added. Id: {course.Id}");
        }

        static void AddSeveralStudents()
        {
            using var context = new PlatformDbContext();

            var student1 = new Student
            {
                FullName = "Hana Ali",
                Email = "hana@mail.com",
                DateOfBirth = new DateTime(2003, 6, 1)
            };

            var student2 = new Student
            {
                FullName = "Adam Omar",
                Email = "adam@mail.com",
                DateOfBirth = new DateTime(2002, 7, 1)
            };

            var student3 = new Student
            {
                FullName = "Mai Hassan",
                Email = "mai@mail.com",
                DateOfBirth = new DateTime(2001, 8, 1)
            };

            context.Students.AddRange(
                student1,
                student2,
                student3);
            //addrange 3shan adef kaza wa7d mara wa7da
            context.SaveChanges();

            Console.WriteLine(
                $"Student IDs: {student1.Id}, " +
                $"{student2.Id}, {student3.Id}");
        }

        static void EnrollStudent(
            int studentId,
            int courseId)
        {
            using var context = new PlatformDbContext();

            bool studentExists = context.Students
                .Any(s => s.Id == studentId);

            if (!studentExists)
            {
                Console.WriteLine(
                    "Enrollment refused: student not found.");

                return;
            }

            var course = context.Courses.Find(courseId);

            if (course == null)
            {
                Console.WriteLine(
                    "Enrollment refused: course not found.");

                return;
            }

            if (!course.IsPublished)
            {
                Console.WriteLine(
                    "Enrollment refused: course is unpublished.");

                return;
            }

            bool alreadyEnrolled = context.Enrollments
                .Any(e => e.StudentId == studentId
                    && e.CourseId == courseId);

            if (alreadyEnrolled)
            {
                Console.WriteLine(
                    "Enrollment refused: student is already enrolled.");

                return;
            }

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now,
                Progress = 0,
                Status = EnrollmentStatus.Active
            };

            context.Enrollments.Add(enrollment);

            context.SaveChanges();

            Console.WriteLine("Student enrolled successfully.");
        }

        static void FindCourse(int id, string title)
        {
            using var context = new PlatformDbContext();

            var courseById = context.Courses.Find(id);

            if (courseById == null)
            {
                Console.WriteLine("Course by Id: not found.");
            }
            else
            {
                Console.WriteLine(
                    $"Course by Id: {courseById.Title}");
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Enter a course title.");
                return;
            }

            var courseByTitle = context.Courses
                .FirstOrDefault(c => c.Title == title);

            if (courseByTitle == null)
            {
                Console.WriteLine("Course by title: not found.");
            }
            else
            {
                Console.WriteLine(
                    $"Course by title: {courseByTitle.Title}");
            }
        }

        static void FilterCourses(
            decimal minPrice,
            decimal maxPrice)
        {
            if (minPrice < 0 || maxPrice < minPrice)
            {
                Console.WriteLine("Invalid price range.");
                return;
            }

            using var context = new PlatformDbContext();

            var courses = context.Courses
                .Where(c => c.Price >= minPrice
                    && c.Price <= maxPrice)
                .OrderByDescending(c => c.Price)
                .Select(c => new
                {
                    c.Title,
                    c.Level,
                    c.Price
                })
                .ToList();

            if (courses.Count == 0)
            {
                Console.WriteLine("No matching courses.");
                return;
            }

            foreach (var course in courses)
            {
                Console.WriteLine(
                    $"{course.Title} - " +
                    $"{course.Level} - " +
                    $"{course.Price}");
            }
        }

        static void ShowCategories(int categoryId)
        {
            using var context = new PlatformDbContext();

            bool categoryExists = context.Categories
                .Any(c => c.Id == categoryId);

            if (!categoryExists)
            {
                Console.WriteLine("Category not found.");
            }
            else
            {
                var subCategories = context.Categories
                    .Where(c => c.ParentCategoryId == categoryId)
                    .ToList();

                Console.WriteLine("Direct sub-categories:");

                if (subCategories.Count == 0)
                {
                    Console.WriteLine("No sub-categories.");
                }

                foreach (var category in subCategories)
                {
                    Console.WriteLine(category.Name);
                }
            }

            var topCategories = context.Categories
                .Where(c => c.ParentCategoryId == null)
                .ToList();

            Console.WriteLine("Top-level categories:");

            foreach (var category in topCategories)
            {
                Console.WriteLine(category.Name);
            }
        }

        static void ShowCourseRoster(int courseId)
        {
            using var context = new PlatformDbContext();

            bool courseExists = context.Courses
                .Any(c => c.Id == courseId);

            if (!courseExists)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            var studentIds = context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.StudentId)
                .ToList();

            Console.WriteLine(
                $"Students count: {studentIds.Count}");

            var students = context.Students
                .Where(s => studentIds.Contains(s.Id))
                .ToList();

            foreach (var student in students)
            {
                Console.WriteLine(student.FullName);
            }
        }

        static void ChangePrice(
            int courseId,
            decimal newPrice)
        {
            if (newPrice < 0 || newPrice > 99999999.99m)
            {
                Console.WriteLine("Invalid price.");
                return;
            }

            using var context = new PlatformDbContext();

            var course = context.Courses.Find(courseId);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            course.Price = newPrice;

            context.SaveChanges();

            Console.WriteLine(
                $"{course.Title}: new price = {course.Price}");
        }

        static void UpdateProgress(
            int studentId,
            int courseId,
            int progress)
        {
            if (progress < 0 || progress > 100)
            {
                Console.WriteLine(
                    "Progress must be between 0 and 100.");

                return;
            }

            using var context = new PlatformDbContext();

            var enrollment = context.Enrollments
                .Find(studentId, courseId);

            if (enrollment == null)
            {
                Console.WriteLine("Enrollment not found.");
                return;
            }

            enrollment.Progress = progress;

            if (progress == 100)
            {
                enrollment.Status = EnrollmentStatus.Completed;
            }
            else
            {
                if (enrollment.Status == EnrollmentStatus.Completed)
                {
                    enrollment.Status = EnrollmentStatus.Active;
                }

                enrollment.FinalGrade = null;
            }

            context.SaveChanges();

            Console.WriteLine(
                $"Progress: {enrollment.Progress}% | " +
                $"Status: {enrollment.Status}");
        }

        static void PublishCourse(int courseId)
        {
            using var context = new PlatformDbContext();

            var course = context.Courses.Find(courseId);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            if (course.IsPublished)
            {
                Console.WriteLine("Course is already published.");
                return;
            }

            course.IsPublished = true;

            course.PublishedAt = DateTime.Now;

            context.SaveChanges();

            Console.WriteLine(
                $"{course.Title} published at {course.PublishedAt}");
        }

        static void Unenroll(
            int studentId,
            int courseId)
        {
            using var context = new PlatformDbContext();

            var enrollment = context.Enrollments
                .Find(studentId, courseId);

            if (enrollment == null)
            {
                Console.WriteLine("Enrollment not found.");
                return;
            }

            context.Enrollments.Remove(enrollment);

            context.SaveChanges();

            Console.WriteLine("Enrollment deleted.");
        }

        static void DeleteCategoryWithException(int categoryId)
        {
            using var context = new PlatformDbContext();

            var category = context.Categories.Find(categoryId);

            if (category == null)
            {
                Console.WriteLine("Category not found.");
                return;
            }

            try
            {
                context.Categories.Remove(category);

                context.SaveChanges();

                Console.WriteLine("Category deleted.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(
                    "Cannot delete category because related records exist.");

                Console.WriteLine(ex.Message);
            }
        }

        static void DeleteCategorySafely(int categoryId)
        {
            using var context = new PlatformDbContext();

            var category = context.Categories.Find(categoryId);

            if (category == null)
            {
                Console.WriteLine("Category not found.");
                return;
            }

            bool hasSubCategories = context.Categories
                .Any(c => c.ParentCategoryId == categoryId);

            if (hasSubCategories)
            {
                Console.WriteLine(
                    "Cannot delete category: it has sub-categories.");

                return;
            }

            bool hasCourses = context.Courses
                .Any(c => c.CategoryId == categoryId);

            if (hasCourses)
            {
                Console.WriteLine(
                    "Cannot delete category: it has courses.");

                return;
            }

            context.Categories.Remove(category);

            context.SaveChanges();

            Console.WriteLine("Category deleted safely.");
        }

        static void DeleteCourse(int courseId)
        {
            using var context = new PlatformDbContext();

            var course = context.Courses.Find(courseId);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            int before = context.Enrollments
                .Count(e => e.CourseId == courseId);

            context.Courses.Remove(course);

            context.SaveChanges();

            // Enrollments are deleted because the relationship uses Cascade.

            int after = context.Enrollments
                .Count(e => e.CourseId == courseId);

            Console.WriteLine(
                $"Enrollments before: {before}");

            Console.WriteLine(
                $"Enrollments after: {after}");
        }

        static void BulkPriceRaise(CourseLevel level)
        {
            if (!Enum.IsDefined(typeof(CourseLevel), level))
            {
                Console.WriteLine("Invalid course level.");
                return;
            }

            using var context = new PlatformDbContext();

            bool priceTooLarge = context.Courses
                .Any(c => c.IsPublished
                    && c.Level == level
                    && c.Price > 99999999.99m / 1.10m);

            if (priceTooLarge)
            {
                Console.WriteLine(
                    "The increased price would exceed the allowed limit.");

                return;
            }

            int affectedRows = context.Courses
                .Where(c => c.IsPublished && c.Level == level)
                .ExecuteUpdate(s => s.SetProperty(
                    c => c.Price,
                    c => c.Price * 1.10m));

            Console.WriteLine(
                $"Price raise: {affectedRows} affected rows.");
        }

        static void BulkDeleteDropped()
        {
            using var context = new PlatformDbContext();

            int affectedRows = context.Enrollments
                .Where(e => e.Status == EnrollmentStatus.Dropped)
                .ExecuteDelete();

            Console.WriteLine(
                $"Dropped enrollments deleted: {affectedRows}");

        }
    }
}
