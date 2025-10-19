namespace PasteCoords;

public static class CoordConverter
{
    static bool IsMgrs(string input)
    {
        foreach (var c in input)
            if (char.IsLetter(c))
                return true;
        return false;
    }

    static string GetTruncatedMgrs(string input, int maxDigits)
    {
        var lastCharIndex = 0;
        for (var i = 0; i < input.Length; i++)
            if (char.IsLetter(input[i]))
                lastCharIndex = i;
        
        var firstDigitIndex = lastCharIndex + 1;
        if (firstDigitIndex >= input.Length)
        {
            Console.WriteLine("Warning: no northing/easting");
            return input;
        }

        var digitCount = input.Length - firstDigitIndex;
        if (digitCount % 2 != 0)
        {
            Console.WriteLine("Error: unbalanced northing/easting lengths");
            return input;
        }
        var numLength = digitCount / 2;

        // No need to truncate unless input exceeds max requested digits
        if (numLength <= maxDigits)
            return input;

        var truncatedNorthing = input.Substring(firstDigitIndex, maxDigits);
        var truncatedEasting = input.Substring(firstDigitIndex + numLength, maxDigits);
        
        var output = input[..firstDigitIndex];
        output += truncatedNorthing;
        output += truncatedEasting;

        return output;
    }

    static string GetFirstLine(string input)
    {
        var lines = input.Split('\r', '\n');
        foreach (var line in lines)
            if (!line.StartsWith('\r') &&
                !line.StartsWith('\n'))
                return line;
        return input;
    }

    static string GetFilteredInput(string input)
    {
        var i = input.LastIndexOf(':');
        if (i >= 0)
            input = input.Substring(i + 1);

        input = input.Trim();
        input = input.Replace(" ", "");
        input = GetFirstLine(input);

        return input;
    }

    public static string GetApacheCoords(string input)
    {
        input = GetFilteredInput(input);

        Console.WriteLine($" INPUT: {input}");

        if (IsMgrs(input))
            input = GetTruncatedMgrs(input, 4);

        Console.WriteLine($"OUTPUT: {input}\n");

        return input;
    }
}
