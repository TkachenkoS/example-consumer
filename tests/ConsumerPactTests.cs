using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;
using PactNet;
using Consumer;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using PactNet.Matchers;
using FluentAssertions;
using PactNet.Infrastructure.Outputters;
using PactNet.Output.Xunit;
using System.Threading.Tasks;

namespace tests
{
    public class ConsumerPactTests
    {
        private IPactBuilderV3 pact;
        // private readonly int port = 9222;

        private readonly List<Walk> walks;

        public ConsumerPactTests(ITestOutputHelper output)
        {

            walks = new List<Walk>()
            {
                new Walk { Id = 27, Name = "Walk-27",Name1 = "Walk-27", Status = "Pending" }
            };

            var Config = new PactConfig
            {
                PactDir = Path.Join("..", "..", "..", "..", "pacts"),
                Outputters = new List<IOutput> { new XunitOutput(output), new ConsoleOutput() },
                LogLevel = PactLogLevel.Debug
            };

            pact = Pact.V3("pactflow-example-consumer", "pactflow-example-provider", Config).WithHttpInteractions();
        }

        [Fact]
        public async Task RetrieveWalks()
        {
            // Arrange
            pact.UponReceiving("A request to get walks")
                        .Given("walks exist")
                        .WithRequest(HttpMethod.Get, "/walks")
                    .WillRespond()
                    .WithStatus(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithJsonBody(Match.MinType(walks[0],1));

            await pact.VerifyAsync(async ctx =>
            {
                // Act
                var consumer = new WalkClient();
                List<Walk> result = await consumer.GetWalks(ctx.MockServerUri.ToString().TrimEnd('/'));
                // Assert
                result.Should().NotBeNull();
                result.Should().HaveCount(1);
                Assert.Equal(27,result[0].Id);
                Assert.Equal("Walk-27", result[0].Name);
                Assert.Equal("Walk-27", result[0].Name2);
                Assert.Equal("Pending",result[0].Status);
            });
        }
    }
}
