using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

/// <summary>
/// Summary description for Utils
/// </summary>
public static class Utils
{ 
    public static void print(String str)
    {
        System.Diagnostics.Debug.WriteLine(str);
    }

    public static bool validate_email(string email)
    {
        try
        {
            var address = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}