using System.Globalization;
using System.IO.Pipes;

namespace AOS_floatingpointnumbers;

public class FloatingPointNumber
{
    public int Sign;
    public int[] Exponent;
    public int[] Mantissa;
    private string _decimal;
    public int NormalBit;
    private int _exponentBias;

    public static FloatingPointNumber GetMinAbs(int exponentSize, int mantissaSize)
    {
        return new FloatingPointNumber(0, new string('0', exponentSize - 1) + "1", new string('0', mantissaSize), 1);
    }
    public static FloatingPointNumber GetMax(int exponentSize, int mantissaSize)
    {
        return new FloatingPointNumber(0, new string('1', exponentSize - 1) + "0", new string('1', mantissaSize), 1);
    }
    public static FloatingPointNumber GetMin(int exponentSize, int mantissaSize)
    {
        return new FloatingPointNumber(1, new string('1', exponentSize - 1) + "0", new string('1', mantissaSize), 1);
    }
    public static FloatingPointNumber GetOne(int exponentSize, int mantissaSize)
    {
        return new FloatingPointNumber(0, "0" + new string('1', exponentSize - 1) , new string('0', mantissaSize), 1);
    }
    public static FloatingPointNumber GetPlusInf(int exponentSize, int mantissaSize)
    {
        return new FloatingPointNumber(0, new string('1', exponentSize), new string('0', mantissaSize), 1);
    }
    public static FloatingPointNumber GetMinusInf(int exponentSize, int mantissaSize)
    {
        return new FloatingPointNumber(1, new string('1', exponentSize), new string('0', mantissaSize), 1);
    }
    public static FloatingPointNumber GetMinSubnormal(int exponentSize, int mantissaSize)
    {
        return new FloatingPointNumber(0, new string('0', exponentSize), new string('0', mantissaSize - 1) + "1", 0);
    }
    public static FloatingPointNumber GetNaN(int exponentSize, int mantissaSize)
    {
        return new FloatingPointNumber(0, new string('1', exponentSize), new string('0', mantissaSize - 1) + "1", 1);
    }
    private FloatingPointNumber(int exponentSize, int mantissaSize)
    {
        Exponent = new int[exponentSize];
        Mantissa = new int[mantissaSize];
        _exponentBias = (int)Math.Pow(2, exponentSize - 1) - 1;
        _decimal = "";
    }

    public (int exponent, int mantissa) GetLengths()
    {
        return (Exponent.Length, Mantissa.Length);
    }

    public FloatingPointNumber(int sign, string exponent, string mantissa, int normalBit) : this(exponent.Length,
        mantissa.Length)
    {
        SetAll(sign, exponent, mantissa, normalBit);
    }
    public FloatingPointNumber(int exponentSize, int mantissaSize, string decimalNum) : this(exponentSize, mantissaSize)
    {
        ConvertDecimalToFloat(decimalNum);
    }

    public void ShowFormatted()
    {
        Console.Write(Sign);
        Console.Write(" ");
        for (int i = 0; i < Exponent.Length; i++)
        {
            Console.Write(Exponent[i]);
        }
        Console.Write(" ");
        Console.Write(NormalBit);
        Console.Write(" ");
        for (int i = 0; i < Mantissa.Length; i++)
        {
            Console.Write(Mantissa[i]);
        }
        Console.Write('\n');
    }

    public void ShowBitString()
    {
        Console.Write(Sign);
        for (int i = 0; i < Exponent.Length; i++)
        {
            Console.Write(Exponent[i]);
        }
        for (int i = 0; i < Mantissa.Length; i++)
        {
            Console.Write(Mantissa[i]);
        }
        Console.Write('\n');
    }

    public void ShowDecimal()
    {
        double num = ToDouble();
        if (double.IsPositiveInfinity(num))
        {
            Console.Write("+inf");
        }
        else if (double.IsNegativeInfinity(num))
        {
            Console.Write("-inf");
        }
        else if (double.IsNaN(num))
        {
            Console.Write("NaN");
        }
        else
        {
            Console.Write(num);
        }
    }

    public string GetDecimalString()
    {
        double num = ToDouble();
        if (double.IsPositiveInfinity(num))
        {
            return "+inf";
        }
        else if (double.IsNegativeInfinity(num))
        {
            return "-inf";
        }
        else if (double.IsNaN(num))
        {
            return "NaN";
        }
        else
        {
            return Convert.ToString(num, CultureInfo.InvariantCulture);
        }
    }
    public double ToDouble()
    {
        double result = 0.0;
        int exponentSum = 0;
        for (int i = 0; i < Exponent.Length; i++)
        {
            exponentSum += Exponent[i];
        }
        if (exponentSum == 0)
        {
            //subnormal
            double fraction = 0.0;
            for (int i = 0; i < Mantissa.Length; i++)
            {
                fraction += Math.Pow(2, -(i + 1)) * Mantissa[i];
            }
            return Math.Pow(-1, Sign) * Math.Pow(2, -(_exponentBias - 1)) * fraction;
        }
        else if (exponentSum == Exponent.Length)
        {
            for (int i = 0; i < Mantissa.Length; i++)
            {
                if (Mantissa[i] != 0) return double.NaN;
            }
            return Sign == 0 ? double.PositiveInfinity : double.NegativeInfinity;
        }
        else
        {
            //normal
            string exponentStr = "";
            for (int i = 0; i < Exponent.Length; i++)
            {
                exponentStr += Exponent[i];
            }
            int exponentInt = Convert.ToInt32(exponentStr, 2);
            double fraction = 1.0;
            for (int i = 0; i < Mantissa.Length; i++)
            {
                fraction += Math.Pow(2, -(i + 1)) * Mantissa[i];
            }
            result = Math.Pow(-1, Sign) * Math.Pow(2, exponentInt - _exponentBias) * fraction;
        }
        return result;
    }
    private void ConvertDecimalToFloat(string dNumber)
    {
        _decimal = dNumber;
        (string wholePart, string fraction) = ParseDecimal();
        Sign = wholePart[0] == '-' ? 1 : 0;
        if (wholePart[0] == '-' || wholePart[0] == '+') wholePart = wholePart.Remove(0, 1);
        // int wholePartInt = Convert.ToInt32(wholePart);
        // string binaryWholePart = Convert.ToString(wholePartInt, 2);
        string binaryWholePart = HelperFunctions.ConvertWholePartToBinary(wholePart);
        string binaryFraction = HelperFunctions.ConvertFractionToBinary(fraction, Mantissa.Length);
        int indexOfOneWholePart = binaryWholePart.IndexOf('1');
        int normalBit;
        int exponent;
        if (indexOfOneWholePart != -1)
        {
            //normal
            normalBit = 1;
            int numberOfBitsAddedToFraction = binaryWholePart.Length - 1 - indexOfOneWholePart;
            exponent = numberOfBitsAddedToFraction;
            var firstFr = binaryWholePart.Substring(indexOfOneWholePart + 1);
            var secondFr = "";
            if (numberOfBitsAddedToFraction < Mantissa.Length)
            {
                secondFr = binaryFraction.Substring(0, binaryFraction.Length - numberOfBitsAddedToFraction);
            }
            string newFraction = firstFr + secondFr;
            SetAll(Sign, ConvertExponentToBinary(exponent), newFraction, normalBit);
            // Console.WriteLine($"{_sign} {ConvertExponentToBinary(exponent)} {normalBit} {newFraction}");
        }
        else
        {
            string newBinaryFraction =
                HelperFunctions.ConvertFractionToBinary(fraction, Mantissa.Length + (int) Math.Pow(2, Exponent.Length - 1) - 2);
            int indexOfOneFraction = newBinaryFraction.IndexOf('1');
            if (indexOfOneFraction + 1 > (int) Math.Pow(2, Exponent.Length - 1) - 2)
            {
                //subnormal
                normalBit = 0;
                exponent = -((int)Math.Pow(2, Exponent.Length - 1) - 1);
                SetAll(Sign, ConvertExponentToBinary(exponent), newBinaryFraction.Substring((int) Math.Pow(2, Exponent.Length - 1) - 2), normalBit);
            }
            else if (indexOfOneFraction == -1)
            {
                //zero
                normalBit = 0;
                exponent = -((int) Math.Pow(2, Exponent.Length - 1) - 1);
                SetAll(Sign, ConvertExponentToBinary(exponent), new string('0', Mantissa.Length), normalBit);
            }
            else
            {
                //normal, smaller than 1
                normalBit = 1;
                exponent = -(indexOfOneFraction + 1);
                string mantissa = "";
                // if ((int) Math.Pow(2, _exponent.Length - 1) - 2 - indexOfOneFraction - 1 >= _mantissa.Length)
                // {
                //     mantissa = newBinaryFraction.Substring(indexOfOneFraction + 1, _mantissa.Length);
                // }
                // else
                // {
                //     mantissa = newBinaryFraction.Substring(indexOfOneFraction + 1 - _mantissa.Length, _mantissa.Length);
                // }
                mantissa = newBinaryFraction.Substring(indexOfOneFraction + 1, Mantissa.Length);
                SetAll(Sign, ConvertExponentToBinary(exponent), mantissa, normalBit);
            }
        }
        
    }

    private void SetAll(int sign, string exponent, string mantissa, int normalBit)
    {
        Sign = sign;
        NormalBit = normalBit;
        for (int i = 0; i < Exponent.Length; i++)
        {
            Exponent[i] = Convert.ToInt32(exponent[i].ToString());
        }

        for (int i = 0; i < Mantissa.Length; i++)
        {
            Mantissa[i] = Convert.ToInt32(mantissa[i].ToString());
        }
    }
    private string ConvertExponentToBinary(int exponent)
    {
        string result = "";
        int biasedExponent = exponent + _exponentBias;
        for (int i = 0; i < Exponent.Length; i++)
        {
            result = biasedExponent % 2 + result;
            biasedExponent /= 2;
        }
        return result;
    }
    private (string, string) ParseDecimal()
    {
        if (_decimal == "") return ("", "");
        int pointIndex = _decimal.IndexOf('.');
        int eIndex = _decimal.IndexOf('E');
        var exp = _decimal.Substring(eIndex + 1);
        var expInt = Convert.ToInt32(exp);
        var wholePart = _decimal.Substring(0, pointIndex);
        var fraction = _decimal.Substring(pointIndex + 1, eIndex - pointIndex - 1);
        if (expInt > 0)
        {
            int toRemove = expInt <= fraction.Length ? expInt : fraction.Length;
            wholePart += fraction.Substring(0, toRemove);
            for (int i = 0; i < expInt - fraction.Length; i++) wholePart += "0";
            fraction = fraction.Remove(0, toRemove);
            fraction = fraction == "" ? "0" : fraction;
        }
        else if (expInt < 0)
        {
            expInt = Math.Abs(expInt);
            int toRemove = expInt <= wholePart.Length ? expInt : wholePart.Length;
            fraction = wholePart.Substring(wholePart.Length - toRemove) + fraction;
            for (int i = 0; i < expInt - wholePart.Length; i++) fraction = "0" + fraction;
            wholePart = wholePart.Remove(wholePart.Length - toRemove);
            wholePart = wholePart == "" ? "0" : wholePart;
        }
        return (wholePart, fraction);
    }
}