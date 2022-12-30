using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MeteorEngine
{
	public class MetaFile
	{
		private const int PAD_TOP = 0;
		private const int PAD_LEFT = 1;
		private const int PAD_BOTTOM = 2;
		private const int PAD_RIGHT = 3;

		private const int DESIRED_PADDING = 3;

		private const char SPLITTER = ' ';
		private const char NUMBER_SEPERATOR = ',';

		private float _aspectRatio;

		private float verticalPerPixelSize;
		private float horizontalPerPixelSize;
		public float SpaceWidth { get; protected set; }
		private int[] padding;
		private int paddingWidth;
		private int paddingHeight;

		private Dictionary<int, Character> metaData = new Dictionary<int, Character>();

		private StreamReader _reader;
		private Dictionary<string, string> values = new Dictionary<string, string>();

		public MetaFile(string filename, float aspectRatio)
		{
			_aspectRatio = aspectRatio;
			OpenFile(filename);
			LoadPaddingData();
			LoadLineSizes();
			int imageWidth = GetValueOfVariable("scaleW");
			LoadCharacterData(imageWidth);
			Close();
		}

		public Character GetCharacter(int ascii)
		{
			return metaData.ContainsKey(ascii) ? metaData[ascii] : null;
		}

		private void OpenFile(string filename)
		{
			try
			{
				_reader = new StreamReader(File.Open(filename, FileMode.Open));
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
			}
		}

		private void Close()
		{
			try
			{
				_reader.Close();
			}
			catch { }
		}

		private void LoadPaddingData()
		{
			ProcessNextLine();
			padding = GetValuesOfVariable("padding");
			paddingWidth = padding[PAD_LEFT] + padding[PAD_RIGHT];
			paddingHeight = padding[PAD_TOP] + padding[PAD_BOTTOM];
		}

		private bool ProcessNextLine()
		{
			values.Clear();
			string line = null;
			try
			{
				line = _reader.ReadLine();
			}
			catch
			{

			}

			if (line == null)
				return false;

			string[] tokens = line.Split(SPLITTER);
			foreach (string part in tokens)
			{
				string[] valuePairs = part.Split('=');
				if (valuePairs.Length == 2)
					values.Add(valuePairs[0], valuePairs[1]);
			}
			return true;
		}

		private int GetValueOfVariable(string variable)
		{
			if (values.ContainsKey(variable))
				return Int32.Parse(values[variable]);

			return int.MaxValue;
		}

		private int[] GetValuesOfVariable(string variable)
		{
			string[] nums = new string[0];
			if (values.ContainsKey(variable))
				nums = values[variable].Split(NUMBER_SEPERATOR);

			int[] intVals = new int[nums.Length];
			for (int i = 0; i < nums.Length; i++)
			{
				intVals[i] = Int32.Parse(nums[i]);
			}

			return intVals;
		}

		private void LoadLineSizes()
		{
			ProcessNextLine();
			int lineHeightPixels = GetValueOfVariable("lineHeight") - paddingHeight;
			verticalPerPixelSize = TextMeshCreator.LINE_HEIGHT / (float)lineHeightPixels;
			horizontalPerPixelSize = verticalPerPixelSize / _aspectRatio;
		}

		private void LoadCharacterData(int imageWidth)
		{
			ProcessNextLine();
			ProcessNextLine();
			while (ProcessNextLine())
			{
				Character c = LoadCharacter(imageWidth);
				if (c != null)
					metaData.Add(c.Id, c);
			}
		}

		private Character LoadCharacter(int imageSize)
		{
			int id = GetValueOfVariable("id");
			if (id == int.MaxValue)
			{
				return null;
			}

			if (id == TextMeshCreator.SPACE_ASCII)
			{
				SpaceWidth = (GetValueOfVariable("xadvance") - paddingWidth) * horizontalPerPixelSize;
				return null;
			}
			float xTex = ((float)GetValueOfVariable("x") + (padding[PAD_LEFT] - DESIRED_PADDING)) / imageSize;
			float yTex = ((float)GetValueOfVariable("y") + (padding[PAD_TOP] - DESIRED_PADDING)) / imageSize;
			int width = GetValueOfVariable("width") - (paddingWidth - (2 * DESIRED_PADDING));
			int height = GetValueOfVariable("height") - ((paddingHeight) - (2 * DESIRED_PADDING));
			float quadWidth = width * horizontalPerPixelSize;
			float quadHeight = height * verticalPerPixelSize;
			float xTexSize = (float)width / imageSize;
			float yTexSize = (float)height / imageSize;
			float xOff = (GetValueOfVariable("xoffset") + padding[PAD_LEFT] - DESIRED_PADDING) * horizontalPerPixelSize;
			float yOff = (GetValueOfVariable("yoffset") + (padding[PAD_TOP] - DESIRED_PADDING)) * verticalPerPixelSize;
			float xAdvance = (GetValueOfVariable("xadvance") - paddingWidth) * horizontalPerPixelSize;
			return new Character(id, xTex, yTex, xTexSize, yTexSize, xOff, yOff, quadWidth, quadHeight, xAdvance);
		}
	}
}