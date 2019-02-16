using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramDoZamawianiaPizzy.Models
{
    public class ProcessInstance
    {
        public string Id;
        public string ActivityId;
        public string Completed;
        public string ProcessDefinitionId;

        public override string ToString()
        {
            return Id;
        }
    }
}
