using System;
using System.Text;

public static class StringBuilderExtension
{
    public static void AppendWithTime(this StringBuilder source, string message)
    {
        source.Append(message);
        source.Append("  ");
        source.Append(DateTime.Now);
        source.Append("\r\n");
    }
}


