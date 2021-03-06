namespace SutekiTmp.Middleware
{
    public static class DemandServiceMiddleware
    {
        public static void DemandService(this IApplicationBuilder app, IServiceCollection _services)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (_services == null) throw new ArgumentNullException(nameof(app));


            app.Map("/ListedServices", builder => builder.Run(async context =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync("<style>.table2_1 table {width:100%;margin:15px 0}.table2_1 th {background-color:#93DAFF;color:#000000}.table2_1,.table2_1 th,.table2_1 td{font-size:0.95em;text-align:center;padding:4px;border:1px solid #c1e9fe;border-collapse:collapse}.table2_1 tr:nth-child(odd){background-color:#dbf2fe;}.table2_1 tr:nth-child(even){background-color:#fdfdfd;}</style>");
                await context.Response.WriteAsync($@"<h3>系統內現有服務共:{_services.Count} 個服務</h3>
                                                            <table class='table2_1'>
                                                                <thead>
                                                                    <tr>
                                                                        <th>服務名稱</th>
                                                                        <th>生命週期</th>
                                                                        <th>Instance</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>");
                foreach (var svc in _services.OrderBy(c => c.ServiceType.Name))
                {
                    await context.Response.WriteAsync("<tr>");
                    await context.Response.WriteAsync($"<td>{svc.ServiceType.Name}</td>");
                    await context.Response.WriteAsync($"<td>{svc.Lifetime}</td>");
                    await context.Response.WriteAsync($"<td>{svc.ImplementationType?.Name}</td>");
                    await context.Response.WriteAsync("</tr>");
                }
                await context.Response.WriteAsync("</tbody></table>");
            }));
        }
    }
}
