using BuildingBlocks.CQRS;
using CatalogAPI.Models;

namespace CatalogAPI.Products.CreateProduct;

public record CreateProductCommand(string name,List<string> category, string description, string imageFile, decimal price) 
    : ICommand<CreateProductResult>;

public record CreateProductResult(Guid id);

internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        //Create Product entity from command object 
        var product = new Product
        {
            Name = command.name,
            Description = command.description,
            Price = command.price,
            ImageFile = command.imageFile,
            Category = command.category,
        };

        //Save to database
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        //return CreateProductResult result
        return new CreateProductResult(product.Id);
    }
}
