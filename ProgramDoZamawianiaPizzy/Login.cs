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
    public partial class Login : Form
    {
        ActivitiClient activitiClient = new ActivitiClient();
        public Login()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var username = this.username.Text;
            var password = this.password.Text;
            activitiClient.SetCredentials(username, password);
            var definitions = await activitiClient.GetProcessDefinitions();
            if(definitions == null)
            {
                Microsoft.VisualBasic.Interaction.MsgBox("Niepoprawne dane logowania", Microsoft.VisualBasic.MsgBoxStyle.Critical);
                this.username.Clear();
                this.password.Clear();
            }
            else
            {
                var loggedForm = new ProcessesDefinitionsForm(activitiClient, definitions);
                loggedForm.Show();
                this.Hide();
            }            
        }
    }
}
