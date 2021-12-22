using System;

namespace UntoMeWorld.Domain.Model
{
    public interface IModel 
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime DeletedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}