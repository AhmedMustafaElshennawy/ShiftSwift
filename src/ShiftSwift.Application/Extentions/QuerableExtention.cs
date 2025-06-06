using Microsoft.EntityFrameworkCore;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Extentions;

public static class QuerableExtension
{
    public static async Task<PaginatedResponse<TEntity>> ToPaginatedListAsync<TEntity>(
        this IQueryable<TEntity> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken) where TEntity : class
    {
        var count = await source.CountAsync(cancellationToken);
        pageSize = pageSize == 0 ? 10 : pageSize;

        var items = pageNumber > 1
            ? await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync(cancellationToken)
            : await source.Take(pageSize).ToListAsync(cancellationToken);

        return PaginatedResponse<TEntity>.Create(items, count, pageNumber, pageSize);
    }
}