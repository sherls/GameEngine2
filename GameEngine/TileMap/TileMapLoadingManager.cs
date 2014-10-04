using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace GameEngine.TileMap
{
	public class TileMapLoadingManager : Singleton<TileMapLoadingManager>
	{
		public TileMapData ImportTmx(string i_fileName)
		{
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content\\TMX\\" + i_fileName);

			TileMapData newTileSet;
			XmlSerializer serializer = new XmlSerializer(typeof(TileMapData), new XmlRootAttribute("map"));

			XmlReader reader = XmlReader.Create(filePath);
			newTileSet = (TileMapData)serializer.Deserialize(reader);
			reader.Close();

			return newTileSet;
		}
	}
}
