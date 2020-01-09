using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace ServerApp
{
    class Program
    {
        static async void Main(string[] args)
        {
            List<User> users = new List<User>();
            TcpListener Listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 3231);
            const string GIVE = "GIVE";
            const string ADD = "ADD";
            const string UPDATE = "UPDATE";
            const string REMOVE = "REMOVE";
            using (var client = Listener.AcceptTcpClient())
            using (var context = new Context())
            {
                Console.WriteLine("Cоединение открыто");
                using (var stream = client.GetStream())
                {
                    var resultText = string.Empty;
                    while (stream.DataAvailable)
                    {
                        var buffer = new byte[1024];
                        stream.Read(buffer, 0, buffer.Length);
                        resultText += System.Text.Encoding.UTF8.GetString(buffer).TrimEnd('\0');
                    }
                    var response = JsonConvert.DeserializeObject<Response>(resultText);

                    if (response.Action == GIVE)
                    {
                        var data = JsonConvert.SerializeObject(context.Users.ToList());
                        var answer = System.Text.Encoding.UTF8.GetBytes(data);
                        stream.Write(answer, 0, answer.Length);
                    }
                    else if (response.Action == ADD)
                    {
                        var user = new User()
                        {
                            Name = response.Value,
                            Id = users.Count + 1
                            
                        };
                        users.Add(user);
                        context.Users.Add(user);
                        await context.SaveChangesAsync();

                    }
                    else if (response.Action == UPDATE)
                    {

                    }
                    else if (response.Action == REMOVE)
                    {
                        await context.SaveChangesAsync();

                        
                    }
                    //users = users.Where(x => x.IsDeleted == false).ToList();
                }
            }
        }
    }
}
