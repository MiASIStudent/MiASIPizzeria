using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using ProgramDoZamawianiaPizzy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProgramDoZamawianiaPizzy
{
    public class ActivitiClient
    {
        private string credentials = "";

        public void SetCredentials(string name, string password)
        {
            this.credentials = name + ":" + password;
        }
        
        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes(this.credentials);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            return httpClient;
        }

        
        private readonly string address = Constants.ADDRESS + "/activiti-rest/service/";
        
        public async Task<bool> CompleteTask(string taskId)
        {
            string postfix = "runtime/tasks/"+taskId;
            using (var httpClient = CreateClient())
            {
                try
                {
                    var content = new StringContent("{\"action\" : \"complete\"}", Encoding.UTF8, "application/json");
                    return httpClient.PostAsync(address + postfix, content).Result.IsSuccessStatusCode;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                return false;
            }
        }

        public async Task<bool> FillForm(FormData formData)
        {
            string postfix = "form/form-data"; 
            using (var httpClient = CreateClient())
            {
                try
                {
                    var serializerSettings = new JsonSerializerSettings();
                    serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    var serialized = JsonConvert.SerializeObject(formData, serializerSettings);
                    var content = new StringContent(serialized, Encoding.UTF8, "application/json");
                    return httpClient.PostAsync(address + postfix, content).Result.IsSuccessStatusCode;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                return false;
            }
        }

        public async Task<List<ProcessInstance>> GetProcessInstances(string processDefinitionId)
        {
            string postfix = "runtime/process-instances";
            using (var httpClient = CreateClient())
            {
                try
                {
                    string responseBody = await httpClient.GetStringAsync(this.address + postfix);
                    List<ProcessInstance> list = JsonConvert.DeserializeObject<RequestData<ProcessInstance>>(responseBody).data;
                    return list.Where(e => e.ProcessDefinitionId == processDefinitionId).ToList();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
            return new List<ProcessInstance>();
        }

        public async Task<List<ProcessDefinition>> GetProcessDefinitions()
        {
            string postfix = "repository/process-definitions";
            using (var httpClient = CreateClient())
            {
                try
                {
                    string responseBody = await httpClient.GetStringAsync(this.address + postfix);
                    List<ProcessDefinition> list = JsonConvert.DeserializeObject<RequestData<ProcessDefinition>>(responseBody).data;              
                    return list;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
            return null;
        }

        public async Task<ProcessInstance> CreateInstance(string processDefinitionId)
        {
            string postfix = "runtime/process-instances";
            using (var httpClient = CreateClient())
            {
                try
                {
                    var content = new StringContent("{\"processDefinitionId\":\"" + processDefinitionId + "\"}", Encoding.UTF8, "application/json");
                    string responseBody = await httpClient.PostAsync(this.address + postfix, content).Result.Content.ReadAsStringAsync();
                    ProcessInstance inst =  JsonConvert.DeserializeObject<ProcessInstance>(responseBody);
                    return inst;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
            return null;
        }
    

        public async Task<List<ActivitiTask>> GetTasksByProcessID(string id)
        {
            string postfix = "runtime/tasks";            
            using (var httpClient = CreateClient())
            {
                try
                {                    
                    string responseBody = await httpClient.GetStringAsync(this.address + postfix);
                    List<ActivitiTask> tasks = JsonConvert.DeserializeObject<RequestData<ActivitiTask>>(responseBody).data;
                    if(tasks == null)
                        return new List<ActivitiTask>();
                    foreach (var task in tasks){
                        task.Form = await GetForm(httpClient, task.Id);
                    }
                    return tasks.Where(e => e.ProcessInstanceId == id).ToList(); ;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
            return new List<ActivitiTask>();
        }

        private async Task<Form> GetForm(HttpClient httpClient, string taskId)
        {
            try
            {
                string postfix = "form/form-data?taskId="+taskId;
                string responseBody = await httpClient.GetStringAsync(this.address + postfix);
                Form form = JsonConvert.DeserializeObject<Form>(responseBody);
                return form;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }        
            return new Form();
        }
    }
}
