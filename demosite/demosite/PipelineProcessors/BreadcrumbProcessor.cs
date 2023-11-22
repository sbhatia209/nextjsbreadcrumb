using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Managers;
using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.LayoutService.ItemRendering.Pipelines.GetLayoutServiceContext;

namespace demosite.PipelineProcessors
{
    public class BreadcrumbProcessor : IGetLayoutServiceContextProcessor
    {
        public void Process(GetLayoutServiceContextArgs args)
        {
            if (Context.Site == null || Context.Item == null)
            {
                return;
            }

            var result = new JArray();

            ProcessItem(Context.Item, result);

            var sortedAnscestors = this.ProcessRecursively(Context.Item, result).Reverse();

            args.ContextData.Add("Breadcrumbs", sortedAnscestors);
        }

        private JArray ProcessRecursively(Sitecore.Data.Items.Item item, JArray resultArr)
        {
            var partentItem = item.Parent;

            if (partentItem != null)
            {
                ProcessItem(partentItem, resultArr);

                this.ProcessRecursively(item.Parent, resultArr);
            }

            return resultArr;
        }
        private void ProcessItem(Sitecore.Data.Items.Item item, JArray resultArr)
        {
            CheckboxField excludeFromBreadcrumb = (CheckboxField)item.Fields["ExcludeFromBreadcrumb"];

            if (excludeFromBreadcrumb != null && !excludeFromBreadcrumb.Checked)
            {
                var resultJson = new JObject
                {
                    ["pageTitle"] = item.DisplayName != null ? item.DisplayName : item.Name,
                    ["url"] = Sitecore.Links.LinkManager.GetItemUrl(item),
                };

                resultArr.Add(resultJson);
            }
        }
    }
}