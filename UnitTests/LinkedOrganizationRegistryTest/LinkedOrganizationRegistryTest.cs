using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationLayer.Validators;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using UnitTests.DataPersistenceLayerTests;

namespace UnitTests.LinkedOrganizationRegistryTests
{
	[TestClass]
	public class LinkedOrganizationRegistryTest
	{
		[TestMethod]
		public void LinkedOrganizationRegistryTests()
		{
			List<LinkedOrganization> linkedOrganizations = new List<LinkedOrganization>();
			DbSet<LinkedOrganization> mockSet = DbContextMock.GetQueryableMockDbSet(linkedOrganizations, x => x.Name);
			ProfessionalPracticesContext mockContext =
			DbContextMock.GetContext(mockSet);
			UnitOfWork unitOfWork = DbContextMock.GetUnitOfWork(mockContext);

			LinkedOrganization newlyCreatedLinkedOrganization = new LinkedOrganization
			{
				Name = "IngSog Company",
				Address = "Av. Xalapa s/n, Obrero Campesina, 91020",
				Email = "ingsog@outlook.com",
				DirectUsers = 5,
				IndirectUsers = 10,
				IdCity = 1,
				IdState = 1,
				IdSector = 1,
			};

			unitOfWork.LinkedOrganizations.Add(newlyCreatedLinkedOrganization);

			int expected = 1;
			int actual = linkedOrganizations.Count;
			Assert.AreEqual(expected, actual);
		}


	}
}