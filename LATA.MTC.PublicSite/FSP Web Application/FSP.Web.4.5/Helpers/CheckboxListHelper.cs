using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Web.Routing;
using System.Text;

namespace FSP.Web.Helpers
{
    public static class CheckboxListHelper
    {
        public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items)
        {

            var output = new StringBuilder();

            foreach (var item in items)
            {
                output.Append(@"<label class=""checkbox"">");
                output.Append(@"<input type=""checkbox"" name=""");
                output.Append(name);
                output.Append("\" value=\"");
                output.Append(item.Value);
                output.Append("\"");
                if (item.Selected)
                    output.Append(@" checked=""checked""");
                output.Append(" />");
                output.Append(item.Text);
                output.Append("</label>");
            }


            return MvcHtmlString.Create(output.ToString());

        }
    }
}