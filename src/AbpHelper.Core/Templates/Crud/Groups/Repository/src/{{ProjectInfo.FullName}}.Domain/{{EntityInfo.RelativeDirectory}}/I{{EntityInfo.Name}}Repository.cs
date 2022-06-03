{{~    
    if EntityInfo.CompositeKeyName
        repository = "IRepository<" + EntityInfo.Name + ">"
    else
        repository = "IRepository<" + EntityInfo.Name + ", " + EntityInfo.PrimaryKey + ">"
    end
~}}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace {{ EntityInfo.Namespace }}
{
    public interface I{{ EntityInfo.Name }}Repository : {{ repository }}
    {
        Task<long> GetNavigationCountAsync(string filter = null);

        Task<List<{{ EntityInfo.Name }}WithNavigation>> GetNavigationListAsync(string filter = null,  string sorting = null, int? skipCount = null, int? maxResultCount = null);

        Task<IQueryable<{{ EntityInfo.Name }}WithNavigation>> GetNavigationQueryableAsync();

        Task<IQueryable<{{ EntityInfo.Name }}WithNavigation>> GetPreparedNavigationQueryableAsync(string filter = null, string sorting = null, int? skipCount = null, int? maxResultCount = null);

        IQueryable<{{ EntityInfo.Name }}WithNavigation> ApplyFilters(IQueryable<{{ EntityInfo.Name }}WithNavigation> query, string filter = null);

        IQueryable<{{ EntityInfo.Name }}WithNavigation> ApplySorting(IQueryable<{{ EntityInfo.Name }}WithNavigation> query, string sorting = null);

        IQueryable<{{ EntityInfo.Name }}WithNavigation> ApplyPaging(IQueryable<{{ EntityInfo.Name }}WithNavigation> query, int? skipCount = null, int? maxResultCount = null);

    }
}