using System;

namespace Thornless.Domain.Templates
{
    public class TemplateField
    {
        public TemplateField(string fieldTemplate)
        {
            if (string.IsNullOrWhiteSpace(fieldTemplate))
                throw new ArgumentNullException(nameof(fieldTemplate));

            SetFields(fieldTemplate);
        }

        public string FieldTemplate { get; private set; } = string.Empty;

        public string FieldName { get; private set; } = string.Empty;

        private void SetFields(string fieldTemplate)
        {
            if (!fieldTemplate.StartsWith("{"))
                fieldTemplate = "{" + fieldTemplate;

            if (!fieldTemplate.EndsWith("}"))
                fieldTemplate += "}";

            int colonIndex = fieldTemplate.IndexOf(":");

            if (colonIndex > -1)
                FieldName = fieldTemplate[1..colonIndex];
            else
                FieldName = fieldTemplate[1..^1];
            FieldTemplate = fieldTemplate;
        }
    }
}
