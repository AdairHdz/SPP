using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace UnitTests.DataPersistenceLayerTests
{
    public static class DbContextMock
    {
        public static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList, Func<T, object> primaryKey = null) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(set => set.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            dbSet.Setup(set => set.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>((s) => sourceList.AddRange(s));
            dbSet.Setup(set => set.Remove(It.IsAny<T>())).Callback<T>((s) => sourceList.Remove(s));
            dbSet.Setup(set => set.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>((s) =>
            {
                for(int i = 0; i < sourceList.Count; i++)
                {
                    foreach(var entity in s)
                    {
                        sourceList.Remove(entity);
                    }
                }
            });
            dbSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => sourceList.SingleOrDefault(x => primaryKey(x).Equals(input.First())));
            return dbSet.Object;
        }

        public static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(set => set.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            dbSet.Setup(set => set.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>((s) => sourceList.AddRange(s));
            dbSet.Setup(set => set.Remove(It.IsAny<T>())).Callback<T>((s) => sourceList.Remove(s));
            dbSet.Setup(set => set.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>((s) =>
            {
                for (int i = 0; i < sourceList.Count; i++)
                {
                    foreach (var entity in s)
                    {
                        sourceList.Remove(entity);
                    }
                }
            });
            return dbSet.Object;
        }

        public static ProfessionalPracticesContext GetContext<T>(DbSet<T> dbSet) where T : class
        {
            var mockContext = new Mock<ProfessionalPracticesContext>();
            mockContext.Setup(c => c.Set<T>()).Returns(dbSet);
            return mockContext.Object;
        }

        public static UnitOfWork GetUnitOfWork(ProfessionalPracticesContext context)
        {
            var mockUnitOfWork = new Mock<UnitOfWork>(context);
            return mockUnitOfWork.Object;
        }
    }
}
