// See https://aka.ms/new-console-template for more information
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server;

var retryPolicy = new MethodConfig
{
    Names = { MethodName.Default },
    RetryPolicy = new RetryPolicy
    {
        MaxAttempts = 2,
        InitialBackoff = TimeSpan.FromSeconds(1),
        MaxBackoff = TimeSpan.FromSeconds(5),
        BackoffMultiplier = 1.5,
        RetryableStatusCodes = { StatusCode.Unavailable, StatusCode.DeadlineExceeded }
    }
};


var hedgingPolicy = new MethodConfig
{
    Names = { MethodName.Default },
    HedgingPolicy = new HedgingPolicy
    {
        HedgingDelay = TimeSpan.FromSeconds(30),
        MaxAttempts = 4,
        NonFatalStatusCodes = { StatusCode.Unavailable, }
    }
};


var channel = GrpcChannel.ForAddress("https://localhost:5000", new GrpcChannelOptions
{
    ServiceConfig = new ServiceConfig
    {
        MethodConfigs = { retryPolicy }
    }
});




#region ClientSide Load Balancing

//var factory = new StaticResolverFactory(addr => new[]
//{
//    new BalancerAddress("localhost", 5000),
//    new BalancerAddress("localhost",5002)
//});

//var services = new ServiceCollection();
//services.AddSingleton<ResolverFactory>(factory);

//var channel = GrpcChannel.ForAddress(
//    "static:///localhost",
//    new GrpcChannelOptions
//    {
//        Credentials = ChannelCredentials.Insecure,
//        ServiceConfig = new ServiceConfig
//        {
//            LoadBalancingConfigs = { new RoundRobinConfig() }
//        },
//        ServiceProvider = services.BuildServiceProvider()
//    });

////var services = new ServiceCollection();
////services.AddSingleton<ResolverFactory>(new DnsResolverFactory(refreshInterval: TimeSpan.FromSeconds(30)));

//#region actualcall
//Request request = new Request() { ContentValue = "Techorama" };


//var client = new Greeter.GreeterClient(channel);
//var reply = client.SayHello(request, options: new CallOptions() { });
//Console.WriteLine(reply.Message);
//#endregion

var client = new Greeter.GreeterClient(channel);
var reply = client.SayHello(request, options: new CallOptions() { });
Console.WriteLine(reply.Message);

#endregion