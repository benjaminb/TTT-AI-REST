using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestClientSDKLibrary;
using RestClientSDKLibrary.Models;
using Microsoft.AspNetCore.Http;

namespace FunctionalTestProject
{
    [TestClass]
    public class FunctionTests
    {
        ServiceClientCredentials _serviceClientCredentials;

        RestClientSDKLibraryClient _client;

        [TestInitialize]
        public void Initialize()
        {
            _serviceClientCredentials = new TokenCredentials("FakeTokenValue");

            _client = new RestClientSDKLibraryClient(
                new Uri("https://localhost:44382/"), _serviceClientCredentials);
        }

        [TestMethod]
        public async Task WinnerXTest()
        {
            // Arrange 
            InputPayload InputPayload = new InputPayload()
            {
                Move = 0,
                AzurePlayerSymbol = "O",
                HumanPlayerSymbol = "X",
                GameBoard = new List<string> { "X", "O", "O", "X", "?", "?", "X", "?", "?" },
                Message = "no message"
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ProcessComplexInputWithHttpMessagesAsync(InputPayload);
            OutputPayload response = resultObject.Body as OutputPayload;

            // Assert
            Assert.IsTrue(response.Winner.Equals("X"));
        }

        [TestMethod]
        public async Task WinnerOTest()
        {
            // Arrange 
            InputPayload InputPayload = new InputPayload()
            {
                Move = 7,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "O",
                GameBoard = new List<string> { "X", "O", "X", "X", "O", "?", "?", "O", "?" },
                Message = "no message"
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ProcessComplexInputWithHttpMessagesAsync(InputPayload);
            OutputPayload response = resultObject.Body as OutputPayload;

            // Assert
            Assert.AreEqual(response.Winner, "O");
        }

        [TestMethod]
        public async Task TieTest()
        {
            // Arrange 
            InputPayload InputPayload = new InputPayload()
            {
                Move = 7,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "O",
                GameBoard = new List<string> { "X", "O", "X", "X", "O", "X", "O", "X", "O" },
                Message = "no message"
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ProcessComplexInputWithHttpMessagesAsync(InputPayload);
            OutputPayload response = resultObject.Body as OutputPayload;

            // Assert
            Assert.IsTrue(response.Winner.Equals("tie"));
        }

        [TestMethod]
        public async Task OppositeSymbolTest()
        {
            // Arrange 
            InputPayload InputPayload = new InputPayload()
            {
                Move = 1,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "O",
                GameBoard = new List<string> { "X", "O", "?", "?", "?", "?", "?", "?", "?" },
                Message = "no message"
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ProcessComplexInputWithHttpMessagesAsync(InputPayload);

            // Assert
            Assert.AreEqual(StatusCodes.Status200OK, (int)resultObject.Response.StatusCode);
            OutputPayload response = resultObject.Body as OutputPayload;

            if (response != null)
                Assert.IsTrue(
                    (response.AzurePlayerSymbol == "X" && response.HumanPlayerSymbol == "O")
                    || (response.AzurePlayerSymbol == "O" && response.HumanPlayerSymbol == "X"));
            else
                Assert.Fail("Expected an ExecuteMove response but didn't receive one");

        }

        [TestMethod]
        public async Task InvalidMarkerTest()
        {
            // Arrange 
            InputPayload InputPayload = new InputPayload()
            {
                Move = 1,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "O",
                GameBoard = new List<string> { "!", "O", "?", "?", "?", "?", "?", "?", "?" },
                Message = "no message"
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ProcessComplexInputWithHttpMessagesAsync(InputPayload);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, (int)resultObject.Response.StatusCode);
        }

        // Tests that players can't have the same marker
        [TestMethod]
        public async Task BothPlayersXTest()
        {
            // Arrange 
            InputPayload InputPayload = new InputPayload()
            {
                Move = 1,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "X",
                GameBoard = new List<string> { "!", "O", "?", "?", "?", "?", "?", "?", "?" },
                Message = "no message"
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ProcessComplexInputWithHttpMessagesAsync(InputPayload);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, (int)resultObject.Response.StatusCode);
        }

        // Tests that the endpoint rejects a list of less than 9 markers
        [TestMethod]
        public async Task TooFewMarkersTest()
        {
            // Arrange 
            InputPayload InputPayload = new InputPayload()
            {
                Move = 1,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "O",
                GameBoard = new List<string> { "X", "O" },
                Message = "no message"
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ProcessComplexInputWithHttpMessagesAsync(InputPayload);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, (int)resultObject.Response.StatusCode);
        }

        // Tests that the endpoint rejects a list of more than 9 markers
        [TestMethod]
        public async Task TooManyMarkersTest()
        {
            // Arrange 
            InputPayload InputPayload = new InputPayload()
            {
                Move = 1,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "O",
                GameBoard = new List<string> { "X", "O", "?", "?", "?", "?", "?", "?", "?", "?" },
                Message = "no message"
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ProcessComplexInputWithHttpMessagesAsync(InputPayload);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, (int)resultObject.Response.StatusCode);
        }

        // Tests that the board has a valid number of X's vs. O's
        [TestMethod]
        public async Task ValidNumberOfMarkersTest()
        {
            // Arrange 
            InputPayload InputPayload = new InputPayload()
            {
                Move = 1,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "O",
                GameBoard = new List<string> { "X", "O", "O", "?", "?", "?", "?", "?", "?" },
                Message = "no message"
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ProcessComplexInputWithHttpMessagesAsync(InputPayload);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, (int)resultObject.Response.StatusCode);
        }
    }
}
