using System;

namespace UntoMeWorld.Domain.Model.Abstractions
{
    public interface IRecyclableModel
    {
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}