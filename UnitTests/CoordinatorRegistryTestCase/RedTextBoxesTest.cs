using DataPersistenceLayer.Entities;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationLayer.Validators;
using System.Collections.Generic;
using System.Windows.Controls;

namespace UnitTests.CoordinatorRegistryTestCase
{
    [TestClass]
    public class RedTextBoxesTest
    {
        [TestMethod]
        public void RedTextBoxTest()
        {
            Grid myGrid = new Grid();

            TextBox name = new TextBox
            {
                Text = "",
                Name = "TextBoxFirstName"
            };
            TextBox lastName = new TextBox
            {
                Text = "",
                Name = "TextBoxLastName"
            };

            TextBox phoneNumber = new TextBox
            {
                Text = "2281244285",
                Name = "TextBoxPhoneNumber"
            };

            TextBox email = new TextBox
            {
                Text = "adairho16@gmail.com",
                Name = "TextBoxEmail"
            };

            TextBox alternateEmail = new TextBox
            {
                Text = "adairho16@gmail.com",
                Name = "TextBoxAlternateEmail"
            };

            myGrid.Children.Add(name);
            myGrid.Children.Add(lastName);
            myGrid.Children.Add(phoneNumber);
            myGrid.Children.Add(email);
            myGrid.Children.Add(alternateEmail);

            User user = new User
            {
                Name = name.Text,
                LastName = lastName.Text,
                Email = email.Text,
                AlternateEmail = alternateEmail.Text,
                PhoneNumber = phoneNumber.Text
            };

            UserValidator uv = new UserValidator();
            FluentValidation.Results.ValidationResult vr = uv.Validate(user);
            IList<ValidationFailure> l = vr.Errors;
            UserFeedback uf = new UserFeedback(myGrid, l);

            uf.ShowFeedback();

            Assert.AreEqual(4, uf.ControlsThatHaveBeenPaintedInRed.Count);
        }
    }
}
