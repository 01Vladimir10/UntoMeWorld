using System;

namespace UntoMeWorld.Domain.Model
{
    public interface IRecyclableModel
    {
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}