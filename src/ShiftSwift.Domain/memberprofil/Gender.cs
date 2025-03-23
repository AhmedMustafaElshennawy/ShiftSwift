namespace ShiftSwift.Domain.memberprofil
{

    public enum GenderType
    {
        Male = 1,
        Female = 2,
        Other = 3
    }
    public class Gender
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        private Gender(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static readonly Gender Male = new(1, "Male");
        public static readonly Gender Female = new(2, "Female");
        public static readonly Gender Other = new(3, "Other");

        public static IEnumerable<Gender> GetAll() => new[] { Male, Female, Other };

        public static Gender FromId(int id) => GetAll().FirstOrDefault(g => g.Id == id)
            ?? throw new ArgumentException("Invalid Gender Id");

        public override string ToString() => Name;
    }
}