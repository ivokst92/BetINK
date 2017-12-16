namespace BetINK.Web.TagHelpers
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public class TextareaInkTagHelper : TagHelper
    {
        private readonly IHtmlGenerator htmlGenerator;
        private readonly HtmlHelper htmlHelper;
        public TextareaInkTagHelper(IHtmlGenerator htmlGenerator,
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

        public int Rows { get; set; }

        public int Cols { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            var dataType = For.Metadata.DataTypeName;
            output.Attributes.SetAttribute(new TagHelperAttribute("class", "form-group"));

            Rows = Rows == 0 ? 1 : Rows;
            Cols = Cols == 0 ? 1 : Cols;

            var type = (For.Metadata?.DataTypeName == null ? string.Empty : For.Metadata.DataTypeName);

            var labelTagBuilder = htmlGenerator.GenerateLabel(
                htmlHelper.ViewContext,
                For.ModelExplorer,
                For.Name,
                labelText: For.Metadata.DisplayName,
                htmlAttributes: "");

            var inputTagBuilder = htmlGenerator.GenerateTextArea(
                htmlHelper.ViewContext,
                For.ModelExplorer,
                For.Name,
                rows: Rows,
                columns: Cols,
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