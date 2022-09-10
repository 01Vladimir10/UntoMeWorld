namespace UntoMeWorld.Domain.Model.Abstractions
{
    public interface ICloneable<out T> where T : class
    {
        public T Clone();
    }
}