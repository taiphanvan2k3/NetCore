namespace App
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Gpa { get; set; }

        // Khi override Equals thì override GetHashCode
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;
            Student tmp = obj as Student;
            return tmp.Id == this.Id;
        }

        public override int GetHashCode()
        {
            System.Console.WriteLine(Id.GetHashCode());
            System.Console.WriteLine(Name.GetHashCode());
            return Id.GetHashCode() + Name.GetHashCode();
        }
    }
}