namespace UntoMeWorld.Domain.Model
{
    public class Pastor : IPerson
    {
        public string Id { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
    }
}