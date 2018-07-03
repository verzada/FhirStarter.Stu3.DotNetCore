using System.Collections.Generic;
using Hl7.Fhir.Model;

namespace FhirStarter.Bonfire.STU3.DotNetCore.Interface
{
    /// <summary>
    /// The interface used by the FhirStarter Inferno server to expose the fhir service. 
    /// It is not meant to handle internal services
    /// </summary>
    public interface IFhirService
    {
        CapabilityStatement.RestComponent GetRestDefinition();
        OperationDefinition GetOperationDefinition();
        ICollection<string> GetStructureDefinitionNames();
    }
}
