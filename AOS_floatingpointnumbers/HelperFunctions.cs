namespace AOS_floatingpointnumbers;

public static class HelperFunctions
{
    public static (string result, string carry) MultiplyStringNumberByTwo(string number)
    {
        int carry = 0;
        string result = "";
        for (int i = number.Length - 1; i >= 0; i--)
        {
            int currDigit = Convert.ToInt32(number[i].ToString());
            int twoSum = 2 * currDigit + carry;
            result = twoSum % 10 + result;
            carry = twoSum / 10;
        }

        return (result, carry.ToString());
    }

    public static bool IsStringNumberZero(string num)
    {
        for (int i = 0; i < num.Length; i++)
        {
            if (num[i] != '0') return false;
        }

        return true;
    }

    public static string ConvertFractionToBinary(string fraction, int maxBits = 100)
    {
        string num = fraction;
        string result = "";
        int i = 0;
        if (IsStringNumberZero(num)) return new string('0', maxBits); 
        while (!HelperFunctions.IsStringNumberZero(num) && i < maxBits)
        {
            (string multiplied, string carry) = HelperFunctions.MultiplyStringNumberByTwo(num);
            result += carry;
            num = multiplied;
            i++;
        }

        return result;
    }

    public static string ConvertWholePartToBinary(string wholePart)
    {
        string num = wholePart;
        string result = "";
        if (IsStringNumberZero(num)) return "0";
        List<int> powers = new List<int>();
        string currentPowerOfTwo = "2";
        string prevPowerOfTwo = "1";
        int i = 1;
        while (!IsStringNumberZero(num))
        {
            if (CompareStringNumbers(currentPowerOfTwo, num) == 1)
            {
                powers.Add(i - 1);
                num = SubtractStringNumbers(num, prevPowerOfTwo);
                currentPowerOfTwo = "2";
                prevPowerOfTwo = "1";
                i = 1;
                continue;
            }
            prevPowerOfTwo = currentPowerOfTwo;
            (string newPower, string powerCarry) = MultiplyStringNumberByTwo(currentPowerOfTwo);
            currentPowerOfTwo = (powerCarry == "0" ? "" : "1") + newPower;
            i++;
        }
        for (int j = powers[0]; j >= 0; j--)
        {
            result += powers.Contains(j) ? "1" : "0";
        }
        return result;
    }

    public static int CompareStringNumbers(string num1, string num2)
    {
        if (num1.Length == num2.Length)
        {
            var length = num1.Length;
            for (int i = 0; i < length; i++)
            {
                if (num1[i] != num2[i]) return num1[i] > num2[i] ? 1 : -1;
            }
            return 0;
        }
        return num1.Length > num2.Length ? 1 : -1;
    }

    public static string SubtractStringNumbers(string num1, string num2)
    {
        //num2 is smaller
        int compare = CompareStringNumbers(num1, num2);
        if (compare == -1 || compare == 0) return "0";
        int carry = 0;
        string result = "";
        for (int i = num2.Length - 1; i >= 0; i--)
        {
            int sub = num1[num1.Length - num2.Length + i] - num2[i] - carry;
            if (sub < 0) carry = 1;
            else carry = 0;
            sub = sub < 0 ? sub + 10 : sub;
            result = sub + result;
        }
        if (num1.Length != num2.Length) result = num1[num1.Length - num2.Length - 1] - '0' - carry + result;
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i] != '0')
            {
                result = result.Substring(i);
                break;
            }
        }
        return result;
    }
}