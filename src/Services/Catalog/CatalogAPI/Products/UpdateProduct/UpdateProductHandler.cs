using BuildingBlocks.CQRS;
using CatalogAPI.Exceptions;

namespace CatalogAPI.Products.UpdateProduct;

public record UpdateProductCommand(Guid id, string name, List<string> category, string description, decimal price) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product= await session.LoadAsync<Product>(command.id, cancellationToken);
        
        if(product is null)
        {
            throw new ProductNotFoundException();
        }

        product.Name = command.name;
        product.Category = command.category;
        product.Description = command.description;
        product.Price = command.price;

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}
