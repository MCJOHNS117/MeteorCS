using System.Collections.Generic;

namespace Meteor.Engine.Fonts
{
	public class Word
	{
		public List<Character> Characters { get; private set; }

		public float Width { get; private set; }

		public float FontSize { get; protected set; }

		public Word(float fontSize)
		{
			FontSize = fontSize;
			Width = 0;
			Characters = new List<Character>();
		}

		public void AddCharacter(Character character)
		{
			Characters.Add(character);
			Width += character.XAdvance * FontSize;
		}
	}
}
