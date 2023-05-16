
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using Server;
using System.Net.NetworkInformation;
using Unary;



using var channel = GrpcChannel.ForAddress("http://localhost:5000");

var client = new Greeter.GreeterClient(channel);
var cts = new CancellationTokenSource();

Request request = new Request() { ContentValue = "Techorama!" };

Console.WriteLine($"sending: {request.ContentValue}");

var response = client.SayHello(request);

#region Options
//Deadline exceded
//var response1 = await client.SayHelloAsync(
//        request,
//        deadline: DateTime.UtcNow.AddMilliseconds(1),
//        cancellationToken: cts.Token);

#endregion
Console.WriteLine(response.Message);