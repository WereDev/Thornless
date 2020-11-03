using System.Linq;
using AutoFixture;
using NUnit.Framework;
using Thornless.Domain.Templates;

namespace Thornless.Domain.Tests.Templates
{
    public class TemplateStringTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GivenStringItem_WhenNFields_ParsesCorrectly(int num)
        {
            var strings = _fixture.CreateMany<string>(num).ToArray();

            var templatedStrings = strings.Select(x => "{" + x + "}").ToArray();

            var stringFormat = "start " + string.Join(" sep ", templatedStrings) + "end";

            var templateString = new TemplateString(stringFormat);

            Assert.AreEqual(stringFormat, templateString.StringFormat);
            Assert.AreEqual(num, templateString.TemplateFields.Length);
            for (int i = 0; i < num; i++)
            {
                Assert.AreEqual(strings[i], templateString.TemplateFields[i].FieldName);
                Assert.AreEqual(templatedStrings[i], templateString.TemplateFields[i].FieldTemplate);
            }
        }
    }
}
