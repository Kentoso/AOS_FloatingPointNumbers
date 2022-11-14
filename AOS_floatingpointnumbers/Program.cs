// See https://aka.ms/new-console-template for more information

using AOS_floatingpointnumbers;

int exponentSize = 4;
int mantissaSize = 10;
// FloatingPointNumber number = new FloatingPointNumber(8, 23, "1.41E12");
// number.ShowFormatted();
// number.ShowBitString();
// number.ShowDecimal();
// FloatingPointNumber number2 = new FloatingPointNumber(1, "11111111", "00000000000000000000000", 1);
// number2.ShowDecimal();
var minAbs = FloatingPointNumber.GetMinAbs(exponentSize, mantissaSize);
var max = FloatingPointNumber.GetMax(exponentSize, mantissaSize);
var min = FloatingPointNumber.GetMin(exponentSize, mantissaSize);
var one = FloatingPointNumber.GetOne(exponentSize, mantissaSize);
var plusInf = FloatingPointNumber.GetPlusInf(exponentSize, mantissaSize);
var minusInf = FloatingPointNumber.GetMinusInf(exponentSize, mantissaSize);
var minSubnormal = FloatingPointNumber.GetMinSubnormal(exponentSize, mantissaSize);
var nan = FloatingPointNumber.GetNaN(exponentSize, mantissaSize);
TableDrawer tableDrawer = new TableDrawer(new List<FloatingPointNumber>(){minAbs, max, min, one, plusInf, minusInf, minSubnormal, nan});
tableDrawer.DrawTable();
Console.WriteLine("Input decimal number in format \"(+ | -){N}.{N}\"");
var num = Console.ReadLine();
FloatingPointNumber number = new FloatingPointNumber(exponentSize, mantissaSize, num);
TableDrawer tableDrawer2 = new TableDrawer(new List<FloatingPointNumber>()
    {number});
tableDrawer2.DrawTable();
