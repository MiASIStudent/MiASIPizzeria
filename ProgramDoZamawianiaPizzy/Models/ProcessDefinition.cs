using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramDoZamawianiaPizzy.Models
{
    public class ProcessDefinition
    {
        public string Id;
        public string Name;
        public string Resource;
        public string DiagramResource;

        public override string ToString()
        {
            if (Name == null)
                return "Unknown";
            return Name;
        }
    }
}
