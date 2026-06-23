using BuildingBlocks.CQRS;
using CatalogAPI.Exceptions;

namespace CatalogAPI.Products.DeleteProduct;

public record DeleteProductCommand(Guid id):ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.id, cancellationToken);

        if (product is null)
            throw new ProductNotFoundException();

        session.Delete(product);
        await session.SaveChangesAsync();
        return new DeleteProductResult(true);
    }
}
