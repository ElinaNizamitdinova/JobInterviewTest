using JobInterviewTest;

namespace InterviewTest
{
    [TestClass]
    public class InteviewTest
    {
        string testlLogPath = "E:\\elina\\JobInterviewTest\\InterviewTest\\TestFile.txt";
        string orderFile = "E:\\elina\\JobInterviewTest\\JobInterviewTest\\orders.csv";
        string testMess = "Test";
        List<Order> orders = new List<Order>();

        [TestMethod]
        public void TestLog()
        {
            Logger.Log(testlLogPath, testMess);
          
        }

        [TestMethod]
        public void TestLoadOrders()
        {
            Order.LoadOrders(orderFile,testlLogPath);

        }
    }
}