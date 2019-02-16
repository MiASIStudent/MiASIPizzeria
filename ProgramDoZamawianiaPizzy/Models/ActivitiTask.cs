using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramDoZamawianiaPizzy.Models
{
    public class ActivitiTask
    {
        public String Id;
        public String Name;
        public String ProcessDefinitionId;
        public String ProcessInstanceId;
        public Form Form;

        public override string ToString()
        {
            return Name ?? "Unknown";
        }

    }

   
}
