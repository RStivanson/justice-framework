using System;
using System.Collections;
using System.Linq;

namespace JusticeFramework.Utility.Extensions {
	public static class TypeExtensions {
		public static byte[] GetBytes(this bool data) {
			return BitConverter.GetBytes(data);
		}

		public static byte[] GetBytes(this char data) {
			byte[] bytes = BitConverter.GetBytes(data);

			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		public static byte[] GetBytes(this short data) {
			byte[] bytes = BitConverter.GetBytes(data);

			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		public static byte[] GetBytes(this ushort data) {
			byte[] bytes = BitConverter.GetBytes(data);

			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		public static byte[] GetBytes(this int data) {
			byte[] bytes = BitConverter.GetBytes(data);

			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		public static byte[] GetBytes(this uint data) {
			byte[] bytes = BitConverter.GetBytes(data);

			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		public static byte[] GetBytes(this float data) {
			byte[] bytes = BitConverter.GetBytes(data);

			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		public static byte[] GetBytes(this double data) {
			byte[] bytes = BitConverter.GetBytes(data);

			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		public static byte[] GetBytes(this long data) {
			byte[] bytes = BitConverter.GetBytes(data);

			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		public static byte[] GetBytes(this ulong data) {
			byte[] bytes = BitConverter.GetBytes(data);

			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		public static byte[] GetBytes(this string data) {
			ArrayList byteList = new ArrayList();

			for (int i = 0; i < data.Length; ++i) {
				byteList.AddRange(data[i].GetBytes());
			}

			return (byte[])byteList.ToArray(typeof(byte));
		}

		public static byte[] Combine(params byte[][] arrays) {
			byte[] rv = new byte[arrays.Sum(a => a.Length)];
			int offset = 0;

			foreach (byte[] array in arrays) {
				Buffer.BlockCopy(array, 0, rv, offset, array.Length);
				offset += array.Length;
			}

			return rv;
		}
	}
}
