using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;


namespace ProjectVS.Utils.CsvParseUtils
{
    public class CsvParseUtils : MonoBehaviour
    {
        public static string TryParseString(string raw)
        {
            return string.IsNullOrEmpty(raw) ? "N/A" : raw;
        }

        public static int TryParseInt(string raw)
        {
            return int.TryParse(raw, out int result) ? result : -1;
        }

        public static float TryParseFloat(string raw)
        {
            return float.TryParse(raw, out float result) ? result : -1f;
        }

        public static int[] TryParseIntArray(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new int[0];

            string[] parts = value.Split(',');
            List<int> result = new();

            foreach (string part in parts)
            {
                if (int.TryParse(part.Trim(), out int num))
                    result.Add(num);
            }

            return result.ToArray();
        }

        public static float[] TryParseFloatArray(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new float[0];

            string[] parts = value.Split(',');
            List<float> result = new();

            foreach (string part in parts)
            {
                if (float.TryParse(part.Trim(), out float num))
                    result.Add(num);
            }

            return result.ToArray();
        }

        public static string ReplaceSetValuePlaceholders(string input, float[] setValues)
        {
            return Regex.Replace(input, @"\(?세트 아이템 수치 ?(\d+)\)?", match =>
            {
                int index = int.Parse(match.Groups[1].Value) - 1;
                if (index >= 0 && index < setValues.Length)
                    return $"({setValues[index]})"; // 괄호 포함해서 반환
                else
                    return "(N/A)";
            });
        }
    }
}
