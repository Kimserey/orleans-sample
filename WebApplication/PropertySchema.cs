using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApplication
{
    public class PropertySchema : ISchemaFilter
    {
        public void Apply(Schema model, SchemaFilterContext context)
        {
            if (context.SystemType == typeof(BankAccountController.Property))
            {
                model.Example = new object[] {
                    new {
                        type = "name",
                        value = "string"
                    },
                    new {
                        type = "age",
                        value = "number"
                    }
                };
            }
        }
    }
}
