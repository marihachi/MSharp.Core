using System.Threading.Tasks;

namespace MSharp.Sample
{
	public class TestClass
	{
		public TestClass()
		{
			Task.Run(async () =>
			{
				await Task.Delay(5000);
			}).Wait();
		}
	}
}
