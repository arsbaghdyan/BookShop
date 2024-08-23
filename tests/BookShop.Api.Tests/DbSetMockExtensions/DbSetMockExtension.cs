using Microsoft.EntityFrameworkCore;
using Moq;

namespace DbSetMockExtensions
{
    public static class DbSetMockExtension
    {
        public static Mock<DbSet<T>> CreateDbSetMock<T>(this IEnumerable<T> entities) where T : class
        {
            var entitiesList = entities.ToList();
            var entitiesQueryable = entitiesList.AsQueryable();

            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            dbSetMock.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(entitiesList.Add);

            return dbSetMock;
        }

        public static void ReturnsDbSet<T>(this Mock<DbSet<T>> dbSetMock, IEnumerable<T> entities) where T : class
        {
            var entitiesQueryable = entities.AsQueryable();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
        }
    }
}
