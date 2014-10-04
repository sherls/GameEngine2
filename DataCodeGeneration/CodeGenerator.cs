using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameEngine.Data;

namespace DataCodeGeneration
{
	public class CodeGenerator
	{
		public string GenerateCodeFor(string i_xmlFile, string i_xmlFileAtRuntime, string i_className, string i_namespaceName)
		{
			var xmlData = DataLoadingManager.Self.ImportXml(i_xmlFile);

			StringBuilder builder = new StringBuilder();

			GenerateUsings(builder);

			GenerateNamespaceHeader(builder, i_namespaceName);

			GenerateClassHeader(builder, i_className);

			DeclareClassMembers(xmlData, builder);

			GenerateInitializeLayoutHeader(builder);

			GenerateLoadAndCreateDictionary(builder, i_xmlFileAtRuntime);

			AssignMembersFromXml(xmlData, builder);

			// End of InitializeLayout function
			builder.AppendLine("\t\t}");

			// End of class
			builder.AppendLine("\t}");

			// End of namespace
			builder.AppendLine("}");

			return builder.ToString();
		}

		private void GenerateUsings(StringBuilder i_builder)
		{
			i_builder.AppendLine("using GameEngine;");
			i_builder.AppendLine("using GameEngine.UI;");
			i_builder.AppendLine("using GameEngine.Data;");
			i_builder.AppendLine("using GameEngine.Physics;");
		}

		private void GenerateNamespaceHeader(StringBuilder i_builder, string i_namespaceName)
		{
			i_builder.AppendLine("namespace " + i_namespaceName);
			i_builder.AppendLine("{");
		}

		private void GenerateClassHeader(StringBuilder i_builder, string i_className)
		{
			i_builder.AppendLine("\tpublic partial class " + i_className);
			i_builder.AppendLine("\t{");
		}

		private void DeclareClassMembers(XmlData i_xmlData, StringBuilder i_builder)
		{
			foreach (var entity in i_xmlData.Entities)
				i_builder.AppendLine(string.Format("\t\t{0} {1};", entity.Type, entity.Name));
		}

		private void GenerateInitializeLayoutHeader(StringBuilder i_builder)
		{
			i_builder.AppendLine("\t\tpublic void InitializeLayout()");
			i_builder.AppendLine("\t\t{");
		}

		private void GenerateLoadAndCreateDictionary(StringBuilder i_builder, string i_xmlFileAtRunTime)
		{
			i_builder.AppendLine("\t\t\tvar xmlData = DataLoadingManager.Self.ImportXml(\"" + i_xmlFileAtRunTime + "\");");
			i_builder.AppendLine("\t\t\tvar createdItems = DataLoadingManager.Self.InstantiateObjectsFromXmlObject(xmlData);");
		}

		private void AssignMembersFromXml(XmlData i_xmlData, StringBuilder i_builder)
		{
			foreach (var entity in i_xmlData.Entities)
			{
				i_builder.AppendLine(string.Format("\t\t\t{0} = createdItems[\"{0}\"] as {1};",
					entity.Name, entity.Type));

				i_builder.AppendLine(string.Format("\t\t\t{0}.Parent = this;", entity.Name));
				i_builder.AppendLine(string.Format("\t\t\tif({0} is IEngineAddable)", entity.Name));
				i_builder.AppendLine(string.Format("\t\t\t\t({0} as IEngineAddable).AddToEngine();", entity.Name));
			}
		}
	}
}
