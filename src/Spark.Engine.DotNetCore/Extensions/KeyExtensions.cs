using System;
using System.Collections.Generic;
using Hl7.Fhir.Model;
using Spark.Engine.DotNetCore.Core;

namespace Spark.Engine.DotNetCore.Extensions
{
    public static class KeyExtensions
    {
        public static Key ExtractKey(this Resource resource)
        {
            var _base = resource.ResourceBase != null ? resource.ResourceBase.ToString() : null;
            var key = new Key(_base, resource.TypeName, resource.Id, resource.VersionId);
            return key;
        }

        public static void ApplyTo(this Key key, Resource resource)
        {
            resource.ResourceBase = key.HasBase() ? new Uri(key.Base) : null;
            resource.Id = key.ResourceId;
            resource.VersionId = key.VersionId;
        }

        private static Key Clone(this Key self)
        {
            var key = new Key(self.Base, self.TypeName, self.ResourceId, self.VersionId);
            return key;
        }

        private static bool HasBase(this Key key)
        {
            return !string.IsNullOrEmpty(key.Base);
        }

        public static Key WithoutBase(this Key self)
        {
            var key = self.Clone();
            key.Base = null;
            return key;
        }

        private static IEnumerable<string> GetSegments(this Key key)
        {
            if (key.Base != null) yield return key.Base;
            if (key.TypeName != null) yield return key.TypeName;
            if (key.ResourceId != null) yield return key.ResourceId;
            if (key.VersionId != null)
            {
                yield return "_history";
                yield return key.VersionId;
            }
        }

        public static string ToUriString(this Key key)
        {
            var segments = key.GetSegments();
            return string.Join("/", segments);
        }
    }
}
