using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace SutekiTmp.Middleware
{
    public static class DemandControllerMiddleware
    {
        public static void DemandController(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            Assembly assembly = Assembly.GetExecutingAssembly();

            var ListOfController = assembly.GetTypes()
            .Where(type => typeof(Controller).IsAssignableFrom(type))
            .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
            .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
            .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
            .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            //    var ListOfController = assembly.GetTypes()
            //.Where(type => typeof(Controller).IsAssignableFrom(type))
            //.SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
            //.Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any()).ToList();


            app.Map("/ListControllerInfo", builder => builder.Run(async context =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync("<style>.table2_1 table {width:100%;margin:15px 0}.table2_1 th {background-color:#93DAFF;color:#000000}.table2_1,.table2_1 th,.table2_1 td{font-size:0.95em;text-align:center;padding:4px;border:1px solid #c1e9fe;border-collapse:collapse}.table2_1 tr:nth-child(odd){background-color:#dbf2fe;}.table2_1 tr:nth-child(even){background-color:#fdfdfd;}</style>");
                await context.Response.WriteAsync($@"<h3>系統內Controller:{ListOfController.GroupBy(c => c.Controller).Select(c => c.Key).Count()} 個, Action : {ListOfController.Count()}個。</h3>
                                                            <table class='table2_1'>
                                                                <thead>
                                                                    <tr>
                                                                        <th>Controller</th>
                                                                        <th>Action</th>
                                                                        <th>Attributes</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>");
                foreach (var loc in ListOfController)
                {
                    await context.Response.WriteAsync("<tr>");
                    await context.Response.WriteAsync($"<td>{loc.Controller}</td>");
                    await context.Response.WriteAsync($"<td>{loc.Action}</td>");
                    await context.Response.WriteAsync($"<td>{loc.Attributes}</td>");
                    await context.Response.WriteAsync("</tr>");
                }
                await context.Response.WriteAsync("</tbody></table>");
            }));
        }
    }
}
