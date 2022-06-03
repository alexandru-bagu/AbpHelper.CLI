using System;
using {{ EntityInfo.Namespace }}.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using System.Threading.Tasks;   

namespace {{ EntityInfo.Namespace }}
{
    public interface I{{ EntityInfo.Name }}AppService :
        ICrudAppService< 
            {{ DtoInfo.ReadTypeName }}, 
            {{ EntityInfo.PrimaryKey ?? EntityInfo.CompositeKeyName }}, 
            Get{{EntityInfo.Name}}InputRequestDto,
            {{ DtoInfo.CreateTypeName }},
            {{ DtoInfo.UpdateTypeName }}>
    {
        Task<PagedResultDto<LookupDto<Guid>>> GetLookupAsync(Get{{EntityInfo.Name}}LookupRequestDto input);

        Task<PagedResultDto<{{ EntityInfo.Name }}WithNavigationDto>> GetListWithNavigationAsync(Get{{ EntityInfo.Name }}InputRequestDto input);
    }
}