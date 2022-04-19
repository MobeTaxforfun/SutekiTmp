using Microsoft.AspNetCore.Http.Features;
using Microsoft.IO;
using SutekiTmp.Domain.Common.Attributes;
using System.Text;

namespace SutekiTmp.Middleware
{
    public class ReqRespLogMiddleware
    {
        // Todo list 之後寫進 Serilog 中
        private readonly RequestDelegate _next;
        private readonly ILogger<ReqRespLogMiddleware> _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        public ReqRespLogMiddleware(RequestDelegate next, ILogger<ReqRespLogMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //先找 endpoint
                var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
                var attribute = endpoint?.Metadata.GetMetadata<LoggedAttribute>();

                if (attribute != null)
                {
                    _logger.LogInformation($"{attribute.ControllerTag},{attribute.ActionTag}");
                    //攔截Request
                    await RequestLogger(context.Request);

                    await _next(context);

                    //攔截Response
                    await ResponseLogger(context);
                }
                else
                {
                    await _next(context);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
            }
        }

        private async Task ResponseLogger(HttpContext context)
        {
            _logger.LogError(
        $@" Http Request Information:{Environment.NewLine} 
                Schema:{context.Request.Scheme}
                Host: {context.Request.Host.ToUriComponent()} 
                Path: {context.Request.Path}
                QueryString: {context.Request.QueryString}
                ResponseHeader: {GetHeaders(context.Response.Headers)} 
                ResponseStatus: {context.Response.StatusCode}"
                );
        }

        private async Task RequestLogger(HttpRequest httpRequest)
        {
            //使用EnableBuffering 才可以多次讀取 Request

            httpRequest.EnableBuffering();
            // 創建 MemorySteam 暫存空間
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            // 將 Body 複製至暫存空間 
            await httpRequest.Body.CopyToAsync(requestStream);

            _logger.LogInformation(
$@" Http Request Information:{Environment.NewLine} 
    Schema:{httpRequest.Scheme}
    Host: {httpRequest.Host}
    Path: {httpRequest.Path} 
    ResponseHeader: {GetHeaders(httpRequest.Headers)}
    QueryString: {httpRequest.QueryString} 
    Request Body: {ReadStreamInChunks(requestStream)}"
    );
            httpRequest.Body.Position = 0;
        }

        private static string GetHeaders(IHeaderDictionary headers)
        {
            var headerStr = new StringBuilder();
            foreach (var header in headers)
            {
                headerStr.Append($@"    {header.Key}: {header.Value}" + "\n");
            }

            return headerStr.ToString();
        }

        /// <summary>
        /// Stream 轉字串
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                                                   0,
                                                   readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);
            return textWriter.ToString();
        }
    }
}
