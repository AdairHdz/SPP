using DataPersistenceLayer.Entities;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationLayer.Validators;
using System.Collections.Generic;
using System.Windows.Controls;

namespace UnitTests.CoordinatorRegistryTestCase
{
    [TestClass]
    public class GreenTextBoxesTest
    {
        [TestMethod]
        public void GreenTextBoxTest()
        {
            Grid myGrid = new Grid();

            TextBox name = new TextBox
            {
                Text = "Adair",
                Name = "TextBoxFirstName"
            };
            TextBox lastName = new TextBox
            {
                Text = "Hernández",
                Name = "TextBoxLastName"
            };

            myGrid.Children.Add(name);
            myGrid.Children.Add(lastName);

            User user = new User
            {
                Name = name.Text,
                LastName = lastName.Text
            };

            UserValidator uv = new UserValidator();
            FluentValidation.Results.ValidationResult vr = uv.Validate(user);
            IList<ValidationFailure> l = vr.Errors;
            UserFeedback uf = new UserFeedback(myGrid, l);

            uf.ShowFeedback();

            Assert.AreEqual(2, uf.ControlsToBePaintedInGreen.Count);

        }
    }
}
