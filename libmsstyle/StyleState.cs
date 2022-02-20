using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libmsstyle
{
    public class StyleState
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        public List<StyleProperty> Properties { get; set; }

        public StyleState()
        {
            Properties = new List<StyleProperty>();
        }
    }
}
