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
    public partial class ProcessForm : Form
    {
        public ProcessForm()
        {            
            InitializeComponent();
        }
             
        private async void button1_Click(object sender, EventArgs e)
        {
            await activitiClient.CreateInstance(this.processDefinition.Id);
            await loadInstances();
        }

        private async Task loadInstances()
        {
            var instances = await activitiClient.GetProcessInstances(this.processDefinition.Id);
            this.listBox1.Items.Clear();
            foreach (var instance in instances)
            {
                this.listBox1.Items.Add(instance);
            }
        }

        

        private async void ProcessForm_Load_1(object sender, EventArgs e)
        {
            this.label1.Text = this.processDefinition.Name;
            await loadInstances();
        }

        private async void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var taskForm = new TasksForm(activitiClient, (Models.ProcessInstance)this.listBox1.SelectedItem);
            taskForm.Show();
            await loadInstances();
        }
    }
}
