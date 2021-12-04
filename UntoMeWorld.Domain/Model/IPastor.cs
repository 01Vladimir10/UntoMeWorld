namespace UntoMeWorld.Domain.Model
{
    public interface IPastor : IPerson
    {
        public string Id { get; set; }
        public string Phone { get; set; }
    }
}