namespace BetINK.Web.TagHelpers
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public class InputInkTagHelper : TagHelper
    {
        private readonly IHtmlGenerator htmlGenerator;
        private readonly HtmlHelper htmlHelper;
        public InputInkTagHelper(IHtmlGenerator htmlGenerator,
           IHtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper as HtmlHelper;
            this.htmlGenerator = htmlGenerator;
        }

        [ViewContext]
        public ViewContext ViewContext
        {
            set => htmlHelper.Contextualize(value);
        }

        public ModelExpression For { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            var dataType = For.Metadata.DataTypeName;
            output.Attributes.SetAttribute(new TagHelperAttribute("class", "form-group"));

            var type = (For.Metadata?.DataTypeName == null ? string.Empty : For.Metadata.DataTypeName);

            var labelTagBuilder = htmlGenerator.GenerateLabel(
                htmlHelper.ViewContext,
                For.ModelExplorer,
                For.Name,
                labelText: For.Metadata.DisplayName,
                htmlAttributes: "");

            var inputTagBuilder = htmlGenerator.GenerateTextBox(
                htmlHelper.ViewContext,
                For.ModelExplorer,
                For.Name,
                value: For.Model,
                format: null,
                htmlAttributes: new { type, @class = "form-control" });

            var validationTagBuilder = htmlGenerator.GenerateValidationMessage(
                htmlHelper.ViewContext,
                For.ModelExplorer,
                For.Name,
                message: null,
                tag: null,
                htmlAttributes: new { @class = "form-text text-danger" });

            output.Content.AppendHtml(labelTagBuilder);
            output.Content.AppendHtml(inputTagBuilder);
            output.Content.AppendHtml(validationTagBuilder);

            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}