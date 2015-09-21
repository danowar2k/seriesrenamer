using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renamer.NewClasses.Util {
	/// <summary>
	/// Utility class for converting basic types to another.
	/// </summary>
	public class ConverterUtils {
		/// <summary>
		/// Converts a string to a bool, by compare it to some string values
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool StringToBool(string str) {
			try {
				return Convert.ToInt32(str) > 0 || str == System.Boolean.TrueString;
			} catch {
				return false;
			}
		}

		/// <summary>
		/// Checks if the given string can be interpreted as a number.
		/// </summary>
		/// <param name="str">string to check</param>
		/// <returns>true if string can be interpreted as a number</returns>
		public static bool IsNumeric(string str) {
			double x;
			return double.TryParse(str, out x);
		}

	}
}
