using Pracrise_Dapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pracrise_Dapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionString = @"Server=DESKTOP-0BAM1G2\SQLEXPRESS;Database=SchoolDb;Integrated Security=True;";
            var service = new ServiceSchool(connectionString);
            // 1. Получить всех студентов, у которых фамилия начинается с буквы 'D'
            var studentsWithLastNameStartingWithD = service.GetStudentsWithLastNameStartingWithD();
            Console.WriteLine("Students with last name starting with 'D':");
            foreach (var student in studentsWithLastNameStartingWithD)
            {
                Console.WriteLine($"{student.FirstName} {student.LastName}");
            }

            // 2. Получить все курсы, название которых содержит слово "Introduction"
            var coursesContainingIntroduction = service.GetCoursesContainingIntroduction();
            Console.WriteLine("\nCourses containing 'Introduction':");
            foreach (var course in coursesContainingIntroduction)
            {
                Console.WriteLine(course.CourseName);
            }

            // 3. Получить студентов, родившихся после 2005 года
            var studentsBornAfter2005 = service.GetStudentsBornAfter2005();
            Console.WriteLine("\nStudents born after 2005:");
            foreach (var student in studentsBornAfter2005)
            {
                Console.WriteLine($"{student.FirstName} {student.LastName} - {student.BirthDate.ToShortDateString()}");
            }

            // 4. Получить курсы, на которые записан студент с именем 'Jane'
            var coursesByStudentName = service.GetCoursesByStudentName("Jane");
            Console.WriteLine("\nCourses taken by student named 'Jane':");
            foreach (var course in coursesByStudentName)
            {
                Console.WriteLine(course.CourseName);
            }

            // 5. Получить всех студентов, которые записаны на курс "Mathematics"
            var studentsByCourseName = service.GetStudentsByCourseName("Mathematics");
            Console.WriteLine("\nStudents enrolled in 'Mathematics' course:");
            foreach (var student in studentsByCourseName)
            {
                Console.WriteLine($"{student.FirstName} {student.LastName}");
            }
            // 6. Получить курс с наибольшим количеством студентов
            var courseWithMostStudents = service.GetCourseWithMostStudents();
            Console.WriteLine("\nCourse with most students:");
            if (courseWithMostStudents != null)
            {
                Console.WriteLine(courseWithMostStudents.CourseName);
            }

            // 7. Получить среднее количество курсов на одного студента
            var averageCoursesPerStudent = service.GetAverageCoursesPerStudent();
            Console.WriteLine($"\nAverage number of courses per student: {averageCoursesPerStudent}");

            // 8. Получить студентов, которые записаны на все курсы
            var studentsEnrolledInAllCourses = service.GetStudentsEnrolledInAllCourses();
            Console.WriteLine("\nStudents enrolled in all courses:");
            foreach (var student in studentsEnrolledInAllCourses)
            {
                Console.WriteLine($"{student.FirstName} {student.LastName}");
            }

            // 9. Получить курс с наименьшим количеством студентов
            var courseWithLeastStudents = service.GetCourseWithLeastStudents();
            Console.WriteLine("\nCourse with least students:");
            if (courseWithLeastStudents != null)
            {
                Console.WriteLine(courseWithLeastStudents.CourseName);
            }

            // 10. Получить всех студентов, не записанных ни на один курс
            var studentsNotEnrolledInAnyCourse = service.GetStudentsNotEnrolledInAnyCourse();
            Console.WriteLine("\nStudents not enrolled in any course:");
            foreach (var student in studentsNotEnrolledInAnyCourse)
            {
                Console.WriteLine($"{student.FirstName} {student.LastName}");
            }
            Console.ReadLine();
        }
    }
}
