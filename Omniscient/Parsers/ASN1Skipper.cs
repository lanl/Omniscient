using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class ASN1Element
    {
        public int Start { get; private set; }
        public byte Tag { get; set; }
        public int Length { get; set; }
        public int DataStart { get; set; }

        private byte[] bytes;

        public ASN1Element(byte[] bytes, int start)
        {
            this.bytes = bytes;
            Start = start;

            Tag = bytes[start];

            if (bytes[start + 1] < 0x80)
            {
                Length = bytes[start + 1];
                DataStart = start + 2;
            }
            else
            {
                int nLengthBytes = bytes[start + 1] - 0x80;
                byte[] lenBytes = new byte[4];

                Array.Copy(bytes, start + 2, lenBytes, 4 - nLengthBytes, nLengthBytes);
                Length = (lenBytes[0] << 24) | (lenBytes[1] << 16) | (lenBytes[2] << 8) | lenBytes[3];
                DataStart = start + 2 + nLengthBytes;
            }
        }
    }

    public class ASN1Skipper
    {
        private byte[] bytes;

        public ASN1Skipper(byte[] bytes)
        {
            this.bytes = bytes;
        }

        public ASN1Element FindContentElement(byte[] startPattern, int maxIterations=64)
        {
            const int MAX_CONTENT_SIZE = 80;
            ASN1Element lastElement;
            ASN1Element thisElement = new ASN1Element(bytes, 0);
            for (int i = 0; i < maxIterations; i++)
            {
                lastElement = thisElement;
                if (lastElement.Length > MAX_CONTENT_SIZE)
                {
                    thisElement = new ASN1Element(bytes, lastElement.DataStart);
                }
                else
                {
                    thisElement = new ASN1Element(bytes, lastElement.DataStart + lastElement.Length);
                }

                if (thisElement.Length >= startPattern.Length)
                {
                    bool patternMatches = true;
                    for (int pIndex=0; pIndex<startPattern.Length; pIndex++)
                    {
                        if (bytes[thisElement.DataStart+pIndex] != startPattern[pIndex])
                        {
                            patternMatches = false;
                            break;
                        }
                    }
                    if (patternMatches) return thisElement;
                }
            }

            return null;
        }
    }
}
