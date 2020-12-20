using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Substratum.IO
{
    public class ReplaceCharacterSet
    {
        public char SearchChar;
        public char ReplaceChar;
    }

    public class CharacterTransformStream : ProcessingStream
    {
        char[] removeChar;
        ReplaceCharacterSet[] replaceCharSet;

        public CharacterTransformStream(Stream stream, char[] removeChar, ReplaceCharacterSet[] replaceCharSet)
            : base(stream)
        {
            this.removeChar = removeChar;
            this.replaceCharSet = replaceCharSet;
        }

        protected override void ProcessBuffer(ProcessingDirection Direction, byte[] buffer, int offset, int count, 
            out byte[] outBuffer, out int outCount)
        {
            outBuffer = new byte[count];
            outCount = 0;
            for (int i = offset; i < offset + count; i++)
            {
                bool include = true;
                // remove characters?
                if (removeChar!=null)
                {
                    for (int j = 0; j < replaceCharSet.Length; j++)
                    {
                        if (buffer[i] == removeChar[j])
                        {
                            include = false;
                            break;
                        }
                    }
                }
                if (include)
                {
                    // replace characters?
                    if (replaceCharSet != null)
                    {
                        for (int j = 0; j < replaceCharSet.Length; j++)
                        {
                            if (buffer[i] == replaceCharSet[j].SearchChar)
                                outBuffer[outCount] = (byte)replaceCharSet[j].ReplaceChar;
                            else
                                outBuffer[outCount] = buffer[i];
                        }
                    }
                    else
                        outBuffer[outCount] = buffer[i];
                    outCount++;
                }
            }
        }
    }
}
