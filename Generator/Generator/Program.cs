using DataCodeGeneration;

namespace Generator
{
	class Program
	{
		static void Main(string[] args)
		{
			string sourceXML = args[0];
			string xmlRelativeToProject = args[1];
			string whereToGenerateCode = args[2];
			string className = args[3];
			string namespaceName = args[4];

			CodeGenerator generator = new CodeGenerator();
			string result = generator.GenerateCodeFor(sourceXML, xmlRelativeToProject, className, namespaceName);

			System.IO.File.WriteAllText(whereToGenerateCode, result);
		}
	}
}