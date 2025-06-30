using System.Collections;
using System.Collections.Generic;
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
    }
}
