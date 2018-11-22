using System;
using System.Security.Cryptography;
using System.Text;

public class Md5Util {
    public static string Encode (string str) {
        using (MD5 md5 = MD5.Create ()) {
            byte[] by = md5.ComputeHash (Encoding.UTF8.GetBytes (str));
            StringBuilder sb = new StringBuilder ();
            for (int i = 0; i < by.Length; i++) {
                sb.Append (by[i].ToString ("x2"));
            }
            return sb.ToString ();
        }
    }
}