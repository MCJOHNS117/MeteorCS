using System.Collections.Generic;

namespace Meteor.Engine.Fonts
{
	public class Line
	{
		public float MaxLength { get; protected set; }
		private float _spaceSize;

		public List<Word> Words { get; protected set; }
		public float CurrentLineLength { get; protected set; }

		public Line(float spaceWidth, float fontSize, float maxLength)
		{
			Words = new List<Word>();
			CurrentLineLength = 0;
			_spaceSize = spaceWidth * fontSize;
			MaxLength = maxLength;
		}

		public bool TryAddWord(Word word)
		{
			float additionalLength = word.Width;
			additionalLength += (Words.Count > 0) ? _spaceSize : 0;
			if (CurrentLineLength + additionalLength <= MaxLength)
			{
				Words.Add(word);
				CurrentLineLength += additionalLength;
				return true;
			}
			return false;
		}
	}
}
