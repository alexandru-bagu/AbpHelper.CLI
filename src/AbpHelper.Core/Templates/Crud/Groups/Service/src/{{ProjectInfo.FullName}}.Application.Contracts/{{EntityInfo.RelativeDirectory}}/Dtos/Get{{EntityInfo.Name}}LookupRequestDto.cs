using System;
using Volo.Abp.Application.Dtos;

namespace {{ EntityInfo.Namespace }}.Dtos
{   
    [Serializable]
    public class Get{{EntityInfo.Name}}LookupRequestDto : Get{{EntityInfo.Name}}InputRequestDto 
    {
        public Get{{EntityInfo.Name}}LookupRequestDto()
        { 
            MaxResultCount = MaxMaxResultCount;
        }
    }
}