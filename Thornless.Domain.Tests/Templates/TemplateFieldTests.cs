using System;
using AutoFixture;
using NUnit.Framework;
using Thornless.Domain.Templates;

namespace Thornless.Domain.Tests.Templates
{
    public class TemplateFieldTests
    {
        private readonly Fixture _fixture = new Fixture();
        
        [Test]
        public void GivenField_ParsesCorrectly()
        {
            var withoutBraces = _fixture.Create<string>();
            var withBraces = $"{{{withoutBraces}}}";

            var field1 = new TemplateField(withBraces);
            Assert.AreEqual(withBraces, field1.FieldTemplate);
            Assert.AreEqual(withoutBraces, field1.FieldName);

            var field2 = new TemplateField(withoutBraces);
            Assert.AreEqual(withBraces, field2.FieldTemplate);
            Assert.AreEqual(withoutBraces, field2.FieldName);
        }
    }
}
