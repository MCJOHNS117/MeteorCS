namespace Meteor.Game.Data
{
	public class Block
	{
		private ushort data;

		private static readonly ushort TYPE_MASK =	0x00FF;
		private static readonly ushort FLAGS_MASK =	0xFF00;

		public byte Type { get { return (byte)(data & TYPE_MASK); } }
		public byte Flags { get { return (byte)((data & FLAGS_MASK) >> 8); } }

		public Block(byte type, byte flags)
		{
			data = 0;
			data |= type;
			data |= (ushort)(flags << 8);
		}
	}
}