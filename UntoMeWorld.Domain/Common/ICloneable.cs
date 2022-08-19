namespace UntoMeWorld.Domain.Common
{
    public interface ICloneable<out T> where T : class
    {
        public T Clone();
    }
}