using System;

namespace RconLib
{
    public class RconMessage
    {
        public const int SizeWithoutBody = 24;

        public int Size { get; set; }
        public int ID { get; set; }
        public int Type { get; set; }
        public string Body { get; set; }

        public byte[] GetFormattedPacket()
        {
            // + 1 for new line at the end
            int size = SizeWithoutBody + Body.Length + 1;
            byte[] message = new byte[size];

            return message;
        }
    }
}
