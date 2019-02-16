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
    public partial class TasksForm : Form
    {
        public TasksForm()
        {
            InitializeComponent();
        }

        Models.FormData newForm = new Models.FormData();

        private async void button1_Click(object sender, EventArgs e)
        {

            
            var selected = (Models.ActivitiTask)this.listBox1.SelectedItem;
            if(this.newForm != null)
            {
                await activitiClient.FillForm(newForm);
            }
            if (selected != null)
                await activitiClient.CompleteTask(selected.Id);
            await loadTasks();
            this.panel1.Controls.Clear();
        }

        private async void TasksForm_Load(object sender, EventArgs e)
        {
            await loadTasks();
        }        

        private async Task loadTasks()
        {
            var tasks = await activitiClient.GetTasksByProcessID(this.processInstance.Id);
            if (tasks == null || listBox1 == null || listBox1.Items == null)
                return;
            
            this.listBox1.Items.Clear();
            foreach (var t in tasks)
            {
                if (t == null)
                    return;
                this.listBox1.Items.Add(t);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = (Models.ActivitiTask)this.listBox1.SelectedItem;
            this.panel1.Controls.Clear();
            if (selected != null && selected.Form != null && selected.Form.FormProperties != null)
            {
                newForm = new Models.FormData();
                newForm.TaskId = selected.Form.TaskId;
                newForm.Properties = new List<Models.NewFormProperty>();
                int y = 0;
                foreach (var prop in selected.Form.FormProperties)
                {                                        
                    if(prop.Type == "enum")
                    {
                        AddDropdown(prop, selected.Form, y);
                    } else if (prop.Type == "string" || prop.Type == "long")
                    {
                        AddInput(prop, y);
                    }
                    y += 25;                    
                }
            }            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AddInput(Models.FormProperty formProperty, int y)
        {
            var label = new Label();
            label.Text = formProperty.Name;
            label.Location = new Point(0, y);
          
            var textBox = new TextBox();
            textBox.Location = new Point(150, y);
            textBox.Size = new System.Drawing.Size(319, 21);
            textBox.TabIndex = 2;
            
            textBox.TextChanged += new EventHandler(delegate (Object o, EventArgs a)
            {

                var value = textBox.Text;
                var property = new Models.NewFormProperty()
                {
                    Id = formProperty.Id,
                    Value = value
                };
                try
                {
                    var find = this.newForm.Properties.Find(e => e.Id == property.Id);
                    find.Value = property.Value;
                }
                catch (Exception e)
                {
                    this.newForm.Properties.Add(property);
                }

            });
            this.panel1.Controls.Add(textBox);
            this.panel1.Controls.Add(label);
        }

        private void AddDropdown(Models.FormProperty formProperty, Models.Form form, int y)
        {
            var label = new Label();
            label.Text = formProperty.Name;
            label.Location = new Point(0, y);

            var comboBox = new ComboBox();
            comboBox.Location = new Point(150, y);
            comboBox.Name = formProperty.Id;
            comboBox.Size = new System.Drawing.Size(319, 21);
            comboBox.TabIndex = 2;
            comboBox.Items.Clear();
            foreach(var p in formProperty.EnumValues)
            {
                if (p != null || p.Name != null)
                {
                    comboBox.Items.Add(p);

                }
            }
            comboBox.SelectedIndexChanged += new EventHandler(delegate (Object o, EventArgs a)
            {
                
                var selected = (Models.EnumValue)comboBox.SelectedItem;
                var property = new Models.NewFormProperty() {
                    Id = formProperty.Id,
                    Value = selected.Name
                };
                try {
                    var find = this.newForm.Properties.Find(e => e.Id == property.Id);
                    find.Value = property.Value;
                }
                catch (Exception e)
                {
                    this.newForm.Properties.Add(property);
                }

            });
            this.panel1.Controls.Add(comboBox);
            this.panel1.Controls.Add(label);
        }
    }
}
