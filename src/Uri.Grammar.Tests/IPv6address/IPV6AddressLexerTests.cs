﻿namespace Uri.Grammar
{
    using TextFx;
    using TextFx.ABNF;
    using TextFx.ABNF.Core;

    using Xunit;

    public class IPV6AddressLexerTests
    {
        [Theory]

        // 6( h16 ":" ) ls32
        [InlineData(@"1:2:3:4:5:6:7:8")]

        // "::" 5( h16 ":" ) ls32
        [InlineData(@"::1:2:3:4:5:6:7")]

        // [ h16 ] "::" 4( h16 ":" ) ls32
        [InlineData(@"::1:2:3:4:5:6")]
        [InlineData(@"1::2:3:4:5:6:7")]

        // [ *1( h16 ":" ) h16 ] "::" 3( h16 ":" ) ls32
        [InlineData(@"::1:2:3:4:5")]
        [InlineData(@"1::2:3:4:5:6")]
        [InlineData(@"1:2::3:4:5:6:7")]

        // [ *2( h16 ":" ) h16 ] "::" 2( h16 ":" ) ls32
        [InlineData(@"::1:2:3:4")]
        [InlineData(@"1::2:3:4:5")]
        [InlineData(@"1:2::3:4:5:6")]
        [InlineData(@"1:2:3::4:5:6:7")]

        // [ *3( h16 ":" ) h16 ] "::"    h16 ":"   ls32
        [InlineData(@"::1:2:3")]
        [InlineData(@"1::2:3:4")]
        [InlineData(@"1:2::3:4:5")]
        [InlineData(@"1:2:3::4:5:6")]
        [InlineData(@"1:2:3:4::5:6:7")]

        // [ *4( h16 ":" ) h16 ] "::" ls32
        [InlineData(@"::1:2")]
        [InlineData(@"1::2:3")]
        [InlineData(@"1:2::3:4")]
        [InlineData(@"1:2:3::4:5")]
        [InlineData(@"1:2:3:4::5:6")]
        [InlineData(@"1:2:3:4:5::6:7")]

        // [ *5( h16 ":" ) h16 ] "::" h16
        [InlineData(@"::1")]
        [InlineData(@"1::2")]
        [InlineData(@"1:2::3")]
        [InlineData(@"1:2:3::4")]
        [InlineData(@"1:2:3:4::5")]
        [InlineData(@"1:2:3:4:5::6")]
        [InlineData(@"1:2:3:4:5:6::7")]

        //  [ *6( h16 ":" ) h16 ] "::"
        [InlineData(@"::")]
        [InlineData(@"1::")]
        [InlineData(@"1:2::")]
        [InlineData(@"1:2:3::")]
        [InlineData(@"1:2:3:4::")]
        [InlineData(@"1:2:3:4:5::")]
        [InlineData(@"1:2:3:4:5:6::")]
        [InlineData(@"1:2:3:4:5:6:7::")]
        public void Read_ShouldSucceed(string input)
        {
            var terminalLexerFactory = new TerminalLexerFactory();
            var sequenceLexerFactory = new SequenceLexerFactory();
            var valueRangeLexerFactory = new ValueRangeLexerFactory();
            var alternativeLexerFactory = new AlternativeLexerFactory();
            var repetitionLexerFactory = new RepetitionLexerFactory();
            var digitLexerFactory = new DigitLexerFactory(valueRangeLexerFactory);
            var decimalOctetLexerFactory = new DecimalOctetLexerFactory(
                valueRangeLexerFactory,
                terminalLexerFactory,
                alternativeLexerFactory,
                repetitionLexerFactory,
                digitLexerFactory,
                sequenceLexerFactory);
            var optionLexerFactory = new OptionLexerFactory();
            var hexadecimalDigitLexerFactory = new HexadecimalDigitLexerFactory(
                digitLexerFactory,
                terminalLexerFactory,
                alternativeLexerFactory);
            var hexadecimalInt16LexerFactory = new HexadecimalInt16LexerFactory(
                repetitionLexerFactory,
                hexadecimalDigitLexerFactory);
            var ipv4AddressLexerFactory = new IPV4AddressLexerFactory(
                sequenceLexerFactory,
                terminalLexerFactory,
                decimalOctetLexerFactory);
            var leastSignificantInt32LexerFactory = new LeastSignificantInt32LexerFactory(
                alternativeLexerFactory,
                sequenceLexerFactory,
                terminalLexerFactory,
                hexadecimalInt16LexerFactory,
                ipv4AddressLexerFactory);
            var factory = new IPv6AddressLexerFactory(
                alternativeLexerFactory,
                sequenceLexerFactory,
                terminalLexerFactory,
                repetitionLexerFactory,
                optionLexerFactory,
                hexadecimalInt16LexerFactory,
                leastSignificantInt32LexerFactory);
            var lexer = factory.Create();
            using (var scanner = new TextScanner(new StringTextSource(input)))
            {
                var result = lexer.Read(scanner, null);
                Assert.NotNull(result);
                Assert.True(result.Success);
                Assert.NotNull(result.Element);
                Assert.Equal(input, result.Element.Text);
            }
        }
    }
}