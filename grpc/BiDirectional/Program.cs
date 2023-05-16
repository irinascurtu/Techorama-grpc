// See https://aka.ms/new-console-template for more information
using Grpc.Core;
using Server;

Console.WriteLine("Hello, World!");
Channel channel = new Channel("127.0.0.1:5000", ChannelCredentials.Insecure);

var client = new Greeter.GreeterClient(channel);
var cts = new CancellationTokenSource();
try
{


    using (var call = client.BiDirectional())
    {
        var responseReaderTask = Task.Run(async () =>
        {
            while (await call.ResponseStream.MoveNext())
            {
                Response message = call.ResponseStream.Current;
                Console.WriteLine("Received " + message.Message);
            }
        });

        var request = new Request();
        for (int i = 0; i < 10; i++)
        {
            request.ContentValue = i.ToString();
            Console.WriteLine("Sending " + request.ContentValue);
            await call.RequestStream.WriteAsync(request);
        }
        await call.RequestStream.CompleteAsync();
        await responseReaderTask;
    }
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
{
    Console.WriteLine("Stream cancelled.");
}