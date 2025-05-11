using Microsoft.EntityFrameworkCore;
using ShiftSwift.Shared.paging;

namespace ShiftSwift.Application.Extentions;

public static class QuerableExtension
{
    internal static async Task<PaginatedResponse<TEntity>> ToPaginatedListAsync<TEntity>(
        this IQueryable<TEntity> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken) where TEntity : class
    {
        int count = await source.CountAsync();
        pageSize = pageSize == 0 ? 10 : pageSize;

        List<TEntity> items = pageNumber > 1 ? await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync(cancellationToken)
            : await source.Take(pageSize).ToListAsync(cancellationToken);

        return PaginatedResponse<TEntity>.Create(items, count, pageNumber, pageSize);
    }
}