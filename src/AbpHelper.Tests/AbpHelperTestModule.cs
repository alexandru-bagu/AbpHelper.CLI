﻿using Volo.Abp;
using Volo.Abp.Modularity;

namespace AbpHelper.Tests
{
    [DependsOn(
        typeof(AbpTestBaseModule),
        typeof(AbpHelperModule)
    )]
    public class AbpHelperTestModule : AbpModule
    {
        
    }
}