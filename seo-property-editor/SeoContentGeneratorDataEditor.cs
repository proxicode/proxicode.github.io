using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Composing;

namespace SeoPropertyEditor
{
    [DataEditor(
        alias: "SeoContentGenerator",
        name: "SEO Content Generator",
        view: "/App_Plugins/SeoPropertyEditor/Views/SeoContentGeneratorEditor.html",
        ValueType = ValueTypes.Text,
        Group = "SEO"]
    )]
    public class SeoContentGeneratorDataEditor : DataEditor
    {
        public SeoContentGeneratorDataEditor(IDataValueEditorFactory dataValueEditorFactory, ILogger<SeoContentGeneratorDataEditor> logger)
            : base(dataValueEditorFactory, logger)
        {
        }

        protected override IConfigurationEditor CreateConfigurationEditor() => new SeoContentGeneratorConfiguration();

        public class SeoContentGeneratorConfiguration : ConfigurationEditor<Dictionary<string, object>>
        {
        }
    }
}
