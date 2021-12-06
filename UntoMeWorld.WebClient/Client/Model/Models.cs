using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WebClient.Client.Model
{
    public class IChurch : Domain.Model.IChurch
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IAddress? Address { get; set; }
        public string? PastorId { get; set; }
    }
}