using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WebClient.Shared.Model
{
    public class Church : IChurch
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IAddress Address { get; set; }
        public string PastorId { get; set; }
    }
    public class Pastor : IPastor 
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Id { get; set; }
        public string Phone { get; set; }
    }
    public class Child : IChild
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Id { get; set; }
        public int Grade { get; set; }
        public string ChurchId { get; set; }
        public int ShoeSize { get; set; }
        public int TopSize { get; set; }
        public int WaistSize { get; set; }
        public UnderwearSize UnderwearSize { get; set; }
        public UnderwearSize BraSize { get; set; }
        public int UniformsCount { get; set; }
        public bool ReceivesChristmasGift { get; set; }
        public int ReceivesUniforms { get; set; }
    }
}