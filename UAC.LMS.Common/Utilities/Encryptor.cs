using Microsoft.VisualBasic;
using System;
using System.Security.Cryptography;
using System.Text;

namespace UAC.LMS.Common.Utilities
{
    /// <summary>
    /// Utilities for password Encryption
    /// </summary>
    public class Encryptor
    {
        public static byte[] GenerateHash(string value, string salt)
        {
            byte[] data = Encoding.ASCII.GetBytes(salt + value);
            data = SHA1Managed.Create().ComputeHash(data);
            return data;
        }
        public static byte[] EncryptText(string key, string name)
        {
            byte[] functionReturnValue = null;
            try
            {
                int intPwdCount = 0;
                int intUserAscii = 0;
                string strBuff = "";
                name = name.Trim().ToUpper();
                if (Strings.Len(Strings.Trim(name)) > 0)
                {
                    for (intPwdCount = 1; intPwdCount <= key.Length; intPwdCount++)
                    {
                        intUserAscii = Strings.Asc(Strings.Mid(key, intPwdCount, 1));
                        intUserAscii = intUserAscii + Strings.Asc(Strings.Mid(Strings.Trim(name), (intPwdCount % Strings.Len(Strings.Trim(name))) + 1, 1));
                        strBuff = strBuff + Strings.Chr(intUserAscii & 0xff);
                    }
                    functionReturnValue = System.Text.Encoding.ASCII.GetBytes(Strings.Trim(strBuff));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return functionReturnValue;
        }
        public static bool CompareByteArray(byte[] source, byte[] target)
        {
            if (source == null || target == null || source.LongLength != target.LongLength)
            {
                return false;
            }
            for (long i = 0; i < source.LongLength; i++)
            {
                if (source[i] != target[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
