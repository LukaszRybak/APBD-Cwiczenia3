namespace StudentsController.Models
{
    public class Student
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string IndexNumber { get; set; }
        public string Birthdate { get; set; }
        public string Studies { get; set; }
        public string Mode { get; set; }
        public string Email { get; set; }
        public string MothersName { get; set; }
        public string FathersName { get; set; }


        public Student(
            string fname,
            string lname,
            string indexNumber,
            string birthdate,
            string studies,
            string mode,
            string email,
            string mothersName,
            string fathersName
            )
        {
            this.Fname = fname;
            this.Lname = lname;
            this.IndexNumber = indexNumber;
            this.Birthdate = birthdate;
            this.Studies = studies;
            this.Mode = mode;
            this.Email = email;
            this.MothersName = mothersName;
            this.FathersName = fathersName;
        }
    }
}
