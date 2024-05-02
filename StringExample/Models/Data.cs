namespace StringExample.Models
{
    public class Data
    {
        public string Faculty { get; set; }

        public string SchoolYear { get; set; }

        public string CadreName { get; set; }

        public string Position { get; set; }

        public string CadreFaculty { get; set; }

        public List<LookUpDto> SelfEvaluation { get; set; }

        public List<LookUpDto> LeaderEvaluation { get; set; }
    }

    public class LookUpDto
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}