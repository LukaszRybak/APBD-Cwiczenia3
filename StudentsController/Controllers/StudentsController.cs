using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentsController.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;


namespace StudentsController.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        private static List<Student> GetStudentList()
        {
            string dbPath = Directory.GetCurrentDirectory() + "\\data\\dane.csv";
            List<string[]> list = new();
            List<Student> studentList = new();

            using (var reader = new StreamReader(dbPath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var value = line.Split(';');
                    list.Add(value);

                }
            }

            foreach (string[] item in list)
            {
                var fname = item[0];
                var lname = item[1];
                var studentIndex = item[2];
                var birthdate = item[3];
                var studies = item[4];
                var mode = item[5];
                var email = item[6];
                var fathersName = item[7];
                var mothersName = item[8];

                var student = new Student(fname, lname, studentIndex, birthdate, studies, mode, email, mothersName, fathersName);

                studentList.Add(student);
            }

            return studentList;
        }
        private static void SaveStudentList(List<Student> studentList)
        {
            string dbPath = Directory.GetCurrentDirectory() + "\\data\\dane.csv";

            using (var writer = new StreamWriter(dbPath))
            {
                foreach (Student student in studentList)
                {
                    writer.WriteLine(
                        string.Join(";",
                            student.Fname,
                            student.Lname,
                            student.IndexNumber,
                            student.Birthdate,
                            student.Studies,
                            student.Mode,
                            student.Email,
                            student.MothersName,
                            student.FathersName
                        )
                    );
                }
            }
        }

        private static bool IsValidIndexNumber(string indexNumber)
        {
            var pattern = "^s[0-9]+$";
            var regex = new Regex(pattern);
            return regex.IsMatch(indexNumber);
        }

        [HttpGet("{indexNumber?}")]
        public IActionResult GetStudents(string? indexNumber = null)
        {

            List<Student> studentList = GetStudentList();


            if (indexNumber != null)
            {
                var specificStudent = studentList.Find(p => p.IndexNumber == indexNumber);
                if (specificStudent == null)
                {
                    return NotFound("Could not find student with given index number");
                }
                var specificStudentJson = JsonConvert.SerializeObject(specificStudent, Newtonsoft.Json.Formatting.Indented);
                return Ok(specificStudentJson);
            }
            else
            {
                var studentListJson = JsonConvert.SerializeObject(studentList, Newtonsoft.Json.Formatting.Indented);
                return Ok(studentListJson);
            }
        }


        [HttpPut("{indexNumber}")]
        public IActionResult UpdateStudent(string indexNumber, Student updatedStudent)
        {
           
            List<Student> studentList = GetStudentList();

            var studentToUpdate = studentList.Find(p => p.IndexNumber == indexNumber);
            if (studentToUpdate == null)
            {
                return NotFound("Could not find student with given index number");
            }

            studentToUpdate.Fname = updatedStudent.Fname;
            studentToUpdate.Lname = updatedStudent.Lname;
            studentToUpdate.Birthdate = updatedStudent.Birthdate;
            studentToUpdate.Studies = updatedStudent.Studies;
            studentToUpdate.Mode = updatedStudent.Mode;
            studentToUpdate.Email = updatedStudent.Email;
            studentToUpdate.FathersName = updatedStudent.FathersName;
            studentToUpdate.MothersName = updatedStudent.MothersName;

            SaveStudentList(studentList);

            var studentToUpdateJson = JsonConvert.SerializeObject(studentToUpdate, Newtonsoft.Json.Formatting.Indented);

            return Ok(studentToUpdateJson);
        }

        [HttpPost]
        public IActionResult AddStudent(Student newStudent)
        {
            if (string.IsNullOrEmpty(newStudent.Fname) ||
                string.IsNullOrEmpty(newStudent.Lname) ||
                string.IsNullOrEmpty(newStudent.IndexNumber) ||
                string.IsNullOrEmpty(newStudent.Birthdate) ||
                string.IsNullOrEmpty(newStudent.Studies) ||
                string.IsNullOrEmpty(newStudent.Mode) ||
                string.IsNullOrEmpty(newStudent.Email) ||
                string.IsNullOrEmpty(newStudent.MothersName) ||
                string.IsNullOrEmpty(newStudent.FathersName))
            {
                return BadRequest("All student fields must be provided");
            }

            if (!IsValidIndexNumber(newStudent.IndexNumber))
            {
                return BadRequest("Invalid index number format");
            }

            List<Student> studentList = GetStudentList();
            if (studentList.Any(s => s.IndexNumber == newStudent.IndexNumber))
            {
                return BadRequest("Index number must be unique");
            }

            studentList.Add(newStudent);
            SaveStudentList(studentList);

            return Ok("New student added successfully");
        }

        [HttpDelete("{indexNumber}")]
        public IActionResult DeleteStudent(string indexNumber)
        {
            List<Student> studentList = GetStudentList();
            var studentToDelete = studentList.Find(s => s.IndexNumber == indexNumber);
            if (studentToDelete == null)
            {
                return NotFound("Could not find student with given index number");
            }
            studentList.Remove(studentToDelete);
            SaveStudentList(studentList);

            return Ok("Student removed successfully");
        }

        [HttpGet("random")]
        public IActionResult GetRandomStudent()
        {
            List<Student> studentList = GetStudentList();
            if (studentList.Count == 0)
            {
                return NoContent();
            }
            var random = new Random();
            var randomIndex = random.Next(studentList.Count);
            var randomStudent = studentList[randomIndex];
            var randomStudentJson = JsonConvert.SerializeObject(randomStudent, Newtonsoft.Json.Formatting.Indented);

            return Ok(randomStudentJson);
        }


    }
}
