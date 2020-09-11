using System;
using System.Globalization;

namespace AnimationExpression.Compiler
{
	public class Scanner
	{
		private readonly string Source;
		private int Index;
		public int LineNumber { get; private set; }

		public Scanner(string source)
		{
			Source = source;
			Rewind();
		}

		public void Rewind()
		{
			Index = 0;
			LineNumber = 0;
		}

		private char GetChar()
		{
			if (Index >= Source.Length)
			{
				return (char) 0;
			}
			return Source[Index];
		}

		private char NextChar()
		{
			if (GetChar() == '\n')
			{
				LineNumber++;
			}

			++Index;

			return GetChar();
		}

		private static bool IsSpace(char c)
		{
			return c == ' ' || c == '\t' || c == '\r' || c == '\n';
		}

		private static bool IsAlphabet(char c)
		{
			return (65 <= c && c <= 90) || (97 <= c && c <= 122);
		}

		private static bool IsNumber(char c)
		{
			return 48 <= c && c <= 57;
		}

		private static bool IsAlphabetOrNumber(char c)
		{
			return (48 <= c && c <= 57) || (65 <= c && c <= 90) || (97 <= c && c <= 122);
		}

		private static bool IsHex(char c)
		{
			return (48 <= c && c <= 57) || (65 <= c && c <= 70) || (97 <= c && c <= 102);
		}

		private static bool IsIdentifier(char c)
		{
			return c == 36 || c == 95 || (48 <= c && c <= 57) || (65 <= c && c <= 90) || (97 <= c && c <= 122);
		}

		public Token GetToken()
		{
			var c = GetChar();

			while (IsSpace(c))
			{
				c = NextChar();
			}

			if (c == 0)
			{
				return null;
			}

			if (IsAlphabet(c) || c == '$' || c == '_')
			{
				var val = c.ToString();
				while ((c = NextChar()) != 0 && IsIdentifier(c))
				{
					val += c;
				}

				switch (val)
				{
					case "if":
						return new Symbol('I');
					case "true":
						return new Number(1f);
					case "false":
						return new Number(0f);
					default:
						return new Identifier(val);
				}
			}

			if (IsNumber(c))
			{
				var value = c.ToString();
				if (c == '0')
				{
					if ((c = NextChar()) != 0 && c == 'x' || c == 'X')
					{
						value = "";
						while ((c = NextChar()) != 0 && IsHex(c))
						{
							value += c;
						}
						
						if (uint.TryParse(value, NumberStyles.HexNumber, null, out var i))
						{
							return new Number(i);
						}
						else
						{
							throw new ArgumentException($"Can't parse {value}");
						}
					}
					else if (IsNumber(c))
					{
						value += c;
						while ((c = NextChar()) != 0 && IsNumber(c))
						{
							value += c;
						}
					}
				}
				else
				{
					while ((c = NextChar()) != 0 && IsNumber(c))
					{
						value += c;
					}
				}

				if (c == '.')
				{
					value += c;
					while ((c = NextChar()) != 0 && IsNumber(c))
					{
						value += c;
					}
				}
					
				if (c == 'f')
				{
					NextChar();
				}
					
				if (float.TryParse(value, out var f))
				{
					return new Number(f);
				}
				else
				{
					throw new ArgumentException($"Can't parse {value}");
				}
			}

			if (c == '/')
			{
				if ((c = NextChar()) != 0)
				{
					if (c == '/')
					{
						while ((c = NextChar()) != 0 && c != '\n')
						{
						}

						NextChar();
						return GetToken();
					}

					if (c == '*')
					{
						for (c = NextChar(); c != 0;)
						{
							if (c == '*')
							{
								if ((c = NextChar()) != 0 && c == '/')
								{
									break;
								}

								continue;
							}

							c = NextChar();
						}

						NextChar();
						return GetToken();
					}

					return new Operator('/');
				}
			}

			if (c == '+' || c == '-' || c == '*')
			{
				NextChar();
				return new Operator(c);
			}

			/*
				&&
				||
			*/
			if (c == '|' || c == '&')
			{
				var type = c;
				if ((c = NextChar()) != 0 && (c == '|' || c == '&'))
				{
					NextChar();
				}

				return new Operator(type);
			}

			/*
				==
				!=
			*/
			if (c == '=')
			{
				var type = c;
				if ((c = NextChar()) != 0 && c == '=')
				{
					NextChar();
				}

				return new Operator(type);
			}
			if (c == '!')
			{
				var type = c;
				if ((c = NextChar()) != 0 && c == '=')
				{
					NextChar();
					return new Operator('N');
				}

				return new Operator(type);
			}

			/*
				>
				>=
				<
				<=
			*/
			if (c == '>')
			{
				if ((c = NextChar()) != 0 && c == '=')
				{
					NextChar();
					return new Operator('G');
				}

				return new Operator('>');
			}

			if (c == '<')
			{
				if ((c = NextChar()) != 0 && c == '=')
				{
					NextChar();
					return new Operator('L');
				}

				return new Operator('<');
			}

			switch (c)
			{
				case '(':
				case ')':
				case '{':
				case '}':
				case '.':
				case ';':
				case ',':
				case '?':
				case ':':
				{
					NextChar();
					return new Symbol(c);
				}
				default:
				{
					throw new ArgumentException($"Unknown character '{c}' at index {Index}.");
				}
			}
		}
	}
}