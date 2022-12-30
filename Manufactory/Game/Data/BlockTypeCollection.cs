using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MeteorGame
{
	public class BlockTypeCollection : Dictionary<ushort, BlockType>
	{
		public BlockTypeCollection()
		{
		}

		public void LoadFromFile(string path)
		{
			Clear();

			XmlSerializer serializer = new XmlSerializer(typeof(List<BlockType>));
			StreamReader reader = new StreamReader(File.Open(path, FileMode.Open));
			List<BlockType> values = serializer.Deserialize(reader) as List<BlockType>;
			reader.Close();

			foreach (BlockType block in values)
			{
				this.Add(block.Type, block);
			}
		}

		public void SaveToFile(string path)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(List<BlockType>));
			StreamWriter writer = new StreamWriter(File.Open(path, FileMode.Create));
			serializer.Serialize(writer, this.Values.ToList());
			writer.Close();
		}
	}
}