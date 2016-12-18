using BlueCarGpsLib.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueCarGpsLib.Model.Search
{
    public class PVCSearchModel
    {
        public string name { get; set; }
        public DateTime? shipDateFrom { get; set; }
        public DateTime? shipDateTo { get; set; }
        public string peNum { get; set; }
        public string brand { get; set; }
        public string batchNo { get; set; }
        public DateTime? startTimeFrom { get; set; }
        public DateTime? startTimeTo { get; set; }
        public DateTime? endTimeFrom { get; set; }
        public DateTime? endTimeTo { get; set; }
        public DateTime? createdAtFrom { get; set; }
        public DateTime? createdAtTo { get; set; }
    }
}
