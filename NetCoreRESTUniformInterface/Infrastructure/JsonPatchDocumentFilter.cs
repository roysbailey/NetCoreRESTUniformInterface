using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreRESTUniformInterface.Infrastructure
{
    public class JsonPatchDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var schemas = swaggerDoc.Components.Schemas.ToList();

            var patchOperation = swaggerDoc.Components.Schemas.ToList()
                .FirstOrDefault(s => s.Key.ToLower() == "operation");

            if (patchOperation.Key != default(string))
                patchOperation.Value.Properties.Remove("operationType");

        }
    }
}
