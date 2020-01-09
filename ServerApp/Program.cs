using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Reflection;

namespace ServerApp
{
    class Program
    {
        static  void Main(string[] args)
        {
            List<User> users = new List<User>();
            TcpListener Listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 3231);
            const string GIVE = "GIVE";
            const string ADD = "ADD";
            const string UPDATE = "UPDATE";
            const string REMOVE = "REMOVE";


            using (var context = new Context())
            {
                var data = /*JsonConvert.SerializeObject(context.Users.ToList())*/CreateResult(context.Users.ToList());
                Console.WriteLine(data);
            }

            //using (var client = Listener.AcceptTcpClient())
            //using (var context = new Context())
            //{
            //    var data = /*JsonConvert.SerializeObject(context.Users.ToList())*/CreateResult(context.Users.ToList());
            //    Console.WriteLine(data);
            //    Console.WriteLine("Cоединение открыто");
            //    using (var stream = client.GetStream())
            //    {
            //        var resultText = string.Empty;
            //        while (stream.DataAvailable)
            //        {
            //            var buffer = new byte[1024];
            //            stream.Read(buffer, 0, buffer.Length);
            //            resultText += System.Text.Encoding.UTF8.GetString(buffer).TrimEnd('\0');
            //        }
            //        var response = JsonConvert.DeserializeObject<Response>(resultText);
            //        if (response.Path == "users")
            //        {
            //            if (response.Action == GIVE)
            //            {
            //                //var data = /*JsonConvert.SerializeObject(context.Users.ToList())*/CreateResult(context.Users.ToList());
            //                //var answer = System.Text.Encoding.UTF8.GetBytes(data);
            //                //Console.WriteLine(data);
            //                //stream.Write(answer, 0, answer.Length);
            //            }
            //            else if (response.Action == ADD)
            //            {
            //                var user = new User()
            //                {
            //                    Name = response.Value,
            //                    Id = users.Count + 1

            //                };
            //                users.Add(user);
            //                context.Users.Add(user);
            //                 context.SaveChangesAsync();

            //            }
            //            else if (response.Action == UPDATE)
            //            {
            //                var user = context.Users.FirstOrDefault(x => x.Id == int.Parse(response.Value));
            //                user.Name = response.NewData;
            //                context.Update(user);
            //                 context.SaveChangesAsync();
            //            }
            //            else if (response.Action == REMOVE)
            //            {
            //                var user = context.Users.FirstOrDefault(x => x.Id == int.Parse(response.Value));
            //                user.IsDeleted = true;
            //                context.Update(user);
            //                 context.SaveChangesAsync();
            //            }
            //        }
            //        //users = users.Where(x => x.IsDeleted == false).ToList();
            //    }
            //}
        }

        static string CreateResult(List<User> users)
        {
            var result = new StringBuilder();
            result.Append("[table]");
            result.Append("[header]");

            foreach (var property in users[0].GetType().GetProperties())
            {
                result.Append("[h]");
                result.Append(property.Name);
                result.Append("[/h]");
            }
            result.Append("[/header]");
            result.Append("[data]");
            result.Append("[data]");
            foreach (var user in users)
            {
                result.Append("[d]");
                result.Append(user.IsDeleted + ",");
                result.Append(user.Name + ",");
                result.Append(user.Id);
                result.Append("[/d]");
            }
            result.Append("[/data]");

            result.Append("[/table]");
            return result.ToString();
        }
    }
}
