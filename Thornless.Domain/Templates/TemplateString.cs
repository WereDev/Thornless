using System.Collections.Generic;

namespace Thornless.Domain.Templates
{
    public class TemplateString
    {
        public TemplateString(string stringFormat)
        {
            StringFormat = stringFormat;
            SetTemplateFields();
        }

        public string StringFormat { get; private set; }

        public TemplateField[] TemplateFields { get; private set; } = new TemplateField[0];

        private void SetTemplateFields()
        {
            if (string.IsNullOrWhiteSpace(StringFormat)) return;
            
            List<TemplateField> items = new List<TemplateField>();

            List<char> chars = new List<char>();
            bool addchars = false;
            foreach (var c in StringFormat)
            {
                switch (c)
                {
                    case '{':
                        chars.Clear();
                        chars.Add(c);
                        addchars = true;
                        break;
                    case '}':
                        chars.Add(c);
                        string s = new string(chars.ToArray());
                        TemplateField ti = new TemplateField(s);
                        items.Add(ti);
                        addchars = false;
                        break;
                    default:
                        if (addchars)
                            chars.Add(c);
                        break;
                }
            }

            TemplateFields = items.ToArray();
        }
    }
}
