﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Application
{
    public static class DI
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {


            return services;
        }
    }
}
