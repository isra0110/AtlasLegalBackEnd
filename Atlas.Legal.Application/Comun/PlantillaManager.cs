using System;
using DotLiquid;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Atlas.Legal.Comun
{
    public static class PlantillaManager
    {
        public static string LlenarPlantilla(string html, object objeto)
        {
            DotLiquid.Template.NamingConvention = new DotLiquid.NamingConventions.CSharpNamingConvention();

            var mObjeto = Hash.FromAnonymousObject(objeto, includeBaseClassProperties: true);
            RegisterViewModel(objeto.GetType());

            var mTemplate = Template.Parse(html);

            return RenderViewModel(mTemplate, mObjeto);
        }

        private static void RegisterViewModel(Type rootType)
        {
            rootType
               .Assembly
               .GetTypes()
               .Where(t => t.Namespace == rootType.Namespace)
               .ToList()
               .ForEach(RegisterSafeTypeWithAllProperties);

            rootType
                .GetProperties()
                .Where(p => p.PropertyType.Namespace == "Walmart.Legal")
                .ToList()
                .ForEach(i => RegisterSafeTypeWithAllProperties(i.PropertyType));
        }

        private static void RegisterSafeTypeWithAllProperties(Type type)
        {
            Template.RegisterSafeType(type,
               type
               .GetProperties()
               .Select(p => p.Name)
               .ToArray());
        }

        private static string RenderViewModel(Template template, Hash root)
        {
            return template.Render(root);
        }

    }
}
