{{~
if ProjectInfo.TemplateType == 'Module'
    dbContextName = "I" + ProjectInfo.Name + "DbContext"
else
    dbContextName = ProjectInfo.Name + "DbContext"
end
~}}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using {{ ProjectInfo.FullName }}.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace {{ EntityInfo.Namespace }}
{
{{~
    if EntityInfo.CompositeKeyName
        primaryKeyText = ""
    else
        primaryKeyText = ", " + EntityInfo.PrimaryKey
    end
~}}
    public class {{ EntityInfo.Name }}Repository : EfCoreRepository<{{ dbContextName }}, {{ EntityInfo.Name }}{{ primaryKeyText }}>, I{{ EntityInfo.Name }}Repository
    {
        public {{ EntityInfo.Name }}Repository(IDbContextProvider<{{ dbContextName }}> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<long> GetNavigationCountAsync(string filter = null)
        {
            var query = await GetNavigationQueryableAsync();
            query = ApplyFilters(query, filter);
            return await query.LongCountAsync();
        }

        public async Task<List<{{ EntityInfo.Name }}WithNavigation>> GetNavigationListAsync(string filter = null, string sorting = null, int? skipCount = null, int? maxResultCount = null)
        {
            var query = await GetPreparedNavigationQueryableAsync(filter, skipCount, maxResultCount, sorting);
            return await query.ToListAsync();
        }

        public async Task<IQueryable<{{ EntityInfo.Name }}WithNavigation>> GetPreparedNavigationQueryableAsync(string filter = null, string sorting = null, int? skipCount = null, int? maxResultCount = null)
        {
            var query = await GetNavigationQueryableAsync();
            query = ApplyFilters(query, filter, networkId);
            query = ApplySorting(query, sorting);
            query = ApplyPaging(query, skipCount, maxResultCount);
            return query;
        }

        public IQueryable<{{ EntityInfo.Name }}WithNavigation> ApplyPaging(IQueryable<{{ EntityInfo.Name }}WithNavigation> query, int? skipCount, int? maxResultCount)
        {
            if (skipCount.HasValue) query = query.Skip(skipCount.Value);
            if (maxResultCount.HasValue) query = query.Take(maxResultCount.Value);
            return query;
        }

        public IQueryable<{{ EntityInfo.Name }}WithNavigation> ApplySorting(IQueryable<{{ EntityInfo.Name }}WithNavigation> query, string sorting)
        {
            if (!string.IsNullOrEmpty(sorting)) query = query.OrderBy(sorting);
            else query = query.OrderBy(p => p.{{ EntityInfo.Name }}.CreationTime);
            return query;
        }

        public IQueryable<{{ EntityInfo.Name }}WithNavigation> ApplyFilters(IQueryable<{{ EntityInfo.Name }}WithNavigation> query, string filter = null)
        {
            query = query.WhereIf(!string.IsNullOrEmpty(filter), p => true);
            return query;
        }

        public async Task<IQueryable<{{ EntityInfo.Name }}WithNavigation>> GetNavigationQueryableAsync()
        {
            var ctx = await GetDbContextAsync();
            var query = from {{ EntityInfo.Name | abp.camel_case }} in ctx.{{ EntityInfo.NamePluralized }}
                        select new {{ EntityInfo.Name }}WithNavigation
                        {
                            {{ EntityInfo.Name }} = {{ EntityInfo.Name | abp.camel_case }}
                        };
            return query;
        }

    }
}