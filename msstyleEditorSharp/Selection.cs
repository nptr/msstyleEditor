using libmsstyle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msstyleEditor
{
    class Selection
    {
        public StyleClass Class { get; set; }
        public StylePart Part { get; set; }
        public StyleState State { get; set; }
        public StyleProperty Property { get; set; }

        public int ResourceId { get; set; }
    }
}
