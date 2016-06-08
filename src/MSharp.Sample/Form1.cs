using MSharp.Data;
using MSharp.Data.Entity;
using MSharp.Data.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSharp.Sample
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private Misskey _Misskey { get; set; }

		private string embedFile { get; set; }

		private void ExecuteFromPostType(PostEntity post, Action<StatusEntity> whenStatus, Action<ReplyEntity> whenReply, Action<RepostEntity> whenRepost)
		{
			if (post.Type == PostType.Status)
			{
				whenStatus(post as StatusEntity);
			}
			else if (post.Type == PostType.Reply)
			{
				whenReply(post as ReplyEntity);
			}
			else if (post.Type == PostType.Repost)
			{
				whenRepost(post as RepostEntity);
			}
			else
				throw new Exception("ポストタイプが不明です。");
		}

		private string getDisplayPostText(PostEntity post)
		{
			var res = "";

			ExecuteFromPostType(post,
			(p) =>
			{
				res = $"[Status]\r\n@{p.User.ScreenName}\r\nText: {p.Text}";
			},
			async (p) =>
			{
				var inReplyToPost = await p.InReplyToPost;
				res = $"[Reply]\r\n@{p.User.ScreenName}\r\nText: {p.Text}\r\nReplyTo: @{inReplyToPost.User.ScreenName}";
			},
			(p) =>
			{
				var repostPost = p.TargetPost as StatusEntity;
				res = $"[Repost]\r\n@{repostPost.User.ScreenName}\r\nText: {repostPost.Text}\r\nRepostBy: @{p.User.ScreenName}";
			});

			return res;
		}

		private void addText(Control target, object text)
		{
			target.Text = target.Text.Insert(0, $"{text.ToString()}\r\n");
		}

		private async void Form1_Load(object sender, EventArgs e)
		{
			var loginForm = new LoginForm();
			if (loginForm.ShowDialog() != DialogResult.OK)
			{
				Close();
				return;
			}

			_Misskey = loginForm.Content;

			var timelinePosts = await _Misskey.Posts.HomeTimeLine(30);

			var items = new List<ListViewItem>();
			foreach (var post in timelinePosts)
			{
				ExecuteFromPostType(post,
				(p) => { items.Add(new ListViewItem(new string[] { p.Type.ToString(), p.User.ScreenName, p.Text, p.CreatedAt.ToString() })); },
				(p) => { items.Add(new ListViewItem(new string[] { p.Type.ToString(), p.User.ScreenName, p.Text, p.CreatedAt.ToString() })); },
				(p) => {
					var target = p.TargetPost as StatusEntity;
					items.Add(new ListViewItem(new string[] { p.Type.ToString(), $"@{target.User.ScreenName} (RP:@{p.User.ScreenName})", target.Text, p.CreatedAt.ToString() }));
				});
			}
			timelineListView.Items.AddRange(items.ToArray());

			var homeStream = _Misskey.HomeStream;

			homeStream.StreamConnected += (s, ev) =>
			{
				addText(textBox3, "接続されたよ");

				addText(button2, "Disconnect Stream");
				button2.Enabled = true;
			};

			homeStream.StreamDisconnected += (s, ev) =>
			{
				addText(textBox3, "切断されたよ");

				addText(button2, "Connect Stream");
				button2.Enabled = true;
			};

			homeStream.PostRecieved += (s, ev) =>
			{
				var post = ev.Data;
				addText(textBox1, getDisplayPostText(post));
			};

			homeStream.NotificationRecieved += (s, ev) =>
			{
				var notif = ev.Data;

				if (notif.Type == NotificationType.Like)
				{
					var like = notif as LikeNotification;

					addText(textBox2, $"UserId{like.UserId}のユーザーにFavされました");
				}

				if (notif.Type == NotificationType.Repost)
				{
					var repost = notif as RepostNotification;

					addText(textBox2, $"UserId{repost.UserId}のユーザーにRPされました");
				}
			};
		}

		private async void postButton_Click(object sender, EventArgs e)
		{
			var imageFile = embedFile != null ? new ImageFile(embedFile) : null;

			try
			{
				var res = await _Misskey.Posts.Create(postBox.Text, imageFile);
				addText(textBox3, $"ポストが投稿されました。Type:{res.Type}, Id:{res.Id}, CreatedAt:{res.CreatedAt}");
			}
			catch (Exception ex)
			{
				addText(textBox3, $"投稿に失敗しました。{ex.Message}\r\n");
			}
		}

		private async void button2_Click(object sender, EventArgs e)
		{
			button2.Enabled = false;

			if (!_Misskey.HomeStream.IsConnecting)
				await _Misskey.HomeStream.ConnectAsync(true);
			else
				_Misskey.HomeStream.Disconnect();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				embedFile = dialog.FileName;
				label2.Text = dialog.SafeFileName;
				addText(textBox3, $"ファイル'{dialog.SafeFileName}'が添付されました。\r\n");
			}
			else
			{
				embedFile = null;
				label2.Text = "画像無し";
			}
		}

		private async void PostDisplayToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var postId = postDisplayToolStripTextBox.Text;
			var post = await _Misskey.Posts.Show(postId);
			MessageBox.Show(getDisplayPostText(post), $"Type: {post.Type.ToString()}");
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var name = Assembly.GetExecutingAssembly().GetName();
			MessageBox.Show($"v{name.Version.Major}.{name.Version.Minor}.{name.Version.Build}", $"{name.Name}のバージョン情報");
		}

		private void testToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var test = new TestClass();
		}
	}
}
