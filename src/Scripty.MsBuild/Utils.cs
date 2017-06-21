using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripty.MsBuild
{
	class Utils
	{
		private const char QUOTE_CHAR = '"';

		static void SkipBlanks(ref int index, string csvString)
		{
			while (index < csvString.Length &&
				csvString[index] == ' ')
				index++;
		}

		public static List<string> AsList(string csvString, IEnumerable<char> separators = null, char escapeChar = '\\', bool skipBlanks = true)
		{
			HashSet<char> seps;
			if (separators == null)
			{
				seps = new HashSet<char>();
				seps.Add(';');
			}
			else
				seps = new HashSet<char>(separators);

			bool quoteCharIsSame = escapeChar == QUOTE_CHAR;

				var list = new List<string>();
				bool markOn = false;
				csvString = skipBlanks ? csvString.Trim() : csvString;
				if (csvString.Length == 0)
					return list;
				int index = 0;
				StringBuilder s = new StringBuilder();
				bool ending = false;

				if (skipBlanks) SkipBlanks(ref index, csvString);

				bool inEscape = false;

				while (index < csvString.Length && !ending)
				{
					if (!inEscape && csvString[index] == QUOTE_CHAR)
					{
						// start quoted item
						if (!markOn && s.Length == 0)
							markOn = true;
						else
							// last quote
							if (index == csvString.Length - 1)
						{
							list.Add(s.ToString());
							s.Length = 0;
							markOn = false;
							ending = true;
						}
						else
						{   // part of item ?
							if (markOn)
							{
								// next quote mark for literal char
								if (quoteCharIsSame && csvString[index + 1] == '"')
								{
									s.Append('"');
									index++;
								}
								else
									// end quoted item
									markOn = false;
							}
							// not marked, is literal char
							else
								s.Append(QUOTE_CHAR);
						}
					}
					else if (!markOn && csvString[index] == '\r')
					{
						// ignore cr
					}
					else if (!markOn && csvString[index] == '\n')
					{
						ending = true; break; // end of line
					}
					else
					{
						// check separator
						if (seps.Contains(csvString[index]))
						{
							// delimiter is part of string
							if (markOn)
								s.Append(csvString[index]);
							else
							{
								list.Add(s.ToString());
								s.Length = 0;
								markOn = false;
								index++; // after ;
								if (skipBlanks) SkipBlanks(ref index, csvString);
								continue; // don't jump to increment index
							}
						}
						else if (markOn && !inEscape && csvString[index] == escapeChar)
						{
							inEscape = true;
						}
						else
						{
							inEscape = false;
							s.Append(csvString[index]);
						}
					}
					index++;
				}

				// if line ends without Delimiter
				if (s.Length > 0)
					list.Add(s.ToString());
				return list;
			}
	}
}
