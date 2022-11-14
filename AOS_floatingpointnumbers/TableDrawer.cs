namespace AOS_floatingpointnumbers;

public class TableDrawer
{
    public List<FloatingPointNumber> Numbers;
    public int MaxMantissaWidth = 0;
    public int MaxExponentWidth = 0;
    public int MaxDecimalDigits = 0;
    public TableDrawer(List<FloatingPointNumber> numbers)
    {
        Numbers = numbers;
        foreach (var number in Numbers)
        {
            var lengths = number.GetLengths();
            if (MaxExponentWidth < lengths.exponent)
            {
                MaxExponentWidth = lengths.exponent;
            }
            if (MaxMantissaWidth < lengths.mantissa)
            {
                MaxMantissaWidth = lengths.mantissa;
            }

            int decimalLength = number.GetDecimalString().Length;
            if (MaxDecimalDigits < decimalLength)
            {
                MaxDecimalDigits = decimalLength;
            }
        }
    }

    public void DrawTable()
    {
        string signHeader = "Sign";
        string exponentHeader = "Exponent";
        string mantissaHeader = "Mantissa";
        string normalBitHeader = "Normal Bit";
        string decimalHeader = "Decimal";
        exponentHeader += MaxExponentWidth - exponentHeader.Length > 0 ? new string(' ', MaxExponentWidth - exponentHeader.Length): "";  
        mantissaHeader += MaxMantissaWidth - mantissaHeader.Length > 0 ? new string(' ', MaxMantissaWidth - mantissaHeader.Length): "";
        decimalHeader += CalculatePadding(decimalHeader, MaxDecimalDigits);
        string headers = $"| {signHeader} | {exponentHeader} | {normalBitHeader} | {mantissaHeader} | {decimalHeader} |";
        Console.WriteLine(new string('_', headers.Length));
        Console.WriteLine(headers);
        Console.WriteLine(new string('=', headers.Length));
        foreach (var number in Numbers)
        {
            string exponent = "";
            string mantissa = "";
            foreach (var bit in number.Exponent)
            {
                exponent += bit;
            }
            foreach (var bit in number.Mantissa)
            {
                mantissa += bit;
            }
            string sign = number.Sign + new string(' ', signHeader.Length - 1);
            exponent += new string(' ', exponentHeader.Length - exponent.Length);
            string normalBit = number.NormalBit + new string(' ', normalBitHeader.Length - 1); 
            mantissa += new string(' ', mantissaHeader.Length - mantissa.Length);
            string numberStr = number.GetDecimalString() ;
            numberStr += new string(' ', decimalHeader.Length - numberStr.Length);
            Console.WriteLine($"| {sign} | {exponent} | {normalBit} | {mantissa} | {numberStr} |");
            Console.WriteLine(new string('-', headers.Length));
        }

    }

    private string CalculatePadding(string header, int max)
    {
        return max - header.Length > 0 ? new string(' ', max - header.Length) : ""; 
    }
}