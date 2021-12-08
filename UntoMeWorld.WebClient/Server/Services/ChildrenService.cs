using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;

namespace UntoMeWorld.WebClient.Server.Services
{
    public class ChildrenService
    {
        private readonly IChildrenStore _children;

        public ChildrenService(IChildrenStore children)
        {
            _children = children;
        }

        public Task<Child> AddChild(Child child)
        {
            return _children.Add(child);
        }
        
        public Task<IEnumerable<Child>> AddChild(List<Child> child)
        {
            return _children.Add(child);
        }
        
        public Task<IEnumerable<Child>> UpdateChild(List<Child> child)
        {
            return _children.Update(child);
        }

        public Task<Child> UpdateChild(Child child)
        {
            return _children.Add(child);
        }

        public Task DeleteChild(List<string> child)
        {
            return _children.Delete(child.Select(c => new Child {Id = c}));
        }
        public Task DeleteChild(string child)
        {
            return _children.Delete(new Child {Id = child});
        }

        public Task<IEnumerable<Child>> GetAllChildren()
        {
            return _children.All();
        }
        
        public Task<IEnumerable<Child>> GetAllChildren(string query)
        {
            return _children.All(query);
        }
        
    }
}