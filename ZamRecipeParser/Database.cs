using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace ZamRecipeParser
{
	public class Database {
		private MySqlConnection conn;
		public Database() {
			conn = null;
		}

		public bool Connect(string host, string user, string password, string database, uint port) {
			try {
				conn = new MySqlConnection("Server=" + host + "; UserId=" + user + "; Password=" + password + "; Database=" + database + "; Port=" + port.ToString() + ";");
				conn.Open();
			}
			catch (Exception ex) {
				System.Windows.Forms.MessageBox.Show(ex.Message, "Error Connecting to Database", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return false;
			}
			return true;
		}

		public uint Query(string query, params object[] args) {
			uint insert_id = 0;
			query = String.Format(query, args);
			MySqlCommand command = conn.CreateCommand();
			command.CommandText = query;
			command.ExecuteNonQuery();

			command = conn.CreateCommand();
			command.CommandText = "select last_insert_id()";
			MySqlDataReader reader = command.ExecuteReader();
			if (reader.Read()) {
				insert_id = reader.GetUInt32(0);
				reader.Close();
			}

			return insert_id;
		}

        public uint SingleSelect(string query, params object[] args)
        {
            uint ret = 0;
            query = string.Format(query, args);
            MySqlCommand command = conn.CreateCommand();
            command.CommandText = query;
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                ret = reader.GetUInt32(0);
            }

            if (!reader.IsClosed)
                reader.Close();

            return ret;
        }

        public uint GetMaxId(string table, string field)
        {
            uint id = 0;
            MySqlCommand command = conn.CreateCommand();
            command.CommandText = string.Format("SELECT MAX(`{0}`) FROM {1}", field, table);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                if (!reader.IsDBNull(0))
                    id = reader.GetUInt32(0);
                reader.Close();
            }

            return id;
        }

		public static string Escape(string str) {
            if (str == null)
                str = "";
			str = str.Replace("'", "\\'");
			str = str.Replace("%", "\\%");
			str = str.Replace("_", "\\_");
            str = str.Replace("\"", "\\\"");
			return str;
		}

		public void Close() {
			if (conn != null)
				conn.Close();
		}
	}
}
