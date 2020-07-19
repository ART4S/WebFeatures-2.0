using System.Linq;
using System.Threading.Tasks;
using Application.Features.Products.GetProducts;
using Application.Tests.Common.Base;
using Application.Tests.Common.Helpers;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Integration.Features.Products
{
	using static EntityTestData;
	
	public class GetProductsQueryTests : RequestTestBase
	{
		[Fact]
		public async Task ShouldReturnProductList()
		{
			// Arrange
			var request = new GetProductsQuery();
			
			// Act
			IQueryable<ProductListDto> list = await SendODataAsync(request);
			
			// Assert
			list.Should().Contain(x => x.Id == ProductId);
		}
	}
}