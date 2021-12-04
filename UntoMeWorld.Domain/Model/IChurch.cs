#nullable enable
namespace UntoMeWorld.Domain.Model
{
    public interface IChurch
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IAddress? Address { get; set; }
        public string PastorId { get; set; }
    }
}