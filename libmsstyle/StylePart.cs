using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libmsstyle
{
    public class StylePart
    {
        public int PartId { get; set; }
        public string PartName { get; set; }
        public Dictionary<int, StyleState> States { get; set; }

        public StylePart()
        {
            States = new Dictionary<int, StyleState>();
        }
    }
}
