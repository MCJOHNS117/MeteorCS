using System;
using System.Collections.Generic;
using System.IO;

namespace MeteorEngine.Utils
{
	internal class DataFile
	{
		private List<string> mContent;
		private List<KeyValuePair<string, DataFile>> mvObjects;
		private Dictionary<string, int> mObjects;

		protected bool mIsComment;

		public DataFile()
		{
			mContent = new List<string>();
			mvObjects = new List<KeyValuePair<string, DataFile>>();
			mObjects = new Dictionary<string, int>();
			mIsComment = false;
		}

		public DataFile this[string name]
		{
			get
			{
				//First check if the objects map contains the key
				if(!mObjects.ContainsKey(name))
				{
					//if not create it
					mObjects.Add(name, mvObjects.Count);
					mvObjects.Add(new KeyValuePair<string, DataFile>(name, new DataFile()));
				}

				//return the object from the map
				return mvObjects[mObjects[name]].Value;
			}
		}

		public int GetValueCount()
		{
			return mContent.Count;
		}

		public void SetString(string s, int nItem = 0)
		{
			if (nItem >= mContent.Count)
				mContent.Add(s);
			else
				mContent[nItem] = s;
		}

		public string GetString(int nItem = 0)
		{
			if (nItem > mContent.Count)
				return "";
			else
				return mContent[nItem];
		}

		public void SetSByte(sbyte b, int nItem = 0)
		{
			if (nItem >= mContent.Count)
				mContent.Add(b.ToString());
			else
				mContent[nItem] = b.ToString();
		}

		public sbyte GetSByte(int nItem = 0)
		{
			if (nItem > mContent.Count)
				return sbyte.MinValue;
			else
				return sbyte.Parse(mContent[nItem]);
		}

		public void SetByte(byte b, int nItem = 0)
		{
			if (nItem >= mContent.Count)
				mContent.Add(b.ToString());
			else
				mContent[nItem] = b.ToString();
		}

		public byte GetByte(int nItem = 0)
		{
			if (nItem > mContent.Count)
				return byte.MinValue;
			else
				return byte.Parse(mContent[nItem]);
		}

		public void SetChar(char c, int nItem = 0)
		{
			if (nItem >= mContent.Count)
				mContent.Add(c.ToString());
			else
				mContent[nItem] = c.ToString();
		}

		public char GetChar(int nItem = 0)
		{
			if (nItem > mContent.Count)
				return char.MinValue;
			else
				return char.Parse(mContent[nItem]);
		}

		public void SetShort(short s, int nItem = 0)
		{
			if (nItem >= mContent.Count)
				mContent.Add(s.ToString());
			else
				mContent[nItem] = s.ToString();
		}

		public short GetShort(int nItem = 0)
		{
			if (nItem > mContent.Count)
				return short.MinValue;
			else
				return short.Parse(mContent[nItem]);
		}

		public void SetUShort(ushort s, int nItem = 0)
		{
			if (nItem >= mContent.Count)
				mContent.Add(s.ToString());
			else
				mContent[nItem] = s.ToString();
		}

		public ushort GetUShort(int nItem = 0)
		{
			if (nItem > mContent.Count)
				return ushort.MinValue;
			else
				return ushort.Parse(mContent[nItem]);
		}

		public void SetUInt(uint i, int nItem = 0)
		{
			if (nItem >= mContent.Count)
				mContent.Add(i.ToString());
			else
				mContent[nItem] = i.ToString();
		}

		public uint GetUInt(int nItem = 0)
		{
			if (nItem > mContent.Count)
				return uint.MinValue;
			else
				return uint.Parse(mContent[nItem]);
		}

		public void SetInt(int i, int nItem = 0)
		{
			if (nItem >= mContent.Count)
				mContent.Add(i.ToString());
			else
				mContent[nItem] = i.ToString();
		}

		public int GetInt(int nItem = 0)
		{
			if (nItem > mContent.Count)
				return int.MinValue;
			else
				return int.Parse(mContent[nItem]);
		}

		public void SetLong(long l, int nItem = 0)
		{
			if (nItem >= mContent.Count)
				mContent.Add(l.ToString());
			else
				mContent[nItem] = l.ToString();
		}

		public long GetLong(int nItem = 0)
		{
			if (nItem > mContent.Count)
				return long.MinValue;
			else
				return long.Parse(mContent[nItem]);
		}

		public void SetULong(ulong l, int nItem = 0)
		{
			if (nItem >= mContent.Count)
				mContent.Add(l.ToString());
			else
				mContent[nItem] = l.ToString();
		}

		public ulong GetULong(int nItem = 0)
		{
			if (nItem > mContent.Count)
				return ulong.MinValue;
			else
				return ulong.Parse(mContent[nItem]);
		}

		public void SetFloat(float f, int nItem = 0)
		{
			if (nItem >= mContent.Count)
				mContent.Add(f.ToString());
			else
				mContent[nItem] = f.ToString();
		}

		public float GetFloat(int nItem = 0)
		{
			if (nItem > mContent.Count)
				return float.MinValue;
			else
				return float.Parse(mContent[nItem]);
		}

		public void SetDouble(double d, int nItem = 0)
		{
			if (nItem >= mContent.Count)
				mContent.Add(d.ToString());
			else
				mContent[nItem] = d.ToString();
		}

		public double GetDouble(int nItem = 0)
		{
			if (nItem > mContent.Count)
				return double.MinValue;
			else
				return double.Parse(mContent[nItem]);
		}

		public void SetDecimal(decimal d, int nItem = 0)
		{
			if (nItem >= mContent.Count)
				mContent.Add(d.ToString());
			else
				mContent[nItem] = d.ToString();
		}

		public decimal GetDecimal(int nItem = 0)
		{
			if (nItem > mContent.Count)
				return decimal.MinValue;
			else
				return decimal.Parse(mContent[nItem]);
		}

		public void SetBool(bool b, int nItem = 0)
		{
			if (nItem >= mContent.Count)
				mContent.Add(b.ToString());
			else
				mContent[nItem] = b.ToString();
		}

		public bool GetBool(int nItem = 0)
		{
			if (nItem > mContent.Count)
				return false;
			else
				return bool.Parse(mContent[nItem]);
		}

		public static bool Write(DataFile df, string filename, string sIndent = "\t", char sListSep = ',')
		{
			string sSeperator = new string(sListSep, 1) + " ";

			int nIndentCount = 0;

			Func<string, int, string> indent = (s, c) =>
			{
				string result = "";
				for (int i = 0; i < c; i++)
					result += s;
				return result;
			};

			Func<DataFile, StreamWriter, bool> write = null;
			write = (df, writer) =>
			{
				foreach(var property in df.mvObjects)
				{
					if(property.Value.mvObjects.Count == 0)
					{
						writer.Write(indent(sIndent, nIndentCount) + property.Key + (property.Value.mIsComment ? "" : " = "));

						int nItems = property.Value.GetValueCount();
						for(int i = 0; i < property.Value.GetValueCount(); i++)
						{
							int x = property.Value.GetString(i).IndexOf(sListSep);
							if(x != -1)
							{
								writer.Write(property.Value.GetString(i) + "\"" + ((nItems > 1) ? sSeperator : ""));
							}
							else
							{
								writer.Write(property.Value.GetString(i) + ((nItems > 1) ? sSeperator : ""));
							}
							nItems--;
						}
						writer.Write("\n");
					}
					else
					{
						writer.Write(indent(sIndent, nIndentCount) + property.Key + "\n");
						writer.Write(indent(sIndent, nIndentCount) + "{\n");
						nIndentCount++;

						write(property.Value, writer);

						writer.Write(indent(sIndent, nIndentCount) + "}\n");
					}
				}

				if (nIndentCount > 0) nIndentCount--;

				return true;
			};

			StreamWriter writer = new StreamWriter(File.Open(filename, FileMode.Create));

			write(df, writer);

			writer.Close();

			return true;
		}

		public static DataFile Read(string filename, char sListSep = ',')
		{
			using (StreamReader reader = new StreamReader(File.Open(filename, FileMode.Open)))
			{
				string sPropName = "";
				string sPropVal = "";

				Stack<DataFile> stkPath = new Stack<DataFile>();
				DataFile df = new DataFile();
				stkPath.Push(df);

				Func<string, string> trim = (s) =>
				{
					s = s.Replace(" ", "");
					s = s.Replace("\t", "");
					s = s.Replace("\n", "");
					s = s.Replace("\r", "");
					s = s.Replace("\f", "");
					s = s.Replace("\v", "");
					return s;
				};

				string line;
				bool nextLine = true;

				while(true)
				{
					line = reader.ReadLine();
					//line = trim(line);

					if (line == null) break;

					if(!string.IsNullOrEmpty(line))
					{
						if (line[0] == '#')
						{
							DataFile comment = new DataFile();
							comment.mIsComment = true;
							stkPath.Peek().mvObjects.Add(new KeyValuePair<string, DataFile>(line, comment));
						}
						else
						{
							int x = line.IndexOf('=');
							if(x != -1)
							{
								sPropName = line.Substring(0, x);
								sPropName = trim(sPropName);

								sPropVal = line.Substring(x + 1);
								sPropVal = trim(sPropVal);

								bool inQuotes = false;
								string sToken = "";
								int nTokenCount = 0;
								foreach(char c in sPropVal)
								{
									if(c == '\"')
									{
										inQuotes = !inQuotes;
									}
									else
									{
										if(inQuotes)
										{
											sToken += c;
										}
										else
										{
											if(c == sListSep)
											{
												sToken = trim(sToken);
												stkPath.Peek()[sPropName].SetString(sToken, nTokenCount);
												sToken = "";
												nTokenCount++;
											}
											else
											{
												sToken += c;
											}
										}
									}
								}

								if(!string.IsNullOrEmpty(sToken))
								{
									sToken = trim(sToken);
									stkPath.Peek()[sPropName].SetString(sToken, nTokenCount);
								}
							}
							else
							{
								line = trim(line);

								if (line[0] == '{')
								{
									stkPath.Push(stkPath.Peek()[sPropName]);
								}
								else
								{
									if (line[0] == '}')
									{
										stkPath.Pop();
									}
									else
									{
										sPropName = line;
									}
								}
							}
						}
					}
				}

				reader.Close();

				return df;
			}
			
		}
	}
}