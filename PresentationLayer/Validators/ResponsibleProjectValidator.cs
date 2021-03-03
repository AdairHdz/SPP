using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace PresentationLayer
{
    class ValidationData
    {
		public static bool ValidateNameComplete(string name)
		{
			bool isValidNameComplete = false;
			name = name.Trim();
			Regex regexName = new Regex(@"^[a-zA-Záéíóú\s]{3,50}$");
			isValidNameComplete = regexName.IsMatch(name);
			return isValidNameComplete;
		}

		public static bool ValidateEmail(string email)
		{
			bool isValidEmail = false;
			Regex regexEmail = new Regex(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
			isValidEmail = regexEmail.IsMatch(email);
			return isValidEmail;
		}

		public static bool ValidateCharge(string charge)
		{
			bool isValidCharge = false;
			charge = charge.Trim();
			Regex regexCharge = new Regex(@"^[a-zA-Záéíóú\s]{4,50}$");
			isValidCharge = regexCharge.IsMatch(charge);
			return isValidCharge;
		}
	}
}
