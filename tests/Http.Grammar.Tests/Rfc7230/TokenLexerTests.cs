﻿using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Text.Scanning;

namespace Http.Grammar.Rfc7230
{
    [TestClass]
    public class TokenLexerTests
    {
        [TestMethod]
        public void ReadToken()
        {
            const string input = "Content-Type";
            var lexer = new TokenLexer();
            using (TextReader textReader = new StringReader(input))
            using (ITextScanner textScanner = new TextScanner(textReader))
            {
                textScanner.Read();
                var token = lexer.Read(textScanner);
                Assert.IsNotNull(token);
                Assert.IsNotNull(token.Data);
                Assert.AreEqual(input, token.Data);
            }
        }
    }
}
