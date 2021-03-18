using DataPersistenceLayer.Entities;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationLayer.Validators;

namespace UnitTests.ValidatorsTests
{
    [TestClass]
    public class UserValidatorTest
    {
        private UserValidator _userValidator;

        [TestInitialize]
        public void TestInitialize()
        {
            _userValidator = new UserValidator();
        }

        [TestMethod]
        public void ValidateUserData_Success()
        {
            User user = new User
            {
                Name = "Adair Benjamin",
                LastName = "Hernandez",
                Gender = Gender.MALE,
                UserStatus = UserStatus.ACTIVE,
                UserType = UserType.Coordinator,
                PhoneNumber = "2281244285",
                Email = "adairho16@gmail.com",
                AlternateEmail = "adairho160@gmail.com"
            };

            var result = _userValidator.TestValidate(user);
            result.ShouldNotHaveAnyValidationErrors();

        }

        [TestMethod]
        public void ValidateUserData_SuccessWithAlternateEmail()
        {
            User user = new User
            {
                Name = "Adair Benjamin",
                LastName = "Hernandez",
                Gender = Gender.MALE,
                UserStatus = UserStatus.ACTIVE,
                UserType = UserType.Coordinator,
                PhoneNumber = "2281244285",
                Email = "adairho16@gmail.com",
                AlternateEmail = "ben.ja20@hotmail.com"
            };

            var result = _userValidator.TestValidate(user);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [DataRow("")]
        [DataRow("Jesus Nauyotzin Ipalnemoani Hernández Fernández Gard")]
        [DataRow("Jesus Nauyotzin Ipalnemoani Hernández Fernández Gar")]
        [TestMethod]
        public void ValidateUserData_Error_InvalidLengthName(string name)
        {
            User user = new User
            {
                Name = name,
                LastName = "Hernández",
                Gender = Gender.MALE,
                UserStatus = UserStatus.ACTIVE,
                UserType = UserType.Coordinator,
                PhoneNumber = "2281244285",
                Email = "adairho16@gmail.com",
                AlternateEmail = ""
            };

            var result = _userValidator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(validatedUser => validatedUser.Name);
        }

        [TestMethod]
        public void ValidateUserData_Error_EmptyLastName()
        {
            User user = new User
            {
                Name = "Adair Benjamín",
                LastName = "",
                Gender = Gender.MALE,
                UserStatus = UserStatus.ACTIVE,
                UserType = UserType.Coordinator,
                PhoneNumber = "2281244285",
                Email = "adairho16@gmail.com",
                AlternateEmail = ""
            };

            var result = _userValidator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(validatedUser => validatedUser.LastName);
        }

        [DataRow("")]
        [DataRow("              ")]
        [DataRow(" ")]
        [DataRow("Hernández Ortiz Hernández Ortiz Hernández Ortiz")]
        [DataRow("Gómez Mendez Valdés Montes Vldz")]
        [DataRow("Aguilar García Valdés Ávila Dio")]
        [TestMethod]
        public void ValidateUserData_Error_InvalidLengthLastName(string lastName)
        {
            User user = new User
            {
                Name = "Adair Benjamín",
                LastName = lastName,
                Gender = Gender.MALE,
                UserStatus = UserStatus.ACTIVE,
                UserType = UserType.Coordinator,
                PhoneNumber = "2281244285",
                Email = "adairho16@gmail.com",
                AlternateEmail = ""
            };

            var result = _userValidator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(validatedUser => validatedUser.LastName);
        }

        [DataRow(-1)]
        [DataRow(2)]        
        [TestMethod]
        public void ValidateUserData_Error_GenderNotInEnum(int gender)
        {
            User user = new User
            {
                Name = "Adair Benjamín",
                LastName = "Hernández Ortiz",
                Gender = (Gender)gender,
                UserStatus = UserStatus.ACTIVE,
                UserType = UserType.Coordinator,
                PhoneNumber = "2281244285",
                Email = "adairho16@gmail.com",
                AlternateEmail = ""
            };

            var result = _userValidator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(validatedUser => validatedUser.Gender);
        }

        [DataRow(-1)]
        [DataRow(2)]
        [TestMethod]
        public void ValidateUserData_Error_UserStatusNotInEnum(int userStatus)
        {
            User user = new User
            {
                Name = "Adair Benjamín",
                LastName = "Hernández Ortiz",
                Gender = Gender.MALE,
                UserStatus = (UserStatus)userStatus,
                UserType = UserType.Coordinator,
                PhoneNumber = "2281244285",
                Email = "adairho16@gmail.com",
                AlternateEmail = ""
            };

            var result = _userValidator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(validatedUser => validatedUser.UserStatus);
        }

        [DataRow(-1)]
        [DataRow(4)]
        [TestMethod]
        public void ValidateUserData_Error_UserTypeNotInEnum(int userType)
        {
            User user = new User
            {
                Name = "Adair Benjamín",
                LastName = "Hernández Ortiz",
                Gender = Gender.MALE,
                UserStatus = UserStatus.ACTIVE,
                UserType = (UserType)userType,
                PhoneNumber = "2281244285",
                Email = "adairho16@gmail.com",
                AlternateEmail = ""
            };

            var result = _userValidator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(validatedUser => validatedUser.UserType);
        }

        [TestMethod]
        public void ValidateUserData_Error_PhoneNumberIsEmpty()
        {
            User user = new User
            {
                Name = "Adair Benjamín",
                LastName = "Hernández Ortiz",
                Gender = Gender.MALE,
                UserStatus = UserStatus.ACTIVE,
                UserType = UserType.Manager,
                PhoneNumber = "",
                Email = "adairho16@gmail.com",
                AlternateEmail = ""
            };

            var result = _userValidator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(validatedUser => validatedUser.PhoneNumber);
        }

        [DataRow("")]
        [DataRow("          ")]
        [DataRow("12345678900")]
        [DataRow("123456789")]
        [TestMethod]
        public void ValidateUserData_Error_InvalidLengthPhoneNumber(string phoneNumber)
        {
            User user = new User
            {
                Name = "Adair Benjamín",
                LastName = "Hernández Ortiz",
                Gender = Gender.MALE,
                UserStatus = UserStatus.ACTIVE,
                UserType = UserType.Manager,
                PhoneNumber = phoneNumber,
                Email = "adairho16@gmail.com",
                AlternateEmail = ""
            };

            var result = _userValidator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(validatedUser => validatedUser.PhoneNumber);
        }
        
        [DataRow("adair")]
        [DataRow("hernandez@")]
        [DataRow("soluciones.tics@hotmail")]
        [DataRow("lis.uv@uv..com")]
        [DataRow("")]
        [DataRow("@invalido@gmail.com")]
        [DataRow("hernandezsoluciones.ticsinvalidolis.uvadairtelefonoce"
            + "lularcomputadoraescritoriohernandezsoluciones.ticsinvalidolis.uvadairtelefonocelularcomputadoraescri"
            +"toriohernandezsoluciones.ticsinvalidolis.uvadairtelefonocelularcomputadoraescritoriopruebaaa@gmail.com")]
        [TestMethod]
        public void ValidateUserData_Error_InvalidEmail(string email)
        {
            User user = new User
            {
                Name = "Adair Benjamín",
                LastName = "Hernández Ortiz",
                Gender = Gender.MALE,
                UserStatus = UserStatus.ACTIVE,
                UserType = UserType.Manager,
                PhoneNumber = "2281244285",
                Email = email,
                AlternateEmail = ""
            };

            var result = _userValidator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(validatedUser => validatedUser.Email);
        }

        [DataRow("adair")]
        [DataRow("hernandez@")]
        [DataRow("soluciones.tics@hotmail")]
        [DataRow("lis.uv@uv..com")]
        [DataRow("@invalido@gmail.com")]
        [DataRow("hernandezsoluciones.ticsinvalidolis.uvadairtelefonoce"
            + "lularcomputadoraescritoriohernandezsoluciones.ticsinvalidolis.uvadairtelefonocelularcomputadoraescri"
            + "toriohernandezsoluciones.ticsinvalidolis.uvadairtelefonocelularcomputadoraescritoriopruebaaa@gmail.com")]
        [TestMethod]
        public void ValidateUserData_Error_InvalidAlternateEmail(string alternateEmail)
        {
            User user = new User
            {
                Name = "Adair Benjamín",
                LastName = "Hernández Ortiz",
                Gender = Gender.MALE,
                UserStatus = UserStatus.ACTIVE,
                UserType = UserType.Manager,
                PhoneNumber = "2281244285",
                Email = "adairho16@gmail.com",
                AlternateEmail = alternateEmail
            };

            var result = _userValidator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(validatedUser => validatedUser.AlternateEmail);
        }
    }
}
