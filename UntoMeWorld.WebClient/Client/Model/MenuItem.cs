using System.Collections.Generic;

namespace UntoMeWorld.WebClient.Client.Model
{
    public class MenuItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsSelected { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public List<MenuItem> Items { get; set; } = new();
    }
}