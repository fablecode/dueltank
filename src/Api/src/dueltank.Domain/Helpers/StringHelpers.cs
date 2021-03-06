﻿using System.IO;

namespace dueltank.Domain.Helpers
{
    public static class StringHelpers
    {
        public static string SanitizeFileName(this string name)
        {
            return string.Concat(name.Split(Path.GetInvalidFileNameChars()));
        }
    }
}