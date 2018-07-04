using System;
using Microsoft.Extensions.Configuration;

namespace FhirStarter.Bonfire.STU3.DotNetCore.Configuration
{
   public class FhirStarterDefaultConfiguration: IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}
