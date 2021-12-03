using System.Collections.Generic;

namespace LINGYUN.Abp.WorkflowCore
{
    public class WorkflowFormData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public object Value { get; set; }
        public IEnumerable<object> Styles { get; set; }
        public int? MaxLength { get; set; }
        public int? MinLength { get; set; }
        public IEnumerable<object> Items { get; set; }
        public IEnumerable<object> Rules { get; set; }
        public WorkflowFormData()
        {
            Styles = new List<object>();
            Items = new List<object>();
            Rules =new List<object>();
        }
    }
}
