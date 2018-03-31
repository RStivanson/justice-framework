using System;
using System.IO;

namespace JusticeFramework.Utility.Extensions {
	public static class FileStreamExtensions {
		public static string ReadString(this FileStream stream, int length) {
			string data = "";

			for (int i = 0; i < length; ++i) {
				data += stream.ReadChar();
			}

			return data;
		}

		public static void ReadString(this FileStream stream, out string result, int length) {
			result = "";

			for (int i = 0; i < length; ++i) {
				result += stream.ReadChar();
			}
		}

		public static string[] ReadStringArray(this FileStream stream, int amount) {
			string[] strings = new string[amount];

			for (int i = 0; i < amount; ++i) {
				strings[i] = stream.ReadString(stream.ReadInt());
			}

			return strings;
		}
		
		public static void WriteString(this FileStream stream, string data) {
			for (int i = 0; i < data.Length; ++i) {
				stream.WriteChar(data[i]);
			}
		}

		public static void WriteStringArray(this FileStream stream, string[] data) {
			stream.WriteInt(data.Length);
			
			for (int i = 0; i < data.Length; ++i) {
				stream.WriteInt(data[i].Length);
				stream.WriteString(data[i]);
			}
		}
		
		// ---------------------------------------- //

		public static bool ReadBool(this FileStream stream) {
			return BitConverter.ToBoolean(Read(stream, sizeof(bool)), 0);
		}

		public static void WriteBool(this FileStream stream, bool data) {
			stream.Write(data.GetBytes(), 0, sizeof(bool));
		}

		// ---------------------------------------- //

		public static char ReadChar(this FileStream stream) {
			return BitConverter.ToChar(Read(stream, sizeof(char)), 0);
		}

		public static void WriteChar(this FileStream stream, char data) {
			stream.Write(data.GetBytes(), 0, sizeof(char));
		}

		// ---------------------------------------- //

		public static short ReadShort(this FileStream stream) {
			return BitConverter.ToInt16(Read(stream, sizeof(short)), 0);
		}

		public static void WriteShort(this FileStream stream, short data) {
			stream.Write(data.GetBytes(), 0, sizeof(short));
		}

		public static ushort ReadUShort(this FileStream stream) {
			return BitConverter.ToUInt16(Read(stream, sizeof(ushort)), 0);
		}

		public static void WriteUShort(this FileStream stream, ushort data) {
			stream.Write(data.GetBytes(), 0, sizeof(ushort));
		}

		// ---------------------------------------- //

		public static int ReadInt(this FileStream stream) {
			return BitConverter.ToInt32(Read(stream, sizeof(int)), 0);
		}

		public static void WriteInt(this FileStream stream, int data) {
			stream.Write(data.GetBytes(), 0, sizeof(int));
		}

		public static uint ReadUInt(this FileStream stream) {
			return BitConverter.ToUInt32(Read(stream, sizeof(uint)), 0);
		}

		public static void WriteUInt(this FileStream stream, uint data) {
			stream.Write(data.GetBytes(), 0, sizeof(uint));
		}

		// ---------------------------------------- //

		public static float ReadFloat(this FileStream stream) {
			return BitConverter.ToSingle(Read(stream, sizeof(float)), 0);
		}

		public static void WriteFloat(this FileStream stream, float data) {
			stream.Write(data.GetBytes(), 0, sizeof(float));
		}

		// ---------------------------------------- //

		public static double ReadDouble(this FileStream stream) {
			return BitConverter.ToDouble(Read(stream, sizeof(double)), 0);
		}

		public static void WriteDouble(this FileStream stream, double data) {
			stream.Write(data.GetBytes(), 0, sizeof(double));
		}

		// ---------------------------------------- //

		public static long ReadLong(this FileStream stream) {
			return BitConverter.ToInt64(Read(stream, sizeof(long)), 0);
		}

		public static void WriteLong(this FileStream stream, long data) {
			stream.Write(data.GetBytes(), 0, sizeof(long));
		}

		public static ulong ReadULong(this FileStream stream) {
			return BitConverter.ToUInt64(Read(stream, sizeof(ulong)), 0);
		}

		public static void WriteULong(this FileStream stream, ulong data) {
			stream.Write(data.GetBytes(), 0, sizeof(ulong));
		}

		// ---------------------------------------- //

		private static byte[] Read(FileStream stream, int length) {
			byte[] byteBuffer = new byte[length];
			stream.Read(byteBuffer, 0, length);

			if (BitConverter.IsLittleEndian) {
				Array.Reverse(byteBuffer);
			}

			return byteBuffer;
		}

		/*public static void InsertBytes(this FileStream stream, int offset, byte[] data) {
			using (FileStream tmpStream = new FileStream(Path.Combine(UnityEngine.Application.persistentDataPath, Path.GetTempFileName()), FileMode.Create)) {
				stream.Position = offset;

				while (stream.Position != stream.Length) {
					tmpStream.WriteByte((byte)stream.ReadByte());
				}

				stream.Position = offset;
				stream.Write(data, 0, data.Length);

				tmpStream.Position = 0;
				while (tmpStream.Position != tmpStream.Length) {
					stream.WriteByte((byte)tmpStream.ReadByte());
				}
			}
		}*/
	}
}
