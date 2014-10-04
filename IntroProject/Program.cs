using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Class
{
	class Program
	{
		public class Monster
		{
			public int AttackDamage { set; get; }
			public int MaxHealth { set; get; }
		}

		static void Main(string[] args)
		{
			/*List<Monster> monsterList = new List<Monster>();

			foreach( Monster currentMonster in monsterList.OrderBy(monster => monster.MaxHealth))
			{
			}*/

			List<int> listOfIntegers = new List<int>();

			System.Random random = new Random();
			const int upperBound = 1000;
			for (int i = 0; i < 100; ++i)
			{
				listOfIntegers.Add(random.Next(upperBound));
			}

			//foreach (int currentInt in listOfIntegers.Where(i => i > 800))
			//foreach (int currentInt in listOfIntegers.Where((i) => { return i > 800; }))
			//foreach( int currentInt in listOfIntegers.Where(GreaterThanPredicate))
			//foreach( int currentInt in listOfIntegers.Where(i=> i > 500 & i < 550))
			//foreach( int currentInt in listOfIntegers.OrderBy(i=>-i))
			foreach (int currentInt in listOfIntegers
				.Where(i => i < 500)
				.OrderBy(i =>i)
				.Where(i => i % 2 == 0)
				)
			{
				//if( currentInt > 800 )
				System.Console.WriteLine(currentInt);
			}

			bool areAny = listOfIntegers.Any( i => i > 600 && i < 610 );
			Console.WriteLine("Any found " + areAny);

			//int someValue = 1000;

			//Func<int, int, bool> delegateForPrintSumOf = (firstNumber, secondNumber) =>
			//{
			//    Console.WriteLine(
			//        string.Format("If you add {0} and {1} you get {2}",
			//        firstNumber, secondNumber, firstNumber + secondNumber));
			//    Console.WriteLine("And some value is " + someValue);

			//    return true;
			//};

			//delegateForPrintSumOf(1, 2);
			//Action<int, int> delegateForPrintSumOf = (firstNumber, secondNumber) =>
			//{
			//    Console.WriteLine(
			//        string.Format("If you add {0} and {1} you get {2}",
			//        firstNumber, secondNumber, firstNumber + secondNumber));
			//};

			//Action<int, int> delegateForPrintSumOf = PrintSumOf;
			//PrintSumOf(10, 234);
			//PrintSumOf(2265, 1);

			Console.ReadLine();
		}

		static bool GreaterThanPredicate(int value)
		{
			return value > 800;
		}

		//static void PrintSumOf(int firstNumber, int secondNumber)
		//{
		//    Console.WriteLine(
		//        string.Format("If you add {0} and {1} you get {2}",
		//        firstNumber, secondNumber, firstNumber + secondNumber));
		//}
	}
}
