using System;
using System.Xml.Serialization;
using System.Collections.Generic;

// XNA
using Microsoft.Xna.Framework;

namespace GameEngine.Data
{
	public class CustomProperty
	{
		public string Name;
		public object Value;
	}

	public class XmlItem
	{
		public Vector2 Offset { set; get; }
		public Vector2 Size { set; get; }
		public Vector2 Velocity { set; get; }
		public Vector2 Acceleration { set; get; }
		public Color Colour { set; get; }
		public string Name { set; get; }
		public string Type { set; get; }

		[XmlElement("CustomProperty")]
		public List<CustomProperty> CustomProperties { set; get; }

		public XmlItem()
		{
			CustomProperties = new List<CustomProperty>();
		}
	}

	public class XmlData
	{
		[XmlElement("Entity")]
		public List<XmlItem> Entities { set; get; }

		public XmlData()
		{
			Entities = new List<XmlItem>();
		}
	}
}
