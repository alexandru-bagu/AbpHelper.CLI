{{~ for using in InterfaceInfo.Usings ~}}
{{~ if using != "Volo.Abp.Application.Services" && using != "System.Threading.Tasks" ~}}
using {{ using }};
{{~ end ~}}
{{~ end ~}}
using System.Threading.Tasks;
{{~ if ProjectInfo.TemplateType == 'Application' ~}}
using {{ ProjectInfo.FullName }}.{{ InterfaceInfo.RelativeNamespace }};
{{~ end ~}}
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace {{ ProjectInfo.FullName }}.Controllers.{{ InterfaceInfo.RelativeNamespace }}
{
    {{~ for attribute in ClassInfo.Attributes ~}}
    {{~ if attribute | string.starts_with "[RemoteService"; defined_remote_service = attribute; end ~}}
    {{~ end ~}}
    {{~ if defined_remote_service ~}}
    {{ defined_remote_service }}
    {{~ else ~}}
    [RemoteService(Name = "{{ ProjectInfo.Name }}{{ Option.Name }}")]
    {{~ end ~}}
    [Area("app")]
    [ControllerName("{{ Option.Name }}")]
    {{~ if ProjectInfo.TemplateType == 'Application' ~}}
    [Route("/api/app/{{ Option.Name | abp.camel_case }}")]
    {{~ else if ProjectInfo.TemplateType == 'Module' ~}}
    [Route("/api/{{ ProjectInfo.Name | abp.camel_case }}/{{ Option.Name | abp.camel_case }}")]
    {{~ end ~}}
    public class {{ Option.Name }}Controller : AbpController, I{{ Option.Name }}AppService
    {
        private readonly I{{ Option.Name }}AppService _service;

        public {{ Option.Name }}Controller(I{{ Option.Name }}AppService service)
        {
            _service = service;
        }
        {{~ for method in ClassInfo.Methods | abp.intersect InterfaceInfo.Methods ~}}
{{~ include "Templates/Controller/ControllerMethod" method ~}}
        {{~ end ~}}
    }
}