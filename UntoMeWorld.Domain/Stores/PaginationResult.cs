using System.Collections.Generic;

namespace UntoMeWorld.Domain.Stores
{
    public class PaginationResult<T>
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public List<T> Result { get; set; }
    }
}