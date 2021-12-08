namespace UntoMeWorld.Domain.Model
{
    public class Child : IPerson
    {
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
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
    }
}