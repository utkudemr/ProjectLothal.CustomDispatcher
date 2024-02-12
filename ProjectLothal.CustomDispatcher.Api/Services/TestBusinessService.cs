namespace ProjectLothal.CustomDispatcher.Api.Services
{
    public class TestBusinessService : ITestBusinessService
    {
        public void TestMethod()
        {
            Console.Write("Helü");
        }
    }

    public interface ITestBusinessService
    {
        void TestMethod();
    }
}
