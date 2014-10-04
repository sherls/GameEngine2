using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

// XNA
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Data
{
	public class DataLoadingManager : Singleton<DataLoadingManager>
	{
		public XmlData ImportXml(string i_fileName)
		{
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content\\XML\\" + i_fileName);

			XmlData newXmlData;
			XmlSerializer serializer = new XmlSerializer(typeof(XmlData));

			XmlReader reader = XmlReader.Create(filePath);
			newXmlData = (XmlData)serializer.Deserialize(reader);
			reader.Close();

			return newXmlData;
		}

		public void ExportXml(string i_fileName)
		{
			//string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\IntroProjectContent\\" + i_fileName);

			//XmlData newXmlData = CreateXmlDataInstance();
			//XmlSerializer serializer = new XmlSerializer(typeof(XmlData));
			//XmlWriterSettings settings = new XmlWriterSettings();
			//settings.Indent = true;

			//XmlWriter writer = XmlWriter.Create(filePath, settings);

			//serializer.Serialize(writer, newXmlData);

			//writer.Close();
		}

		public Dictionary<string, object> InstantiateObjectsFromXmlObject(XmlData i_data)
		{
			Dictionary<string, object> createdObjects = new Dictionary<string, object>();
			foreach (var xmlItem in i_data.Entities)
			{
				var newObject = CreateRuntimeObjectFromXml(xmlItem);

				if (createdObjects.ContainsKey(xmlItem.Name))
				{
					throw new Exception("The XML data has two keys with the name " + xmlItem.Name);
				}

				createdObjects.Add(xmlItem.Name, newObject);
			}

			return createdObjects;
		}

		public object CreateRuntimeObjectFromXml(XmlItem i_xmlItem)
		{
			object newObject = null;
			Type objectType = null;
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			newObject = null;
			objectType = null;
			foreach (var assembly in assemblies)
			{
				if (assembly.FullName.Contains("Microsoft") || assembly.FullName.Contains("XNA") ||
					assembly.Location.Contains("Microsoft"))
					continue;

				var allTypes = assembly.GetTypes();
				foreach (var type in allTypes)
				{
					if (type.Name == i_xmlItem.Type)
					{
						objectType = type;
						break;
					}
				}
				if (objectType != null)
					break;
			}

			newObject = CreateObject(objectType, i_xmlItem, newObject);
			SetupObjectSpecificProperty(objectType, i_xmlItem, ref newObject);

			return newObject;
		}

		private object CreateObject(Type objectType, XmlItem xmlItem, object newObject)
		{
#if DEBUG
			if (objectType == null)
				throw new Exception("Could not find a matching type for " + xmlItem.Type);
#endif

			var constructor = objectType.GetConstructor(new Type[0]);

#if DEBUG
			if (constructor == null)
				throw new Exception("Could not find default constructor for " + xmlItem.Type);
#endif

			newObject = constructor.Invoke(new object[0]);

#if DEBUG
			if (newObject == null)
				throw new Exception("Could not find a matching default constructor for " + xmlItem.Type);
#endif
			return newObject;
		}

		private void SetupObjectSpecificProperty(Type i_objectType, XmlItem i_xmlItem, ref object io_newObject)
		{
			foreach (CustomProperty customProperty in i_xmlItem.CustomProperties)
			{
				string nameOfProperty = customProperty.Name;

				var dynamicProperty = i_objectType.GetProperty(nameOfProperty);
				if (dynamicProperty != null)
				{
					var xmlNodeArray = customProperty.Value as System.Xml.XmlNode[];
					string valueAsString = xmlNodeArray[0].Value;

					if (dynamicProperty.Name == "Texture")
					{
						Texture2D texture = EngineManager.Self.Load<Texture2D>(valueAsString);
						dynamicProperty.SetValue(io_newObject, texture, null);
					}
					else if (dynamicProperty.Name == "Font")
					{
						SpriteFont font = EngineManager.Self.Load<SpriteFont>(valueAsString);
						dynamicProperty.SetValue(io_newObject, font, null);
					}
					else if (dynamicProperty.Name == "HorizontalAlignment")
					{
						var valueAsUint32 = Convert.ToUInt32(valueAsString);
						var value = HAlignment.Left;
						if (valueAsUint32 == 1)
							value = HAlignment.Centre;
						else if (valueAsUint32 == 2)
							value = HAlignment.Right;
						dynamicProperty.SetValue(io_newObject, value, null);
					}
					else if (dynamicProperty.Name == "VerticalAlignment")
					{
						var valueAsUint32 = Convert.ToUInt32(valueAsString);
						var value = VAlignment.Top;
						if (valueAsUint32 == 1)
							value = VAlignment.Centre;
						else if (valueAsUint32 == 2)
							value = VAlignment.Bottom;
						dynamicProperty.SetValue(io_newObject, value, null);
					}
					else if (dynamicProperty.Name == "SizeUnits")
					{
						var valueAsUint32 = Convert.ToUInt32(valueAsString);
						var value = SizeUnits.Absolute;
						if (valueAsUint32 == 1)
							value = SizeUnits.RelativeToParent;
						dynamicProperty.SetValue(io_newObject, value, null);
					}
					else
					{
						var expectedType = dynamicProperty.PropertyType;
						var convertedValue = Convert.ChangeType(valueAsString, expectedType);
						dynamicProperty.SetValue(io_newObject, convertedValue, null);
					}
				}
			}

			var fields = i_objectType.GetFields();
			foreach (var field in fields)
			{
				if (field.Name == "Offset")
					field.SetValue(io_newObject, Convert.ChangeType(i_xmlItem.Offset, field.FieldType));
				else if (field.Name == "Size")
					field.SetValue(io_newObject, Convert.ChangeType(i_xmlItem.Size, field.FieldType));
				else if (field.Name == "Velocity")
					field.SetValue(io_newObject, Convert.ChangeType(i_xmlItem.Velocity, field.FieldType));
				else if (field.Name == "Acceleration")
					field.SetValue(io_newObject, Convert.ChangeType(i_xmlItem.Acceleration, field.FieldType));
				else if (field.Name == "Name")
					field.SetValue(io_newObject, Convert.ChangeType(i_xmlItem.Name, field.FieldType));
			}
		}
	}
}
