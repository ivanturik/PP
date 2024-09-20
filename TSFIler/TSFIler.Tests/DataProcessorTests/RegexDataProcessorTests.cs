using TSFiler.BusinessLogic.Services.DataProcessors;
using TSFiler.Common.Enums;

namespace TSFiler.Tests.DataProcessorTests;

public class RegexDataProcessorTests
{
    private readonly RegexDataProcessor _processor;

    public RegexDataProcessorTests()
    {
        _processor = new RegexDataProcessor();
    }

    [Fact]
    public void SupportsProcessType_ProcessTypeIsRegex_ReturnsTrue()
    {
        var processType = ProcessType.Regex;

        var result = _processor.SupportsProcessType(processType);

        Assert.True(result);
    }

    [Fact]
    public void SupportsProcessType_ProcessTypeIsNotRegex_ReturnsFalse()
    {
        var processType = ProcessType.Default;

        var result = _processor.SupportsProcessType(processType);

        Assert.False(result);
    }

    [Theory]
    [InlineData("2+3", "5")]
    [InlineData("4-2", "2")]
    [InlineData("6*7", "42")]
    [InlineData("8/2", "4")]
    [InlineData("2+4*9", "38")]
    [InlineData("(2+3)*4", "20")]
    [InlineData("10/(2+3)", "2")]
    [InlineData("2*(3+4)*2", "28")]
    public void ProcessData_ValidInput_ReturnsCorrectResult(string input, string expectedOutput)
    {
        var result = _processor.ProcessData(input);

        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public void ProcessData_DivisionByZeroOccurs_ThrowsDivideByZeroException()
    {
        var input = "5/0";

        Assert.Throws<DivideByZeroException>(() => _processor.ProcessData(input));
    }

    [Theory]
    [InlineData("2+3-4", "1")]
    [InlineData("2*3+4", "10")]
    [InlineData("2*(3+5)-1", "15")]
    public void ProcessData_HandlesMixedOperatorsAndParentheses(string input, string expectedOutput)
    {
        var result = _processor.ProcessData(input);

        Assert.Equal(expectedOutput, result);
    }
}
