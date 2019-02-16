using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramDoZamawianiaPizzy.Models
{
    public class EnumValue
    {
        public string Id;
        public string Name;

        public override string ToString()
        {
            return Name ?? "Unknown";
        }
    }
}
