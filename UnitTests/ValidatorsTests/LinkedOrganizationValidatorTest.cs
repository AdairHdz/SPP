using System;
using System.Collections.Generic;
using DataPersistenceLayer.Entities;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationLayer.Validators;

namespace UnitTests.ValidatorsTests
{
    [TestClass]
    public class LinkedOrganizationValidatorTest
    {
        private LinkedOrganizationValidator _linkedOrganizationValidator;

        [TestInitialize]
        public void TestInitialize()
        {
            _linkedOrganizationValidator = new LinkedOrganizationValidator();
        }

        [TestMethod]
        public void ValidateLinkedOrganization_Success()
        {
            LinkedOrganization linkedOrganization = new LinkedOrganization
            {
                IdLinkedOrganization = 1,
                Name = "Intel",
                DirectUsers = "Comunidad estudiantil",
                IndirectUsers = "Comunidad tecnologica",
                Email = "intel@gmail.com",
                PhoneNumbers = new List<Phone>
                    {
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281244285"
                        },
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281115489"
                        },
                    },
                Address = "Enrique Segoviano",
                IdCity = 1,
                IdState = 1,
                IdSector = 1,
                LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE,
            };
            var result = _linkedOrganizationValidator.TestValidate(linkedOrganization);
            result.ShouldNotHaveAnyValidationErrors();
        }


        [TestMethod]
        public void ValidateLinkedOrganization_NameError()
        {
            LinkedOrganization linkedOrganization = new LinkedOrganization
            {
                IdLinkedOrganization = 1,
                Name = "43985",
                DirectUsers = "Comunidad estudiantil",
                IndirectUsers = "Comunidad tecnologica",
                Email = "intel@gmail.com",
                PhoneNumbers = new List<Phone>
                    {
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281244285"
                        },
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281115489"
                        },
                    },
                Address = "Enrique Segoviano",
                IdCity = 1,
                IdState = 1,
                IdSector = 1,
                LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE,
            };
            var result = _linkedOrganizationValidator.TestValidate(linkedOrganization);
            result.ShouldHaveValidationErrorFor(l => l.Name);
        }

        [TestMethod]
        public void ValidateLinkedOrganization_AddressError()
        {
            LinkedOrganization linkedOrganization = new LinkedOrganization
            {
                IdLinkedOrganization = 1,
                Name = "Intel",
                DirectUsers = "Comunidad estudiantil",
                IndirectUsers = "Comunidad tecnologica",
                Email = "intel@gmail.com",
                PhoneNumbers = new List<Phone>
                    {
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281244285"
                        },
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281115489"
                        },
                    },
                Address = "",
                IdCity = 1,
                IdState = 1,
                IdSector = 1,
                LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE,
            };
            var result = _linkedOrganizationValidator.TestValidate(linkedOrganization);
            result.ShouldHaveValidationErrorFor(l => l.Address);
        }

        [TestMethod]
        public void ValidateLinkedOrganization_ExtensionOneError()
        {
            LinkedOrganization linkedOrganization = new LinkedOrganization
            {
                IdLinkedOrganization = 1,
                Name = "Intel",
                DirectUsers = "Comunidad estudiantil",
                IndirectUsers = "Comunidad tecnologica",
                Email = "intel@gmail.com",
                PhoneNumbers = new List<Phone>
                    {
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "abc",
                            PhoneNumber = "2281244285"
                        },
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281115489"
                        },
                    },
                Address = "Enrique Segoviano",
                IdCity = 1,
                IdState = 1,
                IdSector = 1,
                LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE,
            };
            var result = _linkedOrganizationValidator.TestValidate(linkedOrganization);
            result.ShouldHaveValidationErrorFor(l => l.PhoneNumbers[0].Extension);
        }

        [TestMethod]
        public void ValidateLinkedOrganization_ExtensionTwoError()
        {
            LinkedOrganization linkedOrganization = new LinkedOrganization
            {
                IdLinkedOrganization = 1,
                Name = "Intel",
                DirectUsers = "Comunidad estudiantil",
                IndirectUsers = "Comunidad tecnologica",
                Email = "intel@gmail.com",
                PhoneNumbers = new List<Phone>
                    {
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281244285"
                        },
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "@#1",
                            PhoneNumber = "2281115489"
                        },
                    },
                Address = "Enrique Segoviano",
                IdCity = 1,
                IdState = 1,
                IdSector = 1,
                LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE,
            };
            var result = _linkedOrganizationValidator.TestValidate(linkedOrganization);
            result.ShouldHaveValidationErrorFor(l => l.PhoneNumbers[1].Extension);
        }

        [TestMethod]
        public void ValidateLinkedOrganization_PhoneOneError()
        {
            LinkedOrganization linkedOrganization = new LinkedOrganization
            {
                IdLinkedOrganization = 1,
                Name = "Intel",
                DirectUsers = "Comunidad estudiantil",
                IndirectUsers = "Comunidad tecnologica",
                Email = "intel@gmail.com",
                PhoneNumbers = new List<Phone>
                    {
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "1234mkl_,p"
                        },
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281115489"
                        },
                    },
                Address = "Enrique Segoviano",
                IdCity = 1,
                IdState = 1,
                IdSector = 1,
                LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE,
            };
            var result = _linkedOrganizationValidator.TestValidate(linkedOrganization);
            result.ShouldHaveValidationErrorFor(l => l.PhoneNumbers[0].PhoneNumber);
        }


        [TestMethod]
        public void ValidateLinkedOrganization_PhoneTwoError()
        {
            LinkedOrganization linkedOrganization = new LinkedOrganization
            {
                IdLinkedOrganization = 1,
                Name = "Intel",
                DirectUsers = "Comunidad estudiantil",
                IndirectUsers = "Comunidad tecnologica",
                Email = "intel@gmail.com",
                PhoneNumbers = new List<Phone>
                    {
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2283898765"
                        },
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "1234mkl_,p"
                        },
                    },
                Address = "Enrique Segoviano",
                IdCity = 1,
                IdState = 1,
                IdSector = 1,
                LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE,
            };
            var result = _linkedOrganizationValidator.TestValidate(linkedOrganization);
            result.ShouldHaveValidationErrorFor(l => l.PhoneNumbers[1].PhoneNumber);
        }

    }
}
