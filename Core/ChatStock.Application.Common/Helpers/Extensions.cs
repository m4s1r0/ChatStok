using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChatStock.Common.Helpers
{
    public static class Extensions
    {
        public static string ApplyPhoneNumberMask(this string value)
        {
            if (value != null)
            {
                if (value.Length == 11)
                {
                    var a = string.Concat(value[0], value[1]);
                    var b = string.Concat(value[2], value[3], value[4], value[5], value[6]);
                    var c = string.Concat(value[7], value[8], value[9], value[10]);

                    return $"({a}) {b}-{c}";
                }
                else if (value.Length == 10)
                {
                    var a = string.Concat(value[0], value[1]);
                    var b = string.Concat(value[2], value[3], value[4], value[5]);
                    var c = string.Concat(value[6], value[7], value[8], value[9]);

                    return $"({a}) {b}-{c}";
                }
                else
                {
                    return value;
                }
            }
            return "";
        }

        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attributes = field?.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes?.Length > 0 ? ((DescriptionAttribute)attributes[0]).Description : value.ToString() ?? value.ToString();
        }

        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);
        }

        public static string ToJson(this object obj, bool igonoreNull = false)
        {
            if (igonoreNull)
                return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc });

            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc });
        }

        public static string ToIndentedJson(this object obj, bool igonoreNull = false)
        {
            if (igonoreNull)
                return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc });

            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Utc });
        }

        public static T FromJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
        }

        public static T FromJson<T>(this string value, JsonSerializerSettings customSettings)
        {
            return JsonConvert.DeserializeObject<T>(value, customSettings);
        }

        public static bool TryParseJson<T>(this string value, out T result)
        {
            try
            {
                result = JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
                return true;
            }
            catch
            {
                result = default(T);
                return false;
            }
        }

        public static Guid ToGuid(this string value)
        {
            return new Guid(value);
        }

        public static string UnMask(this string value)
        {
            return value.HasValue() ? value.Replace("-", "").Replace(".", "").Replace("/", "").Replace("(", "").Replace(")", "").Replace(" ", "") : value;
        }

        public static DateTime ToDate(this string dateString)
        {
            string[] formats = new string[] { "dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd" };
            CultureInfo.CurrentUICulture = new CultureInfo("pt-BR", false);
            CultureInfo provider = CultureInfo.CurrentCulture;

            var result = DateTime.ParseExact(dateString.Substring(0, 10), formats, provider);

            return result;
        }

        public static DateTime? ToNullableDate(this string dateString)
        {
            CultureInfo.CurrentUICulture = new CultureInfo("pt-BR", false);
            CultureInfo provider = CultureInfo.CurrentCulture;

            if (!DateTime.TryParse(dateString, out _))
            {
                return null;
            }

            string[] formats = new string[] { "dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd" };
            var result = DateTime.ParseExact(dateString.Substring(0, 10), formats, provider);

            return result;
        }

        public static string ToFormattedDateString(this string dateTime, string format = "dd/MM/yyyy")
        {
            CultureInfo.CurrentUICulture = new CultureInfo("pt-BR", false);
            CultureInfo provider = CultureInfo.CurrentCulture;

            DateTime dateVal;

            if (DateTime.TryParse(dateTime, out dateVal))
            {
                var date = dateTime.ToDate();

                return date.ToString(format);
            }

            return string.Empty;
        }

        public static decimal FromFormattedDecimalString(this string decimalValue)
        {
            CultureInfo.CurrentUICulture = new CultureInfo("pt-BR", false);
            CultureInfo provider = CultureInfo.CurrentCulture;

            decimal returnVal;

            if (Decimal.TryParse(decimalValue, out returnVal))
                return returnVal;

            return 0M;
        }

        public static string ToDecimalString(this decimal decimalValue)
        {
            CultureInfo.CurrentUICulture = new CultureInfo("pt-BR", false);
            CultureInfo provider = CultureInfo.CurrentCulture;

            return decimalValue.ToString(provider);
        }

        public static string ToFormattedDateString(this DateTime dateTime, string format = "dd/MM/yyyy")
        {
            return dateTime.ToString(format);
        }

        public static int YearsBeetween(this DateTime startDate, DateTime endDate)
        {
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan span = endDate - startDate;
            int years = (zeroTime + span).Year - 1;

            return years;
        }

        public static DateTime ToDateTime(this long unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }

        public static bool IsValidIndividualRegistrationCharacters(this string individualRegistration)
        {
            if (individualRegistration == null)
                return false;

            individualRegistration = individualRegistration.Trim();
            individualRegistration = individualRegistration.Replace(".", "").Replace("-", "");

            return long.TryParse(individualRegistration, out long valido);
        }

        public static bool IsValidIndividualRegistration(this string individualRegistration)
        {
            if (individualRegistration == null)
                return false;

            int[] m1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] m2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string temp;
            string digit;
            int sum;
            int remainder;

            individualRegistration = individualRegistration.Trim();
            individualRegistration = individualRegistration.Replace(".", "").Replace("-", "");

            if (individualRegistration.Length != 11)
                return false;

            switch (individualRegistration)
            {
                case "00000000000":
                    return false;
                case "11111111111":
                    return false;
                case "22222222222":
                    return false;
                case "33333333333":
                    return false;
                case "44444444444":
                    return false;
                case "55555555555":
                    return false;
                case "66666666666":
                    return false;
                case "77777777777":
                    return false;
                case "88888888888":
                    return false;
                case "99999999999":
                    return false;
                case "12345678909":
                    return false;
            }

            temp = individualRegistration.Substring(0, 9);
            sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(temp[i].ToString()) * m1[i];

            remainder = sum % 11;
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;

            digit = remainder.ToString();

            temp = temp + digit;

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(temp[i].ToString()) * m2[i];

            remainder = sum % 11;
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;

            digit = digit + remainder.ToString();

            return individualRegistration.EndsWith(digit);
        }

        public static bool IsValidDocumentNumberCharacters(this string documentNumber, bool isPerson)
        {
            if (isPerson)
                return documentNumber.IsValidIndividualRegistrationCharacters();

            return documentNumber.IsValidCNPJCharacters();
        }

        public static bool IsValidDocumentNumber(this string documentNumber, bool isPerson)
        {
            if (isPerson)
                return documentNumber.IsValidIndividualRegistration();

            return documentNumber.IsValidCNPJ();
        }

        public static bool IsValidCNPJCharacters(this string companyRegistration)
        {
            if (companyRegistration == null)
                return false;

            companyRegistration = companyRegistration.Trim();
            companyRegistration = companyRegistration.Replace(".", "").Replace("-", "").Replace("/", "");

            return long.TryParse(companyRegistration, out long valido);
        }

        public static bool IsValidCNPJ(this string cnpj)
        {
            int[] multiplier = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int sum;
            int remainder;
            string digit;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            sum = 0;
            for (int i = 0; i < 12; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier[i];
            remainder = (sum % 11);
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;
            digit = remainder.ToString();
            tempCnpj = tempCnpj + digit;
            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];
            remainder = (sum % 11);
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;
            digit = digit + remainder.ToString();
            return cnpj.EndsWith(digit);
        }

        public static bool IsValidCEP(this string cep)
        {
            if (cep == null)
                return false;

            return new Regex(@"^\d{5}-\d{3}$").Match(cep).Success || new Regex(@"^\d{8}$").Match(cep).Success;
        }

        public static bool IsValidBrazilianState(this string uf)
        {
            if (uf == null)
                return false;

            var ufs = new List<string>()
            { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO" };

            if (!ufs.Contains(uf.ToUpper()))
                return false;

            return true;
        }

        public static bool IsValidPhoneNumber(this string phoneNumber)
        {
            if (phoneNumber == null)
                return false;

            phoneNumber = phoneNumber.UnMask();

            if (phoneNumber.Length != 10 && phoneNumber.Length != 11)
                return false;

            if (!Int64.TryParse(phoneNumber, out long valido))
                return false;

            switch (phoneNumber)
            {
                case "00000000000":
                case "0000000000":
                    return false;
                case "11111111111":
                case "1111111111":
                    return false;
                case "22222222222":
                case "2222222222":
                    return false;
                case "33333333333":
                case "3333333333":
                    return false;
                case "44444444444":
                case "4444444444":
                    return false;
                case "55555555555":
                case "5555555555":
                    return false;
                case "66666666666":
                case "6666666666":
                    return false;
                case "77777777777":
                case "7777777777":
                    return false;
                case "88888888888":
                case "8888888888":
                    return false;
                case "99999999999":
                case "9999999999":
                    return false;
            }

            phoneNumber = phoneNumber.ApplyPhoneNumberMask();

            if (phoneNumber.Length != 14 && phoneNumber.Length != 15)
                return false;

            return true;
        }

        public static bool IsValidPhoneNumberNoDdd(this string phoneNumber)
        {
            if (phoneNumber == null)
                return false;

            phoneNumber = phoneNumber.UnMask();

            if (phoneNumber.Length != 8 && phoneNumber.Length != 9)
                return false;

            if (!Int64.TryParse(phoneNumber, out long valido))
                return false;

            switch (phoneNumber)
            {
                case "000000000":
                case "00000000":
                    return false;
                case "111111111":
                case "11111111":
                    return false;
                case "222222222":
                case "22222222":
                    return false;
                case "333333333":
                case "33333333":
                    return false;
                case "444444444":
                case "44444444":
                    return false;
                case "555555555":
                case "55555555":
                    return false;
                case "666666666":
                case "66666666":
                    return false;
                case "777777777":
                case "77777777":
                    return false;
                case "888888888":
                case "88888888":
                    return false;
                case "999999999":
                case "99999999":
                    return false;
            }

            return true;
        }

        public static bool IsValidEmail(this string email)
        {
            if (email == null)
                return false;

            Regex regex = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
            Match match = regex.Match(email);

            return match.Success;
        }

        public static bool IsValidBase64String(this string base64)
        {
            base64 = base64.Trim();
            bool result = (base64.Length % 4 == 0) && Regex.IsMatch(base64, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
            return result;
        }
        public static bool IsValidPDFInBase64String(this string base64)
        {
            try
            {
                byte[] sPDFDecoded = Convert.FromBase64String(base64);

                //return (sPDFDecoded[0] == 0x25 && sPDFDecoded[1] == 0x50 && sPDFDecoded[2] == 0x44 && sPDFDecoded[3] == 0x46);
                var enc = new ASCIIEncoding();
                var header = enc.GetString(sPDFDecoded);

                //%PDF−1.0
                // If you are loading it into a long, this is (0x04034b50).
                if (sPDFDecoded[0] == 0x25 && sPDFDecoded[1] == 0x50
                    && sPDFDecoded[2] == 0x44 && sPDFDecoded[3] == 0x46)
                {
                    return header.StartsWith("%PDF-");
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static Guid CreateDeterministicGuid(this string uniqueCode)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(uniqueCode));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return new Guid(hash.ToString());
        }

        public static bool IsValidDate(this string date)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");

            if (String.IsNullOrEmpty(date))
                return true;

            return DateTime.TryParse(date, out DateTime result);
        }

        public static bool IsValidReferenceId(this string referenceId)
        {
            if (referenceId == null)
                return true;

            var regex = new Regex("^[a-zA-Z0-9 ]*$");
            Match match = regex.Match(referenceId);

            return match.Success;
        }

        static public string EncodeToBase64(this string texto)
        {
            try
            {
                byte[] textoAsBytes = Encoding.ASCII.GetBytes(texto);
                string resultado = System.Convert.ToBase64String(textoAsBytes);
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }

        static public string DecodeFrom64(this string dados)
        {
            try
            {
                byte[] dadosAsBytes = System.Convert.FromBase64String(dados);
                string resultado = System.Text.ASCIIEncoding.ASCII.GetString(dadosAsBytes);
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
