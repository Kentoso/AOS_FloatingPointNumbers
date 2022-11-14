using AOS_floatingpointnumbers;
using Xunit;

namespace AOS_floationgpointnumbers.Tests;

public class UnitTest1
{
    [Fact]
    public void ParsingTest()
    {
        // FloatingPointNumber f1 = new FloatingPointNumber(10, 10, "12.3E+10");
        // Assert.Equal(("12", "3", "+10"), f1.ParseDecimal());
        // FloatingPointNumber f2 = new FloatingPointNumber(10, 10, "-112.3E+1042");
        // Assert.Equal(("-112", "3", "+1042"), f2.ParseDecimal());
        // FloatingPointNumber f3 = new FloatingPointNumber(10, 10, "22212.512E+10");
        // Assert.Equal(("22212", "512", "+10"), f3.ParseDecimal());
        // FloatingPointNumber f4 = new FloatingPointNumber(10, 10, "8546.1232E+10");
        // Assert.Equal(("8546", "1232", "+10"), f4.ParseDecimal());
    }

    [Fact]
    public void FractionConversion()
    {
        // Assert.Equal("0100110011", FloatingPointNumber.ConvertFractionToBinary("3", 10));
        // var res3 = FloatingPointNumber.ConvertFractionToBinary("05", 6);
        // Assert.Equal("000011", res3);
        // Assert.Equal("001010111", FloatingPointNumber.ConvertFractionToBinary("17", 9));
    }

    [Fact]
    public void FloatingPointTest()
    {
        FloatingPointNumber number = new FloatingPointNumber(8, 23, "263.3E0");
    }

    [Fact]
    public void MultiplicationTest()
    {
        // var res = FloatingPointNumber.MultiplyStringNumberByTwo("010");
        // Assert.Equal(("020", "0"), res);
        // res = FloatingPointNumber.MultiplyStringNumberByTwo("500");
        // Assert.Equal(("000", "1"), res);
    }

    [Fact]
    public void ComparisonTest()
    {
        Assert.Equal(-1, HelperFunctions.CompareStringNumbers("1231", "2212"));
        Assert.Equal(0, HelperFunctions.CompareStringNumbers("1000", "1000"));
        Assert.Equal(1, HelperFunctions.CompareStringNumbers("1200", "1000"));
    }

    [Fact]
    public void SubtractionTest()
    {
        var r1 = HelperFunctions.SubtractStringNumbers("100", "90");
        Assert.Equal("10", r1);
        var r2 = HelperFunctions.SubtractStringNumbers("50", "40");
        Assert.Equal("10", r2);
        Assert.Equal("10", HelperFunctions.SubtractStringNumbers("1200", "1190"));
    }

    [Fact]
    public void WholePartTest()
    {
        var r1 = HelperFunctions.ConvertWholePartToBinary("18");
        Assert.Equal("10010", r1);
    }
}