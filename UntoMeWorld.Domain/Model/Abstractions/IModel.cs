using System;

namespace UntoMeWorld.Domain.Model.Abstractions
{
    public interface IModel 
    {
        public string Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}