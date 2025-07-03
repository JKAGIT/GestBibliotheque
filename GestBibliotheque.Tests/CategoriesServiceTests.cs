using GestBibliotheque;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestBibliotheque.Models;
using GestBibliotheque.Repositories;
using GestBibliotheque.Services;
using GestBibliotheque.Utilitaires;
using Xunit;


namespace GestBibliotheque.Tests
{
    public class CategoriesServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IEntityValidationService<Categories>> _mockValidationService;
        private readonly Mock<IRecherche<Categories>> _mockRecherche;
        private readonly CategoriesService _categoriesService;
        public CategoriesServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockValidationService = new Mock<IEntityValidationService<Categories>>();
            _mockRecherche = new Mock<IRecherche<Categories>>();
            _categoriesService = new CategoriesService(_mockUnitOfWork.Object, _mockValidationService.Object, _mockRecherche.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldAddCategory_WhenValid()
        {
            // Arrange
            var newCategorie = new Categories { ID = Guid.NewGuid(), Code = "CAT001" };

            var mockCategoriesRepo = new Mock<IGenericRepository<Categories>>();
            _mockUnitOfWork.Setup(u => u.Categories).Returns(mockCategoriesRepo.Object);

            _mockValidationService
                .Setup(s => s.VerifierExistenceAsync(It.IsAny<Expression<Func<Categories, bool>>>()))
                .ReturnsAsync(false);

            // Act
            await _categoriesService.AddAsync(newCategorie);

            // Assert
            mockCategoriesRepo.Verify(r => r.AddAsync(newCategorie), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }


    }



}
