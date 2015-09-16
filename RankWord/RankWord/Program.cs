using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RankWord
{
	class Program
	{
		/// <summary>
		/// An iterative implementattion of the factorial function.
		/// </summary>
		/// <param name="n"></param>
		/// <returns>n! (n factorial)</returns>
		public static UInt64 factorial(UInt64 n)
		{
			UInt64 product = 1;
			if (n > 0)
			{
				for (UInt64 i = 1; i <= n; i++)
				{
					product *= i;
				}
			}
			return product;
		}

		/// <summary>
		/// This function calculates the permutation of all the characters in the dictionary characterCounts.
		/// </summary>
		/// <param name="characterCounts">A dictionary which contains letters as keys and their occurences as values.</param>
		/// <returns>The total number permutations possible.</returns>
		public static UInt64 permutations(SortedList<char, UInt64> characterCounts)
		{
			UInt64 sum = 0;
			UInt64 factorialProduct = 1;

			//Compute permutations using the the multinomial definition.
			foreach (var count in characterCounts.Values)
			{
				sum += count;
				factorialProduct *= Program.factorial(count);
			}
			return Program.factorial(sum) / factorialProduct;
		}

		/// <summary>
		/// This function knocks out a character from the dictionary characterCounts and then computes the permutations on the remnant.
		/// </summary>
		/// <param name="knockOutCharacter">The character to be removed from the dictionary characterCounts.</param>
		/// <param name="characterCounts">A dictionary which contains letters as keys and their occurences as values.</param>
		/// <returns>The total number permutations possible without knockOutCharacter.</returns>
		public static UInt64 knockOutPermutations(char knockOutCharacter, SortedList<char, UInt64> characterCounts)
		{

			//Create a copy of the dictionary characterCounts because it is passed by reference.
			SortedList<char, UInt64> knockOutCcharacterCounts = new SortedList<char, UInt64>(characterCounts);

			//Remove knockOutCharacter if possible.
			if (characterCounts.ContainsKey(knockOutCharacter) && knockOutCcharacterCounts[knockOutCharacter] > 1)
			{
				knockOutCcharacterCounts[knockOutCharacter] -= 1;
			}
			else if (characterCounts.ContainsKey(knockOutCharacter) && knockOutCcharacterCounts[knockOutCharacter] == 1)
			{
				knockOutCcharacterCounts.Remove(knockOutCharacter);
			}
			return Program.permutations(knockOutCcharacterCounts);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args">Only 1 paramater is accepted.</param>
		static void Main(string[] args)
		{
			Stopwatch timer;
			string word;
			SortedList<char, UInt64> characterCounts;
			UInt64 rank;
			UInt64 permutations;

			//Check argument length
			if (args.Length == 1)
			{
				//Start timer.
				timer = new Stopwatch();
				timer.Start();

				//Construct dictionary from input word.
				word = args[0];
				characterCounts = new SortedList<char, UInt64>();
				foreach (var character in word)
				{
					if (characterCounts.ContainsKey(character))
					{
						characterCounts[character] += 1;
					}
					else
					{
						characterCounts.Add(character, 1);
					}
				}

				//Compute rank by summing permutations before word.
				rank = 1;
				foreach (var character in word)
				{
					for (int i = 0; characterCounts.Keys[i] != character && i < characterCounts.Count; i++)
					{
						permutations = Program.knockOutPermutations(characterCounts.Keys[i], characterCounts);
						rank += permutations;
					}
					if (characterCounts[character] > 1)
					{
						characterCounts[character] -= 1;
					}
					else
					{
						characterCounts.Remove(character);
					}
				}

				//Print out results.
				Console.Out.WriteLine("Rank of " + word + " is: " + rank);
				timer.Stop();
				Console.Out.WriteLine("Time elapsed: " + timer.ElapsedMilliseconds + " milliseconds.");
			}
			else
			{
				Console.Out.WriteLine("Usage: RankWord.exe <word>");
			}
		}
	}
}
