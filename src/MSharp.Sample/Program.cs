using System;
using System.Threading;
using System.Windows.Forms;

namespace MSharp.Sample
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// UIスレッドの例外
			Application.ThreadException += new ThreadExceptionEventHandler(async(sender, ev) =>
			{
				var ex = ev.Exception;

				var error = "ThreadExceptionからの通知:\r\n";
				error += $"{Log.GetErrorMessageFromException(ex)}";

				await Log.AddAsync(error);
				MessageBox.Show($"{error}", ex.GetType().Name);
			});

			// UIスレッド以外の例外
			Thread.GetDomain().UnhandledException += new UnhandledExceptionEventHandler(async(sender, ev) =>
			{
				var ex = ev.ExceptionObject as Exception;
				if (ex != null)
				{
					var error = "UnhandledExceptionからの通知:\r\n";
					error += $"{Log.GetErrorMessageFromException(ex)}";

					await Log.AddAsync(error);
					MessageBox.Show($"{error}", ex.GetType().Name);
				}
			});

			Application.Run(new Form1());
		}
	}
}
