using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class GameUtility {
    public static string AddSpacesToCamelCase(string text) {
        // The regex pattern `(?<!^)(?=[A-Z])` matches any uppercase letter that is not at the start of the string
        // `(?<!^)` - Negative lookbehind to check the uppercase letter is not at the start
        // `(?=[A-Z])` - Positive lookahead to find each uppercase letter
        return Regex.Replace(text, "(?<!^)(?=[A-Z])", " ");
    }
    public static Color HexToColor(string hex) {
        // Removing leading '#' if present
        if (hex.StartsWith("#")) {
            hex = hex.Substring(1);
        }

        // Convert hex to a uint (this includes handling transparency if present)
        uint colorInt = uint.Parse(hex, System.Globalization.NumberStyles.HexNumber);

        // Extract color components
        float red = ((colorInt >> 16) & 0xFF) / 255.0f;
        float green = ((colorInt >> 8) & 0xFF) / 255.0f;
        float blue = (colorInt & 0xFF) / 255.0f;
        float alpha = hex.Length >= 8 ? ((colorInt >> 24) & 0xFF) / 255.0f : 1.0f;

        // Create Unity Color object
        return new Color(red, green, blue, alpha);
    }
}
