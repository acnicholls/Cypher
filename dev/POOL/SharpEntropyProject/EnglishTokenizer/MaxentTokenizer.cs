//Copyright (C) 2005 Richard J. Northedge
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

using System;
using System.Collections;
using System.Text.RegularExpressions;
using SharpEntropy;
using SharpEntropy.IO;

namespace EnglishTokenizer
{
	/// <summary>
	/// Summary description for MaxentTokenizer.
	/// </summary>
	public class MaxentTokenizer
	{
		private IMaximumEntropyModel mModel;
		private Regex mAlphaNumericRegex = new Regex("^[A-Za-z0-9]+$");
		private char[] mWhitespaceChars = new char[]{' ', '\t', '\r', '\n'};

		public MaxentTokenizer(string modelFile)
		{
			mModel = new GisModel(new BinaryGisModelReader(modelFile));
		}

		public MaxentTokenizer(SharpEntropy.IO.IGisModelReader modelReader)
		{
			mModel = new GisModel(modelReader);
		}

		public string[] Tokenize(string input)
		{
			ArrayList tokens = new ArrayList();

			string[] candidateTokens = input.Split(mWhitespaceChars);
			foreach (string candidateToken in candidateTokens)
			{
				if (candidateToken.Length < 2) 
				{
					tokens.Add(candidateToken);
				}
				else if (mAlphaNumericRegex.IsMatch(candidateToken))
				{
					tokens.Add(candidateToken);
				}
				else
				{
					int startPos = 0;
					int endPos = candidateToken.Length;
					for (int currentPos = startPos + 1; currentPos < endPos; currentPos++)
					{
						double[] probabilities = mModel.Evaluate(GenerateContext(candidateToken, currentPos));
						string bestOutcome = mModel.GetBestOutcome(probabilities);
						if (bestOutcome == "T")
						{
							tokens.Add(candidateToken.Substring(startPos, currentPos - startPos));
							startPos = currentPos;
						}
					}
					tokens.Add(candidateToken.Substring(startPos, endPos - startPos));
				}
			}
			return (string[])tokens.ToArray(typeof(string));
		}

		private string[] GenerateContext(string candidateToken, int currentPos)
		{
			ArrayList predicates = new ArrayList();
			predicates.Add("p=" + candidateToken.Substring(0, currentPos));
			predicates.Add("s=" + candidateToken.Substring(currentPos));
			if (currentPos > 0)
			{
				AddCharPredicates("p1", candidateToken[currentPos - 1], predicates);
				if (currentPos > 1)
				{
					AddCharPredicates("p2", candidateToken[currentPos - 2], predicates);
					predicates.Add("p21=" + candidateToken[currentPos - 2] + candidateToken[currentPos - 1]);
				}
				else
				{
					predicates.Add("p2=bok");
				}
				predicates.Add("p1f1=" + candidateToken[currentPos - 1] + candidateToken[currentPos]);
			}
			else
			{
				predicates.Add("p1=bok");
			}
			AddCharPredicates("f1", candidateToken[currentPos], predicates);
			if (currentPos + 1 < candidateToken.Length)
			{
				AddCharPredicates("f2", candidateToken[currentPos + 1], predicates);
				predicates.Add("f12=" + candidateToken[currentPos] + candidateToken[currentPos + 1]);
			}
			else
			{
				predicates.Add("f2=bok");
			}
			if (candidateToken[0] == '&' && candidateToken[candidateToken.Length - 1] == ';')
			{
				predicates.Add("cc"); //character code
			}
			
			return (string[])predicates.ToArray(typeof(string));
		}

		private void AddCharPredicates(string key, char c, ArrayList predicates)
		{
			predicates.Add(key + "=" + c);
			if (System.Char.IsLetter(c))
			{
				predicates.Add(key + "_alpha");
				if (System.Char.IsUpper(c))
				{
					predicates.Add(key + "_caps");
				}
			}
			else if (System.Char.IsDigit(c))
			{
				predicates.Add(key + "_num");
			}
			else if (System.Char.IsWhiteSpace(c))
			{
				predicates.Add(key + "_ws");
			}
			else
			{
				if (c == '.' || c == '?' || c == '!')
				{
					predicates.Add(key + "_eos");
				}
				else if (c == '`' || c == '"' || c == '\'')
				{
					predicates.Add(key + "_quote");
				}
				else if (c == '[' || c == '{' || c == '(')
				{
					predicates.Add(key + "_lp");
				}
				else if (c == ']' || c == '}' || c == ')')
				{
					predicates.Add(key + "_rp");
				}
			}
		}
	}
}
