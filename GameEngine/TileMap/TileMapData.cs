using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GameEngine.TileMap
{
	public class Image
	{
		[XmlAttribute("source")]
		public string Source { set; get; }
		[XmlAttribute("width")]
		public uint Width { set; get; }
		[XmlAttribute("height")]
		public uint Height { set; get; }
	}

	public class TileSet
	{
		[XmlAttribute("firstgid")]
		public uint FirstGid { set; get; }
		[XmlAttribute("tilewidth")]
		public uint TileWidth { set; get; }
		[XmlAttribute("tileheight")]
		public uint TileHeight { set; get; }

		[XmlElement("image")]
		public Image Image { set; get; }
	}

	public class TileMapData
	{
		[XmlAttribute("width")]
		public uint Width { set; get; }
		[XmlAttribute("height")]
		public uint Height { set; get; }
		[XmlAttribute("tilewidth")]
		public uint TileWidth { set; get; }
		[XmlAttribute("tileheight")]
		public uint TileHeight { set; get; }

		[XmlElement("tileset")]
		public TileSet TileSet { set; get; }
		[XmlElement("layer")]
		public List<Layer> Layers { set; get; }

		public TileMapData()
		{
			Layers = new List<Layer>();
		}
	}

	public class Layer
	{
		[XmlAttribute("name")]
		public string Name { set; get; }
		[XmlAttribute("width")]
		public uint Width { set; get; }
		[XmlAttribute("height")]
		public uint Height { set; get; }

		[XmlElement("data")]
		public Data Data { set; get; }
	}

	public class Data
	{
		[XmlElement("tile")]
		public List<Tile> Tiles { set; get; }

		public Data()
		{
			Tiles = new List<Tile>();
		}
	}

	public class Tile
	{
		[XmlAttribute("gid")]
		public uint Gid { set; get; }
	}
}
