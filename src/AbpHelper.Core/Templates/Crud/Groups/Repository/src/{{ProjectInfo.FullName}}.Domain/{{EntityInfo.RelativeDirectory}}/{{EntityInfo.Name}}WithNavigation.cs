using System;
    
namespace {{ EntityInfo.Namespace }}
{
    [Serializable]
    public class {{EntityInfo.Name}}WithNavigation
    {
        public {{EntityInfo.Name}} {{EntityInfo.Name}} { get; set; }
    }
}