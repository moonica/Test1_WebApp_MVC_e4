using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using Test1_WebApp_MVC.Models.Interfaces;
using Test1_WebApp_MVC.Services;

namespace Test1_WebApp_MVC.Models
{
    public class User : IValidatable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CellNumber { get; set; }
        public List<string> ValidationErrors { get; set; }

        public bool IsValid()
        {
            var result =
                isNotEmptyString(this.Name, nameof(this.Name))
                && isNotEmptyString(this.Surname, nameof(this.Surname))
                && isNotEmptyString(this.CellNumber, nameof(this.CellNumber))
                && isValidCellNr(this.CellNumber, nameof(this.CellNumber));

            return result;
        }

        private bool isNotEmptyString(string str, string fieldName)
        {
            if (string.IsNullOrEmpty(str?.Trim()))
            {
                ValidationErrors = ValidationErrors.NewOrCurrent();
                ValidationErrors.Add($"String '{fieldName}' is a required field");
                return false;
            }

            return true;
        }

        private bool isValidCellNr(string str, string fieldName)
        {
            if (!Utils.MatchesPhoneRegex(str))
            {
                ValidationErrors = ValidationErrors.NewOrCurrent();
                ValidationErrors.Add($"Field '{fieldName}' does not contain a valid phone number ({Utils.VALID_PHONE_EXAMPLE})");
                return false;
            }

            return true;
        }
    }
}
