using System;
using System.IO;

namespace JusticeFramework.Utility.Extensions {
	/// <summary>
	/// A collection of extension methods that extend the functionality of streams
	/// </summary>
	public static class StreamExtensions {
		/// <summary>
		/// Reads a strings byte values from the stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <param name="length">The length of characters to read from the stream</param>
		/// <returns>Returns a string read from the file stream</returns>
		public static string ReadString(this Stream stream, int length) {
			return BitConverter.ToString(Read(stream, sizeof(char) * length));
		}

		/// <summary>
		/// Reads a strings byte values from the stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <param name="result">The resulting string read from the stream</param>
		/// <param name="length">The length of characters to read from the stream</param>
		public static void ReadString(this Stream stream, out string result, int length) {
			result = ReadString(stream, length);
		}

		/// <summary>
		/// Reads a series of string byte values from the stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <param name="amount">The amount of strings to read from the stream</param>
		/// <returns>The resulting string array read from the stream</returns>
		public static string[] ReadStringArray(this Stream stream, int amount) {
			string[] strings = new string[amount];
			
			// For each string, attempt to read it from the stream
			for (int i = 0; i < amount; ++i) {
				strings[i] = stream.ReadString(stream.ReadInt());
			}

			return strings;
		}
		
		/// <summary>
		/// Writes a strings byte value to the stream
		/// </summary>
		/// <param name="stream">The stream to write to</param>
		/// <param name="data">The data that should be written to the stream</param>
		public static void WriteString(this Stream stream, string data) {
			stream.Write(data.GetBytes(), 0, sizeof(char) * data.Length);
		}

		/// <summary>
		/// Writes the amount of strings in the array then the string byte values to the stream
		/// </summary>
		/// <param name="stream">The stream to write to</param>
		/// <param name="data">The data that should be written to the stream</param>
		public static void WriteStringArray(this Stream stream, string[] data) {
			// Writes the amount of strings in the array to the stream
			stream.WriteInt(data.Length);

			// For each string in the array, write its length then its content to the stream
			foreach (string toWrite in data) {
				stream.WriteInt(toWrite.Length);
				stream.WriteString(toWrite);
			}
		}
		
		/// <summary>
		/// Reads a boolean byte value from the stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <returns>Returns the value of boolean read from the stream</returns>
		public static bool ReadBool(this Stream stream) {
			return BitConverter.ToBoolean(Read(stream, sizeof(bool)), 0);
		}

		/// <summary>
		/// Writes a boolean value to the stream
		/// </summary>
		/// <param name="stream">The stream to write to</param>
		/// <param name="data">The data that should be written to the stream</param>
		public static void WriteBool(this Stream stream, bool data) {
			stream.Write(data.GetBytes(), 0, sizeof(bool));
		}

		/// <summary>
		/// Reads a character byte value from the stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <returns>Returns the value of the character read from the stream</returns>
		public static char ReadChar(this Stream stream) {
			return BitConverter.ToChar(Read(stream, sizeof(char)), 0);
		}

		/// <summary>
		/// Writes a charcter byte value to the stream
		/// </summary>
		/// <param name="stream">The stream to write to</param>
		/// <param name="data">The data that should be written to the stream</param>
		public static void WriteChar(this Stream stream, char data) {
			stream.Write(data.GetBytes(), 0, sizeof(char));
		}

		/// <summary>
		/// Reads a short byte value from the stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <returns>Returns the value of the short read from the stream</returns>
		public static short ReadShort(this Stream stream) {
			return BitConverter.ToInt16(Read(stream, sizeof(short)), 0);
		}

		/// <summary>
		/// Writes a short byte value to the stream
		/// </summary>
		/// <param name="stream">The stream to write to</param>
		/// <param name="data">The data that should be written to the stream</param>
		public static void WriteShort(this Stream stream, short data) {
			stream.Write(data.GetBytes(), 0, sizeof(short));
		}

		/// <summary>
		/// Reads an unsigned short byte value from the stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <returns>Returns the value of the unsigned short read from the stream</returns>
		public static ushort ReadUShort(this Stream stream) {
			return BitConverter.ToUInt16(Read(stream, sizeof(ushort)), 0);
		}

		/// <summary>
		/// Writes an unsigned short byte value to the stream
		/// </summary>
		/// <param name="stream">The stream to write to</param>
		/// <param name="data">The data that should be written to the stream</param>
		public static void WriteUShort(this Stream stream, ushort data) {
			stream.Write(data.GetBytes(), 0, sizeof(ushort));
		}

		/// <summary>
		/// Reads an integers byte values from the stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <returns>Returns the value of integer read from the stream</returns>
		public static int ReadInt(this Stream stream) {
			return BitConverter.ToInt32(Read(stream, sizeof(int)), 0);
		}

		/// <summary>
		/// Writes an integers byte values to the stream
		/// </summary>
		/// <param name="stream">The stream to write to</param>
		/// <param name="data">The data that should be written to the stream</param>
		public static void WriteInt(this Stream stream, int data) {
			stream.Write(data.GetBytes(), 0, sizeof(int));
		}

		/// <summary>
		/// Reads an usigned integers byte values from the stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <returns>Returns the value of the unsigned integer read from the stream</returns>
		public static uint ReadUInt(this Stream stream) {
			return BitConverter.ToUInt32(Read(stream, sizeof(uint)), 0);
		}

		/// <summary>
		/// Writes an unsigned integers byte values to the stream
		/// </summary>
		/// <param name="stream">The stream to write to</param>
		/// <param name="data">The data that should be written to the stream</param>
		public static void WriteUInt(this Stream stream, uint data) {
			stream.Write(data.GetBytes(), 0, sizeof(uint));
		}

		/// <summary>
		/// Reads a floats byte values from the stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <returns>Returns the value of the float read from the stream</returns>
		public static float ReadFloat(this Stream stream) {
			return BitConverter.ToSingle(Read(stream, sizeof(float)), 0);
		}

		/// <summary>
		/// Writes a floats byte values to the strea,
		/// </summary>
		/// <param name="stream">The stream to write to</param>
		/// <param name="data">The data that should be written to the stream</param>
		public static void WriteFloat(this Stream stream, float data) {
			stream.Write(data.GetBytes(), 0, sizeof(float));
		}

		/// <summary>
		/// Reads a doubles byte values from the stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <returns>Returns the value of the double read from the stream</returns>
		public static double ReadDouble(this Stream stream) {
			return BitConverter.ToDouble(Read(stream, sizeof(double)), 0);
		}

		/// <summary>
		/// Writes a doubles byte values to the stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <param name="data">The data that should be written to the stream</param>
		public static void WriteDouble(this Stream stream, double data) {
			stream.Write(data.GetBytes(), 0, sizeof(double));
		}

		/// <summary>
		/// Reads a longs byte values from the stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <returns>Returns the value of the long read from the stream</returns>
		public static long ReadLong(this Stream stream) {
			return BitConverter.ToInt64(Read(stream, sizeof(long)), 0);
		}

		/// <summary>
		/// Writes a longs byte values to the stream
		/// </summary>
		/// <param name="stream">The stream to write to</param>
		/// <param name="data">The data that should be written to the stream</param>
		public static void WriteLong(this Stream stream, long data) {
			stream.Write(data.GetBytes(), 0, sizeof(long));
		}

		/// <summary>
		/// Reads an unsigned longs byte values from the stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <returns>Returns the value of the unsigned long read from the stream</returns>
		public static ulong ReadULong(this Stream stream) {
			return BitConverter.ToUInt64(Read(stream, sizeof(ulong)), 0);
		}

		/// <summary>
		/// Writes an unsigned longs byte values to the stream
		/// </summary>
		/// <param name="stream">The stream to write to</param>
		/// <param name="data">The data that should be written to the stream</param>
		public static void WriteULong(this Stream stream, ulong data) {
			stream.Write(data.GetBytes(), 0, sizeof(ulong));
		}

		/// <summary>
		/// Reads the amount of specified bytes from the stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <param name="length">The length of bytes to read from the stream</param>
		/// <returns>An array of bytes read from the stream of the given length</returns>
		private static byte[] Read(Stream stream, int length) {
			byte[] byteBuffer = new byte[length];
			
			// Read the data
			stream.Read(byteBuffer, 0, length);

			// Account for little endian machines
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
