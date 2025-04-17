using SNR_ClientApp.TallyResponses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SNR_ClientApp.Utils
{
    internal class StringUtilsCustom
    {
        public static String TALLY_COMPANY;

        public static String replaceSpecialCharacters(String value)
        {
            value = value.Replace("amp;", "&");
            value = value.Replace("apos;", "'");
            value = value.Replace("quot;", "\"");
            return value;
        }

        public static String replaceSpecialCharactersWithXmlValue(String value)
        {
            // Todo : need to check this commented code  , if exception occures while executing xml request dute to & uncomment these lines .
            //value = value.Replace("&", "&amp;");
            value = value.Replace("'", "&apos;");
            //value = value.Replace("\"", "&quot;");
            return value;
        }

        public static string ConvertToDateTime(string dATE)
        {
            try
            {
                if (dATE == null)
                    return null;
                return DateTime.ParseExact(dATE, "yyyymmdd", CultureInfo.InvariantCulture).ToString();
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex, "Exception  while converting string to date (" + dATE + ") to Date");
                return null;
            }
        }

        public static string ConvertStringDateToDD_MM_YYYY(string inputDate)
        {
            try
            {
                if (inputDate == null)
                    return null;


                DateTime parsedDate = DateTime.Parse(inputDate); // Convert string to DateTime object

                string newdate = parsedDate.ToString("dd-MM-yyyy"); // Format date to dd-mm-yyyy

                Console.WriteLine(newdate);
                return newdate;
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex, "Exception  while converting string  inputDate (" + inputDate + ") to Date of format  dd-mm-yyyy");
                return null;
            }
        }

        public static string convertDateToString(string inputDate) {
            try
            {
                if (inputDate == null)
                    return null;


                DateTime parsedDate = DateTime.Parse(inputDate); // Convert string to DateTime object

                string yyyymmdd = parsedDate.ToString("yyyyMMdd"); // Format date to yyyymmdd

                Console.WriteLine(yyyymmdd);
                return yyyymmdd;
            }catch(Exception ex)
            {
                LogManager.HandleException(ex, "Exception  while converting string  inputDate (" + inputDate + ") to Date of format yyyymmdd");
                return null;
            }
        }

        public static int getNumberFromString(string a)
        {
            try
            {
                if (String.IsNullOrEmpty(a))
                    return 0;
                string pattern = @"-?\d+(\.\d+)?";
                string numericString = Regex.Match(a, pattern).Value;
                //string numericString = Regex.Match(a, @"\d+\.\d+").Value;
               // string numericString = Regex.Replace(a, "[^0-9-]", "");

                // Convert the remaining string to an integer
                int number = (int)double.Parse(numericString);
                //int number = int.Parse(numericString);

                return number;
                //string b = string.Empty;
                //int val=0;

                //for (int i = 0; i < a.Length; i++)
                //{
                //    if (Char.IsDigit(a[i]))
                //        b += a[i];
                //}

                //if (b.Length > 0)
                //    val = int.Parse(b);

                //return val;
            }catch(Exception ex) {
                LogManager.WriteLog("exception occured whilw converting string to int ");
                return 0;
            
            }
        }

        public static double getDoubleFromString(string a)
        {
            if (a == null)
                return 0;
            string b = string.Empty;
            double val = 0;

            for (int i = 0; i < a.Length; i++)
            {
                if (Char.IsDigit(a[i]))
                    b += a[i];
            }

            if (b.Length > 0)
                val = StringUtilsCustom.ExtractDoubleValue(b);

            return val;
        }

        public static string FormatDate(string date)
        {
            string formattedDate = "";
            string[] splitDates = date.Split('T');
            DateTime dateTime = DateTime.ParseExact(splitDates[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            formattedDate = dateTime.ToString("dd-MM-yyyy");
            return formattedDate;
        }

        internal static DateTime ConvertFormat(string inputDate)
        {
            try
            {

                if (DateTime.TryParseExact(inputDate, "d-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    return date;
                }

                // Try parsing with two-digit year format
                if (DateTime.TryParseExact(inputDate, "d-MMM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date2))
                {
                    return date2;
                }

                return DateTime.Parse(inputDate);
                //DateTime date3 = DateTime.ParseExact(inputDate, "d-MMM-yy", System.Globalization.CultureInfo.InvariantCulture);
                ////string outputDate = date.ToString("dd-MM-yyyy");
                ////return outputDate;
                //return date3;
            }catch(Exception ex)
            {
                LogManager.WriteLog($"Error while Converting Date {inputDate} to DateTime in StringUtils Custom .ConvertFormat() ");
                LogManager.HandleException(ex);
                return DateTime.Parse(inputDate);
            }
        }

        public static double ExtractDoubleValue(string input)
        {
            if (string.IsNullOrEmpty(input))
                return 0;
            try
            {
                // Remove any non-digit or non-decimal separator characters from the string
                string pattern = @"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?";

                // Find the first occurrence of a number in the string
                Match match = Regex.Match(input, pattern);

                if (match.Success)
                {
                    // Parse the matched number into a double value
                    return double.Parse(match.Value);

                   
                }
                else
                {
                    return 0;
                }
               
                // Parse the cleaned string into a double value
              
            }catch(Exception ex)
            {
                LogManager.HandleException(ex);
                return 0;
            }
        }
    }
}
