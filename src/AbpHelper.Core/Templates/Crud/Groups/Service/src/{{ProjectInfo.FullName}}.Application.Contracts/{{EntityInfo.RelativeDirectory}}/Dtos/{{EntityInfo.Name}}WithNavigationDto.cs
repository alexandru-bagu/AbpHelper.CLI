using System;
using Volo.Abp.Application.Dtos;

namespace {{ EntityInfo.Namespace }}.Dtos
{
    [Serializable]
    public class {{EntityInfo.Name}}WithNavigationDto
    {
        public {{EntityInfo.Name}}Dto {{EntityInfo.Name}} { get; set; }
    }
}