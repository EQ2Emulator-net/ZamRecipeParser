namespace ZamRecipeParser
{
	partial class FormDatabase {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.Label();
			this.textBox_host = new System.Windows.Forms.TextBox();
			this.textBox_username = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox_password = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textBox_database = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBox_port = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.button_cancel = new System.Windows.Forms.Button();
			this.button_ok = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(29, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Host";
			// 
			// textBox_host
			// 
			this.textBox_host.Location = new System.Drawing.Point(12, 25);
			this.textBox_host.Name = "textBox_host";
			this.textBox_host.Size = new System.Drawing.Size(226, 20);
			this.textBox_host.TabIndex = 1;
			this.textBox_host.Text = "127.0.0.1";
			// 
			// textBox_username
			// 
			this.textBox_username.Location = new System.Drawing.Point(12, 64);
			this.textBox_username.Name = "textBox_username";
			this.textBox_username.Size = new System.Drawing.Size(226, 20);
			this.textBox_username.TabIndex = 2;
			this.textBox_username.Text = "root";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Username";
			// 
			// textBox_password
			// 
			this.textBox_password.Location = new System.Drawing.Point(12, 103);
			this.textBox_password.Name = "textBox_password";
			this.textBox_password.PasswordChar = '*';
			this.textBox_password.Size = new System.Drawing.Size(226, 20);
			this.textBox_password.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 87);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Password";
			// 
			// textBox_database
			// 
			this.textBox_database.Location = new System.Drawing.Point(12, 142);
			this.textBox_database.Name = "textBox_database";
			this.textBox_database.Size = new System.Drawing.Size(226, 20);
			this.textBox_database.TabIndex = 4;
			this.textBox_database.Text = "eq2";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 126);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(53, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Database";
			// 
			// textBox_port
			// 
			this.textBox_port.Location = new System.Drawing.Point(12, 181);
			this.textBox_port.Name = "textBox_port";
			this.textBox_port.Size = new System.Drawing.Size(226, 20);
			this.textBox_port.TabIndex = 5;
			this.textBox_port.Text = "3306";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 165);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(26, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Port";
			// 
			// button_cancel
			// 
			this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button_cancel.Location = new System.Drawing.Point(163, 220);
			this.button_cancel.Name = "button_cancel";
			this.button_cancel.Size = new System.Drawing.Size(75, 23);
			this.button_cancel.TabIndex = 7;
			this.button_cancel.Text = "Cancel";
			this.button_cancel.UseVisualStyleBackColor = true;
			// 
			// button_ok
			// 
			this.button_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button_ok.Location = new System.Drawing.Point(82, 220);
			this.button_ok.Name = "button_ok";
			this.button_ok.Size = new System.Drawing.Size(75, 23);
			this.button_ok.TabIndex = 6;
			this.button_ok.Text = "OK";
			this.button_ok.UseVisualStyleBackColor = true;
			// 
			// FormDatabase
			// 
			this.AcceptButton = this.button_ok;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.button_cancel;
			this.ClientSize = new System.Drawing.Size(250, 255);
			this.Controls.Add(this.button_ok);
			this.Controls.Add(this.button_cancel);
			this.Controls.Add(this.textBox_port);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textBox_database);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBox_password);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textBox_username);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox_host);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormDatabase";
			this.Text = "Connect to Database";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox_host;
		private System.Windows.Forms.TextBox textBox_username;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox_password;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBox_database;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBox_port;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button button_cancel;
		private System.Windows.Forms.Button button_ok;
	}
}