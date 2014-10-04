using System.Text;

using GameEngine.Data;

namespace DataCodeGeneration
{
	public class CodeGenerator
	{
		public string GenerateCodeFor(string i_xmlFile)
		{
			var xmlData = DataLoadingManager.Self.ImportXml(i_xmlFile);

			StringBuilder builder = new StringBuilder();

			foreach (var entity in xmlData.Entities)
			{
				builder.AppendLine(string.Format("{0} = createdItems[\"{0}\"] as {1}",
					entity.Name, entity.Type)
					);

				builder.AppendLine(string.Format("{0}.Parent = this", entity.Name));
			}

			return builder.ToString();
		}
	}
}
