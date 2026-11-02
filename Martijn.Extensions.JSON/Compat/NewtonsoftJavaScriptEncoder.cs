using System.Text.Encodings.Web;

namespace Martijn.Extensions.JSON.Compat
{
    /// <summary>
    /// Escapes exactly the characters Newtonsoft escapes with StringEscapeHandling.Default:
    /// control characters, the quote, the backslash, U+0085, U+2028 and U+2029.
    /// Everything else (HTML characters, non-ASCII text, emoji) is written as is.
    /// </summary>
    internal sealed class NewtonsoftJavaScriptEncoder : JavaScriptEncoder
    {
        public static readonly NewtonsoftJavaScriptEncoder Instance = new();

        public override int MaxOutputCharactersPerInputCharacter => 12;

        public override bool WillEncode(int unicodeScalar) =>
            unicodeScalar < 0x20 || unicodeScalar == '"' || unicodeScalar == '\\' ||
            unicodeScalar == 0x85 || unicodeScalar == 0x2028 || unicodeScalar == 0x2029;

        public override unsafe int FindFirstCharacterToEncode(char* text, int textLength)
        {
            for (int i = 0; i < textLength; i++)
            {
                if (WillEncode(text[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public override unsafe bool TryEncodeUnicodeScalar(int unicodeScalar, char* buffer, int bufferLength, out int numberOfCharactersWritten)
        {
            string encoded;
            if (!WillEncode(unicodeScalar))
            {
                encoded = char.ConvertFromUtf32(unicodeScalar);
            }
            else
            {
                encoded = unicodeScalar switch
                {
                    '\b' => "\\b",
                    '\t' => "\\t",
                    '\n' => "\\n",
                    '\f' => "\\f",
                    '\r' => "\\r",
                    '"' => "\\\"",
                    '\\' => "\\\\",
                    _ => "\\u" + unicodeScalar.ToString("x4")
                };
            }

            if (encoded.Length > bufferLength)
            {
                numberOfCharactersWritten = 0;
                return false;
            }

            for (int i = 0; i < encoded.Length; i++)
            {
                buffer[i] = encoded[i];
            }

            numberOfCharactersWritten = encoded.Length;
            return true;
        }
    }
}
