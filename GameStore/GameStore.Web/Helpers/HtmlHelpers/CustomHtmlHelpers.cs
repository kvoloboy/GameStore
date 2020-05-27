using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using GameStore.Web.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStore.Web.Helpers.HtmlHelpers
{
    public static class CustomHtmlHelpers
    {
        public static IHtmlContent CheckBoxList(
            this IHtmlHelper htmlHelper,
            IEnumerable<ListItem> items,
            IEnumerable<string> selectedItems,
            string name,
            string containerId)
        {
            var ul = CreateContainer("ul");
            ul.Attributes.Add("id", containerId);
            selectedItems ??= new string[0];

            foreach (var rootItem in items)
            {
                var inputId = $"{name}{rootItem.Id}";
                var isChecked = selectedItems.Contains(rootItem.Id);

                var label = CreateLabel(rootItem.Name, inputId);
                var checkbox = CreateCheckbox(name, inputId, rootItem.Id.ToString(), isChecked);
                var li = CreateContainer("li", checkbox, label);

                ul.InnerHtml.AppendHtml(li);
            }

            var content = GetMarkup(ul);
            var markup = new HtmlString(content);

            return markup;
        }

        public static IHtmlContent CheckBoxList(
            this IHtmlHelper htmlHelper,
            IEnumerable<TreeViewListItem> items,
            IEnumerable<string> selectedItems,
            string name,
            string containerId)
        {
            var container = CreateContainer();
            container.Attributes.Add("id", containerId);
            selectedItems ??= new string[0];

            foreach (var rootItem in items)
            {
                var idSegment = name.Replace(".", "_");
                var checkboxId = $"{idSegment}{rootItem.Id}";
                var isChecked = selectedItems.Contains(rootItem.Id);

                var checkbox = CreateCheckbox(name, checkboxId, rootItem.Id.ToString(), isChecked);
                var label = CreateLabel(rootItem.Name, checkboxId);
                var innerContainer = CreateContainer(innerElements: new[] {checkbox, label});

                AppendChildren(rootItem, selectedItems, name, innerContainer);

                container.InnerHtml.AppendHtml(innerContainer);
            }

            var content = GetMarkup(container);
            var markup = new HtmlString(content);

            return markup;
        }


        private static void AppendChildren(
            TreeViewListItem parent,
            IEnumerable<string> selectedItems,
            string name,
            TagBuilder container)
        {
            if (parent.Children == null)
            {
                return;
            }

            foreach (var child in parent.Children)
            {
                var idSegment = name.Replace(".", "_");
                var checkboxId = $"{idSegment}{child.Id}";
                var parentId = $"{idSegment}{parent.Id}";
                var isChecked = selectedItems.Contains(child.Id);

                var checkbox = CreateCheckbox(name, checkboxId, child.Id.ToString(), isChecked, parentId);
                var label = CreateLabel(child.Name, checkboxId);
                var innerContainer = CreateContainer(innerElements: new[] {checkbox, label});
                innerContainer.AddCssClass("pl-3");

                AppendChildren(child, selectedItems, name, innerContainer);

                container.InnerHtml.AppendHtml(innerContainer);
            }
        }

        private static TagBuilder CreateContainer(string tagName = "div", params TagBuilder[] innerElements)
        {
            var container = new TagBuilder(tagName);
            container.AddCssClass("form-check");

            foreach (var innerElement in innerElements)
            {
                container.InnerHtml.AppendHtml(innerElement);
            }

            return container;
        }

        private static TagBuilder CreateCheckbox(string name, string id, string value, bool isChecked,
            string parentId = null)
        {
            var input = new TagBuilder("input");

            input.Attributes.Add("type", "checkbox");
            input.Attributes.Add("name", name);
            input.Attributes.Add("value", value);
            input.Attributes.Add("id", id);

            if (isChecked)
            {
                input.Attributes.Add("checked", string.Empty);
            }

            if (parentId != null)
            {
                input.Attributes.Add("data-parent-id", parentId);
            }

            input.AddCssClass("form-check-input");

            return input;
        }

        private static TagBuilder CreateLabel(string innerText, string @for)
        {
            var label = new TagBuilder("label");

            label.InnerHtml.Append(innerText);
            label.Attributes.Add("for", @for);
            label.AddCssClass("form-check-label");

            return label;
        }

        private static string GetMarkup(IHtmlContent parent)
        {
            using var writer = new StringWriter();
            parent.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}