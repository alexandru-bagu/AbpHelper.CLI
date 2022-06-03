using System;
using Volo.Abp.Application.Dtos;

namespace {{ EntityInfo.Namespace }}.Dtos
{
    [Serializable]
    public class Get{{EntityInfo.Name}}InputRequestDto : PagedAndSortedResultRequestDto
    {
    }
}