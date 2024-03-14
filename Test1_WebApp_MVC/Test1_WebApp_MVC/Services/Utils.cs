using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Test1_WebApp_MVC.Models;

namespace Test1_WebApp_MVC.Services
{
    public static class Utils
    {
        #region REGEX UTILS

        //the regex pattern is fairly broad to account for phone number formats from any country. A future extension could require inputting the user's country and matching the specific pattern for it.
        //TODO: put regex in appsettings; even a flag for ignoring whitespace
        public static string PHONE_REGEX = @"^*";
        //TODO: REGEX ISN'T RIGHT, FIX IT.
        //public static string PHONE_REGEX = @"^\\\\+?[1-9][0-9]{7,14}$";

        public static string VALID_PHONE_EXAMPLE = "eg. 082 111 2222, +27 82 000 9999";

        public static bool MatchesRegex(string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }

        public static bool MatchesPhoneRegex(string input)
        {
            try
            {
                var pattern = new Regex(PHONE_REGEX, RegexOptions.IgnorePatternWhitespace);
                return pattern.IsMatch(input);
            }
            catch (Exception ex)
            { 
                return false; 
            }
        }

        #endregion REGEX UTILS


        #region EXTENSION UTILS

        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return (list?.Count ?? 0) == 0;
        }

        public static List<T> NewOrCurrent<T>(this List<T> list)
        {
            if (list == null)
                return new List<T>();

            return list;
        }

        public static string Flatten(this List<string> stringList, string joinString)
        {
            if ((stringList?.Count ?? 0) == 0)
                return string.Empty;

            var sb = new StringBuilder();
            stringList.ForEach(s => sb.Append(s).Append(joinString));

            return sb.ToString();
        }
        
        #endregion EXTENSION UTILS
        
        
        #region VIEWDATA UTILS

        public static void Upsert<T>(this ViewDataDictionary viewData, string key, T value) 
        { 
            if (viewData?.ContainsKey(key) ?? false)
                viewData[key] = value;
            else
                viewData?.Add(key, value);
        }

        public static void NullSafeRemove(this ViewDataDictionary viewData, string key)
        {
            if (viewData?.ContainsKey(key) ?? false)
                viewData.Remove(key);
        }

        public static void Reset(this ViewDataDictionary viewData, string activeButtonId = null)
        {
            //TODO: variabelize these string identifiers

            if (!string.IsNullOrEmpty(activeButtonId))
                viewData.Upsert("activeBtn", activeButtonId);

            viewData.NullSafeRemove("userMsg");
            viewData.NullSafeRemove("userSuccess");
        }

        public static void SetState(this ViewDataDictionary viewData, bool success, string message, string activeButtonId = null)
        {
            if (!string.IsNullOrEmpty(activeButtonId))
                viewData.Upsert("activeBtn", activeButtonId);

            viewData.Upsert("userMsg", message);
            viewData.Upsert("userSuccess", success);
        }

        #endregion VIEWDATA UTILS
    }
}
