using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZamRecipeParser
{
	public partial class FormDatabase : Form {
		public FormDatabase() {
			InitializeComponent();
		}

		public string GetHost() {
			return textBox_host.Text;
		}

		public string GetUsername() {
			return textBox_username.Text;
		}

		public string GetPassword() {
			return textBox_password.Text;
		}

		public string GetDatabase() {
			return textBox_database.Text;
		}

		public uint GetPort() {
			return uint.Parse(textBox_port.Text);
		}
	}
}
