using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using UnitTests.DataPersistenceLayerTests;

namespace UnitTests.RequestProjectsTestCase
{
	[TestClass]
	public class RequestProjectTest
	{
		private UnitOfWork _unitOfWork;
		private List<Practicioner> _practicioners;
		private DbSet<Practicioner> _mockSetPracticioners;
		private ProfessionalPracticesContext _mockContext;
		private List<RequestProject> _request;
		private DbSet<RequestProject> _mockSetRequest;

		[TestInitialize]
		public void TestInitialize()
		{
			_practicioners = new List<Practicioner> {
				new Practicioner
				{
					Enrollment = "zS18012124",	
				}
			};

			_request = new List<RequestProject>
			{
				new RequestProject
				{
					IdRequestProject = 1,
					RequestDate = DateTime.Now,
					IdProject = 1,
					Enrollment = "zS18012124",
					RequestStatus = RequestStatus.REQUESTED
				}
			};

			_mockSetPracticioners = DbContextMock.GetQueryableMockDbSet(_practicioners, p => p.Enrollment);
			_mockContext = DbContextMock.GetContext(_mockSetPracticioners);
			_mockSetRequest = DbContextMock.GetQueryableMockDbSet(_request, r => r.IdRequestProject);
			_mockContext = DbContextMock.GetContext(_mockSetRequest);
			_unitOfWork = new UnitOfWork(_mockContext);
		}

		[TestMethod]
		public void GetProjectInformation()
		{
			List<Project> projects = new List<Project>
			{
				new Project
				{
					IdProject =2,
					NameProject = "Sistema Integral",
				}
			};
			DbSet<Project> mockSet = DbContextMock.GetQueryableMockDbSet(projects, x => x.IdProject);
			ProfessionalPracticesContext mockContext = DbContextMock.GetContext(mockSet);
			UnitOfWork unitOfWork = new UnitOfWork(mockContext);
			Project projectWithSameName = unitOfWork.Projects.FindFirstOccurence(Project => Project.NameProject == "Sistema Integral");
			Assert.IsNotNull(projectWithSameName);
		}

		[TestMethod]
		public void RequestProject()
		{
			RequestProject request = new RequestProject
			{
				IdRequestProject = 2,
				RequestDate = DateTime.Now,
				IdProject = 2,
				Enrollment = "zS18012124",
				RequestStatus = RequestStatus.REQUESTED
			};
			_unitOfWork.RequestProjects.Add(request);
			int expected = 2;
			int actual = _request.Count;
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetQuantityRequestPracticioner()
		{
			List<RequestProject> requestProject = new List<RequestProject>
			{
				new RequestProject
				{
				IdRequestProject = 3,
				RequestDate = DateTime.Now,
				IdProject = 2,
				Enrollment = "zS18012123",
				RequestStatus = RequestStatus.REQUESTED
				}
			};
			DbSet<RequestProject> mockSet = DbContextMock.GetQueryableMockDbSet(requestProject, x => x.IdRequestProject);
			ProfessionalPracticesContext mockContext = DbContextMock.GetContext(mockSet);
			UnitOfWork unitOfWork = new UnitOfWork(mockContext);

			RequestProject requestProjectsFound = unitOfWork.RequestProjects.FindFirstOccurence(Request => Request.Enrollment == "zS18012123"); 
			Assert.IsNotNull(requestProjectsFound.Enrollment);
		}

	}
}
