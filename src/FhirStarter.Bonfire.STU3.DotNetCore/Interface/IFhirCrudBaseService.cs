using System.Net.Http;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Spark.Engine.DotNetCore.Core;

namespace FhirStarter.Bonfire.STU3.DotNetCore.Interface
{
    public interface IFhirCrudBaseService
    {
        // The name of the Resource you can query (earlier called GetAlias)
        string GetServiceResourceReference();
        HttpResponseMessage Create(IKey key, Resource resource);
        Base Read(SearchParams searchParams);
        Base Read(string id);
        HttpResponseMessage Update(IKey key, Resource resource);
        HttpResponseMessage Delete(IKey key);
        HttpResponseMessage Patch(IKey key, Resource resource);
    }
}
