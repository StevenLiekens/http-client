﻿namespace Uri.Grammar
{
    using System;

    using TextFx;
    using TextFx.ABNF;
    using TextFx.ABNF.Core;

    public class DecimalOctetLexerFactory : ILexerFactory<DecimalOctet>
    {
        private readonly IAlternativeLexerFactory alternativeLexerFactory;

        private readonly ILexerFactory<Digit> digitLexerFactory;

        private readonly IRepetitionLexerFactory repetitionLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        private readonly IValueRangeLexerFactory valueRangeLexerFactory;

        public DecimalOctetLexerFactory(
            IValueRangeLexerFactory valueRangeLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            IAlternativeLexerFactory alternativeLexerFactory,
            IRepetitionLexerFactory repetitionLexerFactory,
            ILexerFactory<Digit> digitLexerFactory,
            IConcatenationLexerFactory concatenationLexerFactory)
        {
            if (valueRangeLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(valueRangeLexerFactory));
            }

            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }

            if (alternativeLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alternativeLexerFactory));
            }

            if (repetitionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(repetitionLexerFactory));
            }

            if (digitLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(digitLexerFactory));
            }

            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }

            this.valueRangeLexerFactory = valueRangeLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.alternativeLexerFactory = alternativeLexerFactory;
            this.repetitionLexerFactory = repetitionLexerFactory;
            this.digitLexerFactory = digitLexerFactory;
            this.concatenationLexerFactory = concatenationLexerFactory;
        }

        public ILexer<DecimalOctet> Create()
        {
            // %x30-35
            var a = this.valueRangeLexerFactory.Create('\x30', '\x35');

            // "25"
            var b = this.terminalLexerFactory.Create("25", StringComparer.Ordinal);

            // "25" %x30-35 
            var c = this.concatenationLexerFactory.Create(b, a);

            // DIGIT
            var d = this.digitLexerFactory.Create();

            // %x30-34
            var e = this.valueRangeLexerFactory.Create('\x30', '\x34');

            // "2"
            var f = this.terminalLexerFactory.Create("2", StringComparer.Ordinal);

            // "2" %x30-34 DIGIT 
            var g = this.concatenationLexerFactory.Create(f, e, d);

            // 2DIGIT
            var h = this.repetitionLexerFactory.Create(d, 2, 2);

            // "1"
            var i = this.terminalLexerFactory.Create("1", StringComparer.Ordinal);

            // "1" 2DIGIT  
            var j = this.concatenationLexerFactory.Create(i, h);

            // %x31-39
            var k = this.valueRangeLexerFactory.Create('\x31', '\x39');

            // %x31-39 DIGIT 
            var l = this.concatenationLexerFactory.Create(k, d);

            // "25" %x30-35 / "2" %x30-34 DIGIT / "1" 2DIGIT / %x31-39 DIGIT / DIGIT
            var m = this.alternativeLexerFactory.Create(c, g, j, l, d);

            // dec-octet
            return new DecimalOctetLexer(m);
        }
    }
}