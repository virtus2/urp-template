using UnityEngine;

public static class StringUtility
{
    public static string ToColoredString(this string str, Color color)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{str}</color>";
    }
}
