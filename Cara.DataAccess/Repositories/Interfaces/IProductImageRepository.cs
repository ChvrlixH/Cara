using Cara.Core.Entities;

namespace Cara.DataAccess.Repositories.Interfaces;

public interface IProductImageRepository : IRepository<ProductImage>
{
	bool DoesPImageExist(int productId);
	Task<List<ProductImage>> ListIncludeAsync();
	Task<ProductImage> FirstInclude(int id);
	IEnumerable<ProductImage> GetAllProduct();
}
