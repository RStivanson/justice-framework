using System;
using System.Collections;
using System.Linq;

namespace JusticeFramework.Core.Extensions {
	/// <summary>
	/// A collection of extension methods that extend the functionality of primitive types
	/// </summary>
	public static class PrimitiveExtensions {
		/// <summary>
		/// Converts a boolean value into a byte array representation
		/// </summary>
		/// <param name="data">The data to convert to bytes</param>
		/// <returns>An array of bytes for the provided data</returns>
		public static byte[] GetBytes(this bool data) {
			return BitConverter.GetBytes(data);
		}

		/// <summary>
		/// Converts a character value into a byte array representation
		/// </summary>
		/// <param name="data">The data to convert to bytes</param>
		/// <returns>An array of bytes for the provided data</returns>
		public static byte[] GetBytes(this char data) {
			byte[] bytes = BitConverter.GetBytes(data);

			// Account for cross system endian differences
			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		/// <summary>
		/// Converts a short value into a byte array representation
		/// </summary>
		/// <param name="data">The data to convert to bytes</param>
		/// <returns>An array of bytes for the provided data</returns>
		public static byte[] GetBytes(this short data) {
			byte[] bytes = BitConverter.GetBytes(data);

			// Account for cross system endian differences
			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		/// <summary>
		/// Converts a unsigned short value into a byte array representation
		/// </summary>
		/// <param name="data">The data to convert to bytes</param>
		/// <returns>An array of bytes for the provided data</returns>
		public static byte[] GetBytes(this ushort data) {
			byte[] bytes = BitConverter.GetBytes(data);

			// Account for cross system endian differences
			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		/// <summary>
		/// Converts a integer value into a byte array representation
		/// </summary>
		/// <param name="data">The data to convert to bytes</param>
		/// <returns>An array of bytes for the provided data</returns>
		public static byte[] GetBytes(this int data) {
			byte[] bytes = BitConverter.GetBytes(data);

			// Account for cross system endian differences
			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		/// <summary>
		/// Converts a unsigned integer value into a byte array representation
		/// </summary>
		/// <param name="data">The data to convert to bytes</param>
		/// <returns>An array of bytes for the provided data</returns>
		public static byte[] GetBytes(this uint data) {
			byte[] bytes = BitConverter.GetBytes(data);

			// Account for cross system endian differences
			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		/// <summary>
		/// Converts a float value into a byte array representation
		/// </summary>
		/// <param name="data">The data to convert to bytes</param>
		/// <returns>An array of bytes for the provided data</returns>
		public static byte[] GetBytes(this float data) {
			byte[] bytes = BitConverter.GetBytes(data);

			// Account for cross system endian differences
			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		/// <summary>
		/// Converts a double value into a byte array representation
		/// </summary>
		/// <param name="data">The data to convert to bytes</param>
		/// <returns>An array of bytes for the provided data</returns>
		public static byte[] GetBytes(this double data) {
			byte[] bytes = BitConverter.GetBytes(data);

			// Account for cross system endian differences
			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		/// <summary>
		/// Converts a long value into a byte array representation
		/// </summary>
		/// <param name="data">The data to convert to bytes</param>
		/// <returns>An array of bytes for the provided data</returns>
		public static byte[] GetBytes(this long data) {
			byte[] bytes = BitConverter.GetBytes(data);

			// Account for cross system endian differences
			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		/// <summary>
		/// Converts a unsigned long value into a byte array representation
		/// </summary>
		/// <param name="data">The data to convert to bytes</param>
		/// <returns>An array of bytes for the provided data</returns>
		public static byte[] GetBytes(this ulong data) {
			byte[] bytes = BitConverter.GetBytes(data);

			// Account for cross system endian differences
			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}

			return bytes;
		}

		/// <summary>
		/// Converts a string value into a byte array representation
		/// </summary>
		/// <param name="data">The data to convert to bytes</param>
		/// <returns>An array of bytes for the provided data</returns>
		public static byte[] GetBytes(this string data) {
			ArrayList byteList = new ArrayList();

			// Add each character to the list of bytes
			for (int i = 0; i < data.Length; ++i) {
				byteList.AddRange(data[i].GetBytes());
			}

			return (byte[])byteList.ToArray(typeof(byte));
		}

        /// <summary>
        /// Determines if this string is empty or is all whitespace.
        /// </summary>
        /// <param name="data">The string to check.</param>
        /// <returns>Returns true if the string is empty or all whitespace, false otherwise.</returns>
        public static bool IsNullOrWhiteSpace(this string data) {
            return string.IsNullOrWhiteSpace(data);
        }

        /// <summary>
        /// Determines if the string matches other other using an ordinal comparison.
        /// </summary>
        /// <param name="data">The first string.</param>
        /// <param name="other">The second string.</param>
        /// <returns>Returns true if the strings match, false otherwise.</returns>
        public static bool EqualsOrdinal(this string data, string other) {
            return data.Equals(other, StringComparison.Ordinal);
        }

        /// <summary>
        /// Determines if the string matches other other using an ordinal comparison ignoring case.
        /// </summary>
        /// <param name="data">The first string.</param>
        /// <param name="other">The second string.</param>
        /// <returns>Returns true if the strings match, false otherwise.</returns>
        public static bool EqualsOrdinalIgnoreCase(this string data, string other) {
            return data.Equals(other, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Combines a series of byte arrays into a single array
        /// </summary>
        /// <param name="arrays">A list of byte arrays</param>
        /// <returns>A single array of bytes made up of all provided bytes arrays</returns>
        public static byte[] Combine(params byte[][] arrays) {
			// Get the size for the new array and allocate memory
			byte[] combinedArray = new byte[arrays.Sum(a => a.Length)];
			int offset = 0;

			// For each byte in array
			foreach (byte[] array in arrays) {
				// Copy the memory and move the offset forward in the array
				Buffer.BlockCopy(array, 0, combinedArray, offset, array.Length);
				offset += array.Length;
			}

			// Return the combined array
			return combinedArray;
		}
	}
}
