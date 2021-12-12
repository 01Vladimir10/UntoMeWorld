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

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Phone)}: {Phone}, {nameof(Name)}: {Name}, {nameof(Lastname)}: {Lastname}, {nameof(Age)}: {Age}, {nameof(Gender)}: {Gender}";
        }
    }
}