using OpenTK;
using System.Collections.Generic;

namespace Meteor.Engine.Fonts
{
	public class TextMeshCreator
	{
		public const float LINE_HEIGHT = 0.03f;
		public const int SPACE_ASCII = 32;

		private MetaFile metaData;

		public TextMeshCreator(string filename, float aspectRatio)
		{
			metaData = new MetaFile(filename, aspectRatio);
		}

		public TextMeshData CreateTextMesh(GUIText text)
		{
			List<Line> lines = CreateStructure(text);
			TextMeshData data = CreateQuadVertices(text, lines);
			return data;
		}

		private List<Line> CreateStructure(GUIText text)
		{
			char[] chars = text.Text.ToCharArray();
			List<Line> lines = new List<Line>();
			Line currentLine = new Line(metaData.SpaceWidth, text.FontSize, text.MaxLineSize);
			Word currentWord = new Word(text.FontSize);
			foreach(char c in chars)
			{
				int ascii = (int)c;
				if(ascii == SPACE_ASCII)
				{
					bool added = currentLine.TryAddWord(currentWord);
					if(!added)
					{
						lines.Add(currentLine);
						currentLine = new Line(metaData.SpaceWidth, text.FontSize, text.MaxLineSize);
						currentLine.TryAddWord(currentWord);
					}
					currentWord = new Word(text.FontSize);
					continue;
				}
				Character character = metaData.GetCharacter(ascii);
				currentWord.AddCharacter(character);
			}
			CompleteStructure(lines, currentLine, currentWord, text);
			return lines;
		}

		private void CompleteStructure(List<Line> lines, Line currentLine, Word currentWord, GUIText text)
		{
			bool added = currentLine.TryAddWord(currentWord);
			if(!added)
			{
				lines.Add(currentLine);
				currentLine = new Line(metaData.SpaceWidth, text.FontSize, text.MaxLineSize);
				currentLine.TryAddWord(currentWord);
			}
			lines.Add(currentLine);
		}

		private TextMeshData CreateQuadVertices(GUIText text, List<Line> lines)
		{
			text.LineCount = lines.Count;
			float cursorX = 0f;
			float cursorY = 0f;
			List<FontVertex> vertices = new List<FontVertex>();
			foreach(Line line in lines)
			{
				if (text.Centered)
					cursorX = (line.MaxLength - line.CurrentLineLength) / 2;
				foreach(Word word in line.Words)
				{
					foreach(Character token in word.Characters)
					{
						AddVerticesForCharacter(vertices, cursorX, cursorY, token, text.FontSize);
						cursorX += token.XAdvance * text.FontSize;
					}
					cursorX += metaData.SpaceWidth * text.FontSize;
				}
				cursorX = 0;
				cursorY += LINE_HEIGHT * text.FontSize;
			}

			return new TextMeshData(vertices.ToArray());
		}

		private void AddVerticesForCharacter(List<FontVertex> vertices, float curserX, float curserY, Character character, float fontSize)
		{
			float x = curserX + (character.XOffset * fontSize);
			float y = curserY + (character.YOffset * fontSize);
			float maxX = x + (character.sizeX * fontSize);
			float maxY = y + (character.sizeY * fontSize);
			float properX = (2 * x) - 1;
			float properY = (-2 * y) + 1;
			float properMaxX = (2 * maxX) - 1;
			float properMaxY = (-2 * maxY) + 1;
			addVertices(vertices, character, properX, properY, properMaxX, properMaxY);
		}

		private static void addVertices(List<FontVertex> vertices, Character token, float x, float y, float maxX, float maxY)
		{
			vertices.Add(new FontVertex(new Vector2(x, y), new Vector2(token.X, token.Y)));
			vertices.Add(new FontVertex(new Vector2(x, maxY), new Vector2(token.X, token.Y + token.Height)));
			vertices.Add(new FontVertex(new Vector2(maxX, maxY), new Vector2(token.X + token.Width, token.Y + token.Height)));
			vertices.Add(new FontVertex(new Vector2(maxX, maxY), new Vector2(token.X + token.Width, token.Y + token.Height)));
			vertices.Add(new FontVertex(new Vector2(maxX, y), new Vector2(token.X + token.Width, token.Y)));
			vertices.Add(new FontVertex(new Vector2(x, y), new Vector2(token.X, token.Y)));
		}
	}
}