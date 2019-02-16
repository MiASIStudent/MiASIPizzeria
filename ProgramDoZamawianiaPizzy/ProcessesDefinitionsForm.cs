using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramDoZamawianiaPizzy
{
    public partial class ProcessesDefinitionsForm : Form
    {
        public ProcessesDefinitionsForm()
        {
            InitializeComponent();
        }

        private void ProcessesDefinitionsForm_Load(object sender, EventArgs e)
        {
            foreach(var def in this.processDefinitions)
            {
                this.listBox1.Items.Add(def);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var loggedForm = new ProcessForm(activitiClient, (Models.ProcessDefinition)this.listBox1.SelectedItem);
            loggedForm.Show();             
        }
    }
}
