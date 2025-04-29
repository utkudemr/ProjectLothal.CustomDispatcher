namespace ProjectLothal.CustomDispatcher.Api.Decorators
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public abstract class BaseAttribute : Attribute
    {
        public abstract Type GetDecoratorType();
    }
}
