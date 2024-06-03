using Dapper;
using Pracrise_Dapper.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Pracrise_Dapper
{
    internal class ServiceSchool
    {
        private readonly string _connectionString;
        public ServiceSchool(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void addStudent(Students student)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "insert into Students(FirstName, LastName, BirthDate) " +
                    "values(@FirstName, @LastName, @BirthDate) ";
                connection.Execute(sql, student);

            }
        }
        public void addCourse(Course course)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "insert into Courses(CourseName, Description) " +
                    "values(@CourseName, @Description) ";
                connection.Execute(sql, course);

            }
        }
        public void EnrollStudentInCourse(int studentId, int courseId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "insert into StudentCourses(StudentId, CourseId) " +
                    "values(@StudentId, @CourseId) ";
                connection.Execute(sql,new {StudentId = studentId, CourseId = courseId});

            }
        }
        public Students GetStudent(int id)
        {
            using (var connection = new SqlConnection(_connectionString)) // Открытие соединения
            {
                var sql = "SELECT * FROM Students WHERE Id = @Id";
                return connection.QueryFirstOrDefault<Students>(sql, new { Id = id }); // Выполнение SQL запроса и возврат результата
            }
        }

        // Метод для получения курса по Id
        public Course GetCourse(int id)
        {
            using (var connection = new SqlConnection(_connectionString)) // Открытие соединения
            {
                var sql = "SELECT * FROM Courses WHERE Id = @Id";
                return connection.QueryFirstOrDefault<Course>(sql, new { Id = id }); // Выполнение SQL запроса и возврат результата
            }
        }

        // Метод для получения всех студентов
        public IEnumerable<Students> GetAllStudents()
        {
            using (var connection = new SqlConnection(_connectionString)) // Открытие соединения
            {
                var sql = "SELECT * FROM Students";
                return connection.Query<Students>(sql).ToList(); // Выполнение SQL запроса и возврат списка студентов
            }
        }

        // Метод для получения всех курсов
        public IEnumerable<Course> GetAllCourses()
        {
            using (var connection = new SqlConnection(_connectionString)) // Открытие соединения
            {
                var sql = "SELECT * FROM Courses";
                return connection.Query<Course>(sql).ToList(); // Выполнение SQL запроса и возврат списка курсов
            }
        }

        // Метод для получения курсов студента по его Id
        public IEnumerable<Course> GetCoursesByStudent(int studentId)
        {
            using (var connection = new SqlConnection(_connectionString)) // Открытие соединения
            {
                var sql = @"SELECT c.* 
                        FROM Courses c
                        JOIN StudentCourses sc ON c.Id = sc.CourseId
                        WHERE sc.StudentId = @StudentId";
                return connection.Query<Course>(sql, new { StudentId = studentId }).ToList(); // Выполнение SQL запроса и возврат списка курсов
            }
        }

        // Метод для обновления информации о студенте
        public void UpdateStudent(Students student)
        {
            using (var connection = new SqlConnection(_connectionString)) // Открытие соединения
            {
                var sql = "UPDATE Students SET FirstName = @FirstName, LastName = @LastName, BirthDate = @BirthDate WHERE Id = @Id";
                connection.Execute(sql, student); // Выполнение SQL запроса для обновления данных студента
            }
        }

        // Метод для обновления информации о курсе
        public void UpdateCourse(Course course)
        {
            using (var connection = new SqlConnection(_connectionString)) // Открытие соединения
            {
                var sql = "UPDATE Courses SET CourseName = @CourseName, Description = @Description WHERE Id = @Id";
                connection.Execute(sql, course); // Выполнение SQL запроса для обновления данных курса
            }
        }

        // Метод для удаления студента по его Id
        public void DeleteStudent(int id)
        {
            using (var connection = new SqlConnection(_connectionString)) // Открытие соединения
            {
                var sql = "DELETE FROM Students WHERE Id = @Id";
                connection.Execute(sql, new { Id = id }); // Выполнение SQL запроса для удаления студента
            }
        }

        // Метод для удаления курса по его Id
        public void DeleteCourse(int id)
        {
            using (var connection = new SqlConnection(_connectionString)) // Открытие соединения
            {
                var sql = "DELETE FROM Courses WHERE Id = @Id";
                connection.Execute(sql, new { Id = id }); // Выполнение SQL запроса для удаления курса
            }
        }
        public IEnumerable<Students> GetStudentsWithLastNameStartingWithD()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Students WHERE LastName LIKE 'D%'";
                return connection.Query<Students>(sql).ToList();
            }
        }
        public IEnumerable<Course> GetCoursesContainingIntroduction()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Courses WHERE CourseName LIKE '%Introduction%'";
                return connection.Query<Course>(sql).ToList();
            }
        }
        public IEnumerable<Students> GetStudentsBornAfter2005()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Students WHERE BirthDate > '2005-01-01'";
                return connection.Query<Students>(sql).ToList();
            }
        }
        public IEnumerable<Course> GetCoursesByStudentName(string firstName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
            SELECT c.* 
            FROM Courses c
            JOIN StudentCourses sc ON c.Id = sc.CourseId
            JOIN Students s ON s.Id = sc.StudentId
            WHERE s.FirstName = @FirstName";
                return connection.Query<Course>(sql, new { FirstName = firstName }).ToList();
            }
        }
        public IEnumerable<Students> GetStudentsByCourseName(string courseName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
            SELECT s.* 
            FROM Students s
            JOIN StudentCourses sc ON s.Id = sc.StudentId
            JOIN Courses c ON c.Id = sc.CourseId
            WHERE c.CourseName = @CourseName";
                return connection.Query<Students>(sql, new { CourseName = courseName }).ToList();
            }
        }
        public Course GetCourseWithMostStudents()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
                SELECT TOP 1 c.*, COUNT(sc.StudentId) AS StudentCount 
                FROM Courses c
                JOIN StudentCourses sc ON c.Id = sc.CourseId
                GROUP BY c.Id, c.CourseName, c.Description
                ORDER BY COUNT(sc.StudentId) DESC";
                return connection.QueryFirstOrDefault<Course>(sql);
            }
        }

        // 7. Получить среднее количество курсов на одного студента
        public double GetAverageCoursesPerStudent()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
                SELECT AVG(CourseCount) 
                FROM (
                    SELECT COUNT(sc.CourseId) AS CourseCount
                    FROM Students s
                    LEFT JOIN StudentCourses sc ON s.Id = sc.StudentId
                    GROUP BY s.Id
                ) AS CourseCounts";
                return connection.ExecuteScalar<double>(sql);
            }
        }

        // 8. Получить студентов, которые записаны на все курсы
        public IEnumerable<Students> GetStudentsEnrolledInAllCourses()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
                SELECT s.*
                FROM Students s
                WHERE NOT EXISTS (
                    SELECT c.Id
                    FROM Courses c
                    WHERE NOT EXISTS (
                        SELECT sc.CourseId
                        FROM StudentCourses sc
                        WHERE sc.CourseId = c.Id AND sc.StudentId = s.Id
                    )
                )";
                return connection.Query<Students>(sql).ToList();
            }
        }

        // 9. Получить курс с наименьшим количеством студентов
        public Course GetCourseWithLeastStudents()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
                SELECT TOP 1 c.*, COUNT(sc.StudentId) AS StudentCount 
                FROM Courses c
                JOIN StudentCourses sc ON c.Id = sc.CourseId
                GROUP BY c.Id, c.CourseName, c.Description
                ORDER BY COUNT(sc.StudentId)";
                return connection.QueryFirstOrDefault<Course>(sql);
            }
        }

        // 10. Получить всех студентов, не записанных ни на один курс
        public IEnumerable<Students> GetStudentsNotEnrolledInAnyCourse()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
                SELECT s.*
                FROM Students s
                LEFT JOIN StudentCourses sc ON s.Id = sc.StudentId
                WHERE sc.StudentId IS NULL";
                return connection.Query<Students>(sql).ToList();
            }
        }
        //________________________dz__________________________
        public IEnumerable<Students> GetStudentsBornInJanuary()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Students WHERE MONTH(BirthDate) = 1";
                return connection.Query<Students>(sql).ToList();
            }
        }
        public IEnumerable<Course> GetCoursesWithMoreThan5Students()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
        SELECT c.* 
        FROM Courses c
        JOIN StudentCourses sc ON c.Id = sc.CourseId
        GROUP BY c.Id, c.CourseName, c.Description
        HAVING COUNT(sc.StudentId) > 5";
                return connection.Query<Course>(sql).ToList();
            }
        }
        public IEnumerable<IGrouping<int, Students>> GetStudentsGroupedByBirthYear()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Students";
                var students = connection.Query<Students>(sql).ToList();
                return students.GroupBy(s => s.BirthDate.Year);
            }
        }
        public IEnumerable<Students> GetStudentsWithLastNameEndingInS()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Students WHERE LastName LIKE '%s'";
                return connection.Query<Students>(sql).ToList();
            }
        }
        public IEnumerable<Course> GetCoursesWithOneOrTwoStudents()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
        SELECT c.* 
        FROM Courses c
        JOIN StudentCourses sc ON c.Id = sc.CourseId
        GROUP BY c.Id, c.CourseName, c.Description
        HAVING COUNT(sc.StudentId) IN (1, 2)";
                return connection.Query<Course>(sql).ToList();
            }
        }
        public IEnumerable<Students> GetStudentsEnrolledInMoreThan2Courses()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
        SELECT s.*
        FROM Students s
        JOIN StudentCourses sc ON s.Id = sc.StudentId
        GROUP BY s.Id, s.FirstName, s.LastName, s.BirthDate
        HAVING COUNT(sc.CourseId) > 2";
                return connection.Query<Students>(sql).ToList();
            }
        }
        public IEnumerable<Students> GetStudentsWithLongNames()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Students WHERE LEN(FirstName) > 4";
                return connection.Query<Students>(sql).ToList();
            }
        }
        public IEnumerable<Course> GetCoursesStartingWithC()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Courses WHERE CourseName LIKE 'C%'";
                return connection.Query<Course>(sql).ToList();
            }
        }
        public IEnumerable<Students> GetStudentsBornInLeapYear()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
        SELECT * 
        FROM Students 
        WHERE (YEAR(BirthDate) % 4 = 0 AND YEAR(BirthDate) % 100 <> 0) OR (YEAR(BirthDate) % 400 = 0)";
                return connection.Query<Students>(sql).ToList();
            }
        }
        public IEnumerable<Students> GetStudentsWithSameFirstName()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
        SELECT s.*
        FROM Students s
        JOIN (
            SELECT FirstName
            FROM Students
            GROUP BY FirstName
            HAVING COUNT(*) > 1
        ) dup ON s.FirstName = dup.FirstName";
                return connection.Query<Students>(sql).ToList();
            }
        }
        public IEnumerable<Course> GetCoursesWithStudentsOlderThan20()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
        SELECT DISTINCT c.*
        FROM Courses c
        JOIN StudentCourses sc ON c.Id = sc.CourseId
        JOIN Students s ON s.Id = sc.StudentId
        WHERE DATEDIFF(YEAR, s.BirthDate, GETDATE()) > 20";
                return connection.Query<Course>(sql).ToList();
            }
        }
        public IEnumerable<Students> GetStudentsNotOlderThan22()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Students WHERE DATEDIFF(YEAR, BirthDate, GETDATE()) <= 22";
                return connection.Query<Students>(sql).ToList();
            }
        }
        public IEnumerable<Students> GetStudentsSortedByBirthDate()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Students ORDER BY BirthDate DESC";
                return connection.Query<Students>(sql).ToList();
            }
        }
        public IEnumerable<Students> GetStudentsEnrolledInScienceCourses()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
        SELECT DISTINCT s.*
        FROM Students s
        JOIN StudentCourses sc ON s.Id = sc.StudentId
        JOIN Courses c ON c.Id = sc.CourseId
        WHERE c.CourseName LIKE '%Science%'";
                return connection.Query<Students>(sql).ToList();
            }
        }
        public double GetAverageAgeOfStudentsInBiology()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
        SELECT AVG(DATEDIFF(YEAR, BirthDate, GETDATE()))
        FROM Students s
        JOIN StudentCourses sc ON s.Id = sc.StudentId
        JOIN Courses c ON c.Id = sc.CourseId
        WHERE c.CourseName = 'Biology'";
                return connection.ExecuteScalar<double>(sql);
            }
        }
    }
}
