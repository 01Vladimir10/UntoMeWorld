using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.MongoDatabase.Models
{
    public class MongoChurch : IChurch
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
        #nullable enable
        public IAddress? Address { get; set; }
        public string? PastorId { get; set; }
        #nullable disable
    }
}