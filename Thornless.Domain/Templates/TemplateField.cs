using System;

namespace Thornless.Domain.Templates
{
    public class TemplateField
    {
        private string _fieldTemplate = string.Empty;
        private string _fieldName = string.Empty;

        public string FieldTemplate
        {
            get
            {
                return _fieldTemplate;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _fieldName = string.Empty;
                    _fieldTemplate = string.Empty;
                }
                else
                {
                    _fieldTemplate = value;
                    SetFieldName();
                }
            }
        }

        public string FieldName { get; private set; }

        private void SetFieldName()
        {
            _fieldName = string.Empty;

            if (!_fieldTemplate.StartsWith("{") || !_fieldTemplate.EndsWith("}"))
                throw new ArgumentException("Invalid field format.  Template field should start/end with {/}");

            int colonIndex = _fieldTemplate.IndexOf(":");

            if (colonIndex > -1)
                _fieldName = _fieldTemplate[1..colonIndex];
            else
                _fieldName = _fieldTemplate[1..^1];
        }
    }
}
