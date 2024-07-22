using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AliciasPieShop.TagHelpers
{
    public class EmailTagHelper: TagHelper    // by convention we will use this tag using <email></email>
    {
        public string? Address { get; set; }
        public string? Content { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", "mailto:" + Address);
            output.Content.SetContent(Content);
        }
    }
}
