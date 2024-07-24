using AliciasPieShop.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace AliciasPieShopTests.TagHelpers
{
    public class EmailTagHelperTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public EmailTagHelperTests(ITestOutputHelper helper)
        {
            _outputHelper = helper;
        }

        /// <summary>
        /// verify that the Email tag helper generates the correct output link (a mailto link which opens the email client to email the pie shop owner)
        /// </summary>
        [Fact]
        public void Generates_Email_Link()
        {
            // ARRANGE
            EmailTagHelper emailTagHelper = new EmailTagHelper()
            {
                Address = "test@aliciaspieshop.com",
                Content = "Email"
            };

            _outputHelper.WriteLine("created test email tag helper. Address = '{0}', Content = '{1}'", emailTagHelper.Address, emailTagHelper.Content);

            // unexplained setup :/ no idea what this does, if you ever end up needing to unit test tag helpers do some research
            var tagHelperContext = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                string.Empty);

            var content = new Mock<TagHelperContent>();     // this is a Moq Mock object wrapping a TagHelperContent instance

            var tagHelperOutput = new TagHelperOutput(
                "a",
                new TagHelperAttributeList(),
                (cache, encoder) => Task.FromResult(content.Object));

            // ACT
            emailTagHelper.Process(tagHelperContext, tagHelperOutput);

            // ASSERT

            // verify that the content (the stuff between the opening and closing tags) is "Email"
            Assert.Equal("Email", tagHelperOutput.Content.GetContent());

            // verify that tag name is 'a', in other words verify that an anchor tag (<a>) was generated
            Assert.Equal("a", tagHelperOutput.TagName);

            // verify that an attribute with valie "mailto:[emailAddress]" was generated
            Assert.Equal("mailto:test@aliciaspieshop.com", tagHelperOutput.Attributes[0].Value);

        }
    }
}
