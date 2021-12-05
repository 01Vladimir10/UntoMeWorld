using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WebClient.Client.Model
{
    public class Church : IChurch
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IAddress? Address { get; set; }
        public string? PastorId { get; set; }
    }
}