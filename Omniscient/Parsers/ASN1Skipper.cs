/*
This source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

This source code is distributed under the New BSD license:

================================================================================

   Copyright (c) 2020, International Atomic Energy Agency (IAEA), IAEA.org

   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of IAEA nor the names of its contributors
      may be used to endorse or promote products derived from this software
      without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS"AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
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
