namespace UntoMeWorld.Domain.Model
{
    public interface IPerson
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public Sex Sex { get; set; }
    }
}