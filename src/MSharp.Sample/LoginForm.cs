using System;
using System.Windows.Forms;

namespace MSharp.Sample
{
	public partial class LoginForm : Form
	{
		public LoginForm()
		{
			InitializeComponent();
			DialogResult = DialogResult.None;
		}

		public Misskey Content { get; set; }

		private async void loginButton_Click(object sender, EventArgs e)
		{
			var result = await new Misskey().Authorize(screenNameBox.Text, passwordBox.Text);

			if (result != null)
			{
				Content = result;
				DialogResult = DialogResult.OK;
				Close();
			}
			else
				MessageBox.Show("ログインに失敗しました。入力内容を確認してください。");
		}
	}
}
