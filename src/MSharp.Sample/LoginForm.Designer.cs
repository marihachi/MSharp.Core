namespace MSharp.Sample
{
	partial class LoginForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.passwordBox = new System.Windows.Forms.TextBox();
			this.screenNameBox = new System.Windows.Forms.TextBox();
			this.loginButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(20, 73);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(39, 18);
			this.label2.TabIndex = 11;
			this.label2.Text = "Pass:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(32, 40);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(27, 18);
			this.label1.TabIndex = 10;
			this.label1.Text = "ID:";
			// 
			// passwordBox
			// 
			this.passwordBox.BackColor = System.Drawing.SystemColors.Control;
			this.passwordBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.passwordBox.Location = new System.Drawing.Point(67, 71);
			this.passwordBox.Margin = new System.Windows.Forms.Padding(4);
			this.passwordBox.Name = "passwordBox";
			this.passwordBox.PasswordChar = '*';
			this.passwordBox.Size = new System.Drawing.Size(138, 25);
			this.passwordBox.TabIndex = 9;
			// 
			// screenNameBox
			// 
			this.screenNameBox.BackColor = System.Drawing.SystemColors.Control;
			this.screenNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.screenNameBox.Location = new System.Drawing.Point(67, 38);
			this.screenNameBox.Margin = new System.Windows.Forms.Padding(4);
			this.screenNameBox.MaxLength = 30;
			this.screenNameBox.Name = "screenNameBox";
			this.screenNameBox.Size = new System.Drawing.Size(138, 25);
			this.screenNameBox.TabIndex = 8;
			// 
			// loginButton
			// 
			this.loginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.loginButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.loginButton.Location = new System.Drawing.Point(144, 104);
			this.loginButton.Margin = new System.Windows.Forms.Padding(4);
			this.loginButton.Name = "loginButton";
			this.loginButton.Size = new System.Drawing.Size(61, 31);
			this.loginButton.TabIndex = 7;
			this.loginButton.Text = "Login";
			this.loginButton.UseVisualStyleBackColor = true;
			this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
			// 
			// LoginForm
			// 
			this.AcceptButton = this.loginButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(224, 172);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.passwordBox);
			this.Controls.Add(this.screenNameBox);
			this.Controls.Add(this.loginButton);
			this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoginForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Misskeyにログイン";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox passwordBox;
		private System.Windows.Forms.TextBox screenNameBox;
		private System.Windows.Forms.Button loginButton;
	}
}