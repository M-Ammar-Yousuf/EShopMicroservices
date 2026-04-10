namespace Catalog.API.Products.GetProductById;

public record GetProductByIdRequest(Guid Id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product Product);

internal class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger) : IQueryHandler<GetProductByIdRequest, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdRequest query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByIdQueryHandler.Handle called with {@query}", query);

        var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

        if (product is null)
        {
            logger.LogWarning("Product with id {id} not found", query.Id);
            throw new ProductNotFoundException();
        }

        var result = new GetProductByIdResult(product);
        return result;
    }
}
