{{-
if Option.SkipCustomRepository
    if EntityInfo.CompositeKeyName
        repositoryType = "IRepository<" + EntityInfo.Name + ">"
    else
        repositoryType = "IRepository<" + EntityInfo.Name + ", " + EntityInfo.PrimaryKey + ">"
    end
    repositoryName = "Repository"
else
    repositoryType = "I" + EntityInfo.Name + "Repository"
    repositoryName = "_repository"
end ~}}
using System;
{{~ if !EntityInfo.CompositeKeyName
    crudClassName = "CrudAppService"
else
    crudClassName = "AbstractKeyCrudAppService"
~}}
using System.Linq;
using System.Threading.Tasks;
{{~ end -}}
{{~ if !Option.SkipPermissions 
    permissionNamesPrefix = ProjectInfo.Name + "Permissions." + EntityInfo.Name
~}}
using {{ ProjectInfo.FullName }}.Permissions;
{{~ end ~}}
using {{ EntityInfo.Namespace }}.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
{{~ if Option.SkipCustomRepository ~}}
using Volo.Abp.Domain.Repositories;
{{~ end ~}}
using System.Linq;  

namespace {{ EntityInfo.Namespace }}
{
    [RemoteService(IsEnabled = false)]
    public class {{ EntityInfo.Name }}AppService : {{ crudClassName }}<{{ EntityInfo.Name }}, {{ DtoInfo.ReadTypeName }}, {{ EntityInfo.PrimaryKey ?? EntityInfo.CompositeKeyName }}, Get{{EntityInfo.Name}}InputRequestDto, {{ DtoInfo.CreateTypeName }}, {{ DtoInfo.UpdateTypeName }}>,
        I{{ EntityInfo.Name }}AppService
    {
        {{~ if !Option.SkipPermissions ~}}
        protected override string GetPolicyName { get; set; } = {{ permissionNamesPrefix }}.Default;
        protected override string GetListPolicyName { get; set; } = {{ permissionNamesPrefix }}.Default;
        protected override string CreatePolicyName { get; set; } = {{ permissionNamesPrefix }}.Create;
        protected override string UpdatePolicyName { get; set; } = {{ permissionNamesPrefix }}.Update;
        protected override string DeletePolicyName { get; set; } = {{ permissionNamesPrefix }}.Delete;
        {{~ end ~}}

        {{~ if !Option.SkipCustomRepository ~}}
        private readonly {{ repositoryType }} {{ repositoryName }};
        
        public {{ EntityInfo.Name }}AppService({{ repositoryType }} repository) : base(repository)
        {
            {{ repositoryName }} = repository;
        }
        {{~ else ~}}
        public {{ EntityInfo.Name }}AppService({{ repositoryType }} repository) : base(repository)
        {
        }
        {{~ end ~}}
        {{~ if EntityInfo.CompositeKeyName ~}}
        
        protected override Task DeleteByIdAsync({{ EntityInfo.CompositeKeyName }} id)
        {
            // TODO: AbpHelper generated
            return {{ repositoryName }}.DeleteAsync(e =>
            {{~ for prop in EntityInfo.CompositeKeys ~}}
                e.{{ prop.Name }} == id.{{ prop.Name}}{{ if !for.last}} &&{{end}}
            {{~ end ~}}
            );
        }

        protected override async Task<{{ EntityInfo.Name }}> GetEntityByIdAsync({{ EntityInfo.CompositeKeyName }} id)
        {
            // TODO: AbpHelper generated
            return await AsyncExecuter.FirstOrDefaultAsync(
                (await {{ repositoryName }}.WithDetailsAsync()).Where(e =>
                {{~ for prop in EntityInfo.CompositeKeys ~}}
                    e.{{ prop.Name }} == id.{{ prop.Name}}{{ if !for.last}} &&{{end}}
                {{~ end ~}}
                )
            ); 
        }

        protected override IQueryable<{{ EntityInfo.Name }}> ApplyDefaultSorting(IQueryable<{{ EntityInfo.Name }}> query)
        {
            // TODO: AbpHelper generated
            return query.OrderBy(e => e.{{ EntityInfo.CompositeKeys[0].Name }});
        }
        {{~ end ~}}

        protected override async Task<IQueryable<{{ EntityInfo.Name }}>> CreateFilteredQueryAsync(Get{{ EntityInfo.Name }}InputRequestDto input)
        {
            var query = await _repository.GetNavigationQueryableAsync();
            query = _repository.ApplyFilters(query, input.Filter);
            return query.Select(p => p.{{ EntityInfo.Name }});
        }

        [Authorize]
        public async Task<PagedResultDto<LookupDto<Guid>>> GetLookupAsync(Get{{EntityInfo.Name}}LookupRequestDto input)
        {
            var query = await CreateFilteredQueryAsync(input);
            var count = await query.CountAsync();
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var results = await query.ToListAsync();
            return new PagedResultDto<LookupDto<Guid>>()
            {
                TotalCount = count,
                Items = ObjectMapper.Map<List<{{ EntityInfo.Name }}>, List<LookupDto<Guid>>>(results)
            };
        }

        [Authorize({{ permissionNamesPrefix }}.Default)]
        public async Task<PagedResultDto<{{ EntityInfo.Name }}WithNavigationDto>> GetListWithNavigationAsync(Get{{ EntityInfo.Name }}InputRequestDto input)
        {
            var count = await _repository.GetNavigationCountAsync(input.Filter);
            var results = await _repository.GetNavigationListAsync(input.Filter, input.Sorting, input.SkipCount, input.MaxResultCount);
            return new PagedResultDto<{{ EntityInfo.Name }}WithNavigationDto>()
            {
                TotalCount = count,
                Items = ObjectMapper.Map<List<{{ EntityInfo.Name }}WithNavigation>, List<{{ EntityInfo.Name }}WithNavigationDto>>(results)
            };
        }
    }
}
