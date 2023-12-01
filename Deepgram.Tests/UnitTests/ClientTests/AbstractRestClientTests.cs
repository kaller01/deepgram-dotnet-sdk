﻿using Deepgram.Models;

namespace Deepgram.Tests.UnitTests.ClientTests;


public class AbstractRestfulClientTests
{

    IHttpClientFactory _httpClientFactory;
    string _apiKey;

    [SetUp]
    public void Setup()
    {
        _apiKey = new Faker().Random.Guid().ToString();
        _httpClientFactory = Substitute.For<IHttpClientFactory>();
    }

    [Test]
    public async Task GetAsync_Should_Return_ExpectedResult_On_SuccessfulResponse()
    {
        // Arrange        
        var expectedResponse = new AutoFaker<GetProjectsResponse>().Generate();
        var httpClient = MockHttpClient.CreateHttpClientWithResult(expectedResponse, HttpStatusCode.OK);
        _httpClientFactory.CreateClient(Constants.HTTPCLIENT_NAME).Returns(httpClient);
        var client = new ConcreteRestClient(_apiKey, _httpClientFactory);

        // Act
        var result = await client.GetAsync<GetProjectsResponse>(Constants.PROJECTS);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<GetProjectsResponse>();
            result.Should().BeEquivalentTo(expectedResponse);
        };
    }

    [Test]
    public void GetAsync_Should_ThrowsException_On_UnsuccessfulResponse()
    {
        // Arrange       
        var expectedResponse = new AutoFaker<GetProjectsResponse>().Generate();
        var httpClient = MockHttpClient.CreateHttpClientWithResult(expectedResponse, HttpStatusCode.BadRequest);
        _httpClientFactory.CreateClient(Constants.HTTPCLIENT_NAME).Returns(httpClient);
        var client = new ConcreteRestClient(_apiKey, _httpClientFactory) { };

        // Act & Assert
        client.Invoking(y => y.GetAsync<GetProjectsResponse>(Constants.PROJECTS))
             .Should().ThrowAsync<HttpRequestException>();

    }

    [Test]
    public async Task GetAsync_With_Id_Should_Return_ExpectedResult_On_SuccessfulResponse()
    {
        // Arrange    
        var expectedResponse = new AutoFaker<GetProjectResponse>().Generate();
        expectedResponse.ProjectId = "1";
        var httpClient = MockHttpClient.CreateHttpClientWithResult(expectedResponse, HttpStatusCode.OK);
        _httpClientFactory.CreateClient(Constants.HTTPCLIENT_NAME).Returns(httpClient);
        var client = new ConcreteRestClient(_apiKey, _httpClientFactory) { };

        // Act
        var result = await client.GetAsync<GetProjectResponse>($"{Constants.PROJECTS}/1");

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<GetProjectResponse>();
            result.ProjectId.Should().Be(expectedResponse.ProjectId);
        };
    }


    [Test]
    public async Task PostAsync_Should_Return_Response_Model_With_Schema_And_No_Body()
    {
        // Arrange      
        var expectedResponse = new AutoFaker<CreateProjectKeyResponse>().Generate();
        var httpClient = MockHttpClient.CreateHttpClientWithResult(expectedResponse, HttpStatusCode.OK);
        _httpClientFactory.CreateClient(Constants.HTTPCLIENT_NAME).Returns(httpClient);
        var client = new ConcreteRestClient(_apiKey, _httpClientFactory) { };

        // Act
        var result = await client.PostAsync<CreateProjectKeyResponse>(Constants.PROJECTS, new StringContent(string.Empty));

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<CreateProjectKeyResponse>();
            result.ApiKeyId.Should().Be(expectedResponse.ApiKeyId);
        };
    }

    [Test]
    public async Task PostAsync_Should_Return_Response_Model_With_Schema_And_BodyAsync()
    {
        // Arrange       
        var expectedResponse = new AutoFaker<SyncPrerecordedResponse>().Generate();
        var httpClient = MockHttpClient.CreateHttpClientWithResult(expectedResponse, HttpStatusCode.OK);
        _httpClientFactory.CreateClient(Constants.HTTPCLIENT_NAME).Returns(httpClient);
        var client = new ConcreteRestClient(_apiKey, _httpClientFactory) { };

        // Act
        var result = await client.PostAsync<SyncPrerecordedResponse>(Constants.PROJECTS, new StringContent(""));

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<SyncPrerecordedResponse>();
        };
    }

    [Test]
    public void PostAsync_Should_Throw_Exception_On_UnsuccessfulResponse()
    {
        // Arrange       
        var expectedResponse = new AutoFaker<SyncPrerecordedResponse>().Generate();
        var httpClient = MockHttpClient.CreateHttpClientWithResult(expectedResponse, HttpStatusCode.BadRequest);
        _httpClientFactory.CreateClient(Constants.HTTPCLIENT_NAME).Returns(httpClient);
        var client = new ConcreteRestClient(_apiKey, _httpClientFactory) { };

        // Act & Assert      

        client.Invoking(y => y.PostAsync<GetProjectsResponse>(Constants.PROJECTS, new StringContent("")))
             .Should().ThrowAsync<HttpRequestException>();

    }

    [Test]
    public void Delete_Should_Return_Nothing_On_SuccessfulResponse()
    {
        // Arrange   
        var httpClient = MockHttpClient.CreateHttpClientWithResult(new VoidResponse(), HttpStatusCode.OK);
        _httpClientFactory.CreateClient(Constants.HTTPCLIENT_NAME).Returns(httpClient);
        var client = new ConcreteRestClient(_apiKey, _httpClientFactory) { };

        // Act & Assert
        client.Invoking(y => y.Delete($"{Constants.PROJECTS}/1"))
            .Should().NotThrowAsync();
    }


    [Test]
    public void Delete_Should_ThrowsException_On_Response_Containing_Error()
    {
        // Arrange    
        var httpClient = MockHttpClient.CreateHttpClientWithException(new MessageResponse(), HttpStatusCode.BadRequest);
        _httpClientFactory.CreateClient(Constants.HTTPCLIENT_NAME).Returns(httpClient);
        var client = new ConcreteRestClient(_apiKey, _httpClientFactory) { };

        // Act & Assert
        _ = client.Invoking(y => y.Delete(Constants.PROJECTS))
             .Should().ThrowAsync<HttpRequestException>();

    }

    [Test]
    public async Task DeleteAsync_TResponse_Should_Return_ExpectedResult_On_SuccessfulResponse()
    {
        // Arrange        
        var expectedResponse = new AutoFaker<MessageResponse>().Generate();
        var httpClient = MockHttpClient.CreateHttpClientWithResult(expectedResponse, HttpStatusCode.OK);
        _httpClientFactory.CreateClient(Constants.HTTPCLIENT_NAME).Returns(httpClient);
        var client = new ConcreteRestClient(_apiKey, _httpClientFactory) { };

        // Act
        var result = await client.DeleteAsync<MessageResponse>($"{Constants.PROJECTS}/1");

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<MessageResponse>();
            result.Message.Should().Be(expectedResponse.Message);
        };
    }

    [Test]
    public void DeleteAsync_TResponse_Should_ThrowsException_On_UnsuccessfulResponse()
    {
        // Arrange       
        var expectedResponse = new AutoFaker<MessageResponse>().Generate();
        var httpClient = MockHttpClient.CreateHttpClientWithException(expectedResponse, HttpStatusCode.BadRequest);
        _httpClientFactory.CreateClient(Constants.HTTPCLIENT_NAME).Returns(httpClient);
        var client = new ConcreteRestClient(_apiKey, _httpClientFactory) { };

        // Act & Assert
        client.Invoking(y => y.DeleteAsync<MessageResponse>(Constants.PROJECTS))
             .Should().ThrowAsync<HttpRequestException>();

    }


    [Test]
    public async Task PatchAsync_Should_Return_ExpectedResult_On_SuccessfulResponse()
    {
        // Arrange        
        var expectedResponse = new AutoFaker<MessageResponse>().Generate();
        var httpClient = MockHttpClient.CreateHttpClientWithResult(expectedResponse, HttpStatusCode.OK);
        _httpClientFactory.CreateClient(Constants.HTTPCLIENT_NAME).Returns(httpClient);
        var client = new ConcreteRestClient(_apiKey, _httpClientFactory) { };

        // Act
        var result = await client.PatchAsync<MessageResponse>($"{Constants.PROJECTS}/1", new StringContent(""));

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<MessageResponse>();
            result.Message.Should().Be(expectedResponse.Message);
        };
    }

    [Test]
    public void PatchAsync_TResponse_Should_ThrowsException_On_UnsuccessfulResponse()
    {
        // Arrange       
        var expectedResponse = new AutoFaker<MessageResponse>().Generate();
        var httpClient = MockHttpClient.CreateHttpClientWithException(expectedResponse, HttpStatusCode.BadRequest);
        _httpClientFactory.CreateClient(Constants.HTTPCLIENT_NAME).Returns(httpClient);
        var client = new ConcreteRestClient(_apiKey, _httpClientFactory) { };

        //Act & Assert
        client.Invoking(y => y.PatchAsync<MessageResponse>(Constants.PROJECTS, new StringContent("")))
             .Should().ThrowAsync<HttpRequestException>();

    }

    [Test]
    public async Task PutAsync_Should_Return_ExpectedResult_On_SuccessfulResponse()
    {
        // Arrange
        var expectedResponse = new AutoFaker<MessageResponse>().Generate();
        var httpClient = MockHttpClient.CreateHttpClientWithResult(expectedResponse, HttpStatusCode.OK);
        _httpClientFactory.CreateClient(Constants.HTTPCLIENT_NAME).Returns(httpClient);
        var client = new ConcreteRestClient(_apiKey, _httpClientFactory) { };

        // Act
        var result = await client.PutAsync<MessageResponse>($"{Constants.PROJECTS}/1", new StringContent(""));

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<MessageResponse>();
            result.Message.Should().Be(expectedResponse.Message);
        };
    }

    [Test]
    public void PutAsync_TResponse_Should_ThrowsException_On_UnsuccessfulResponse()
    {
        // Arrange       
        var httpClient = MockHttpClient.CreateHttpClientWithException(new MessageResponse(), HttpStatusCode.BadRequest);
        _httpClientFactory.CreateClient(Constants.HTTPCLIENT_NAME).Returns(httpClient);
        var client = new ConcreteRestClient(_apiKey, _httpClientFactory) { };

        // Act & Assert
        client.Invoking(y => y.PutAsync<MessageResponse>(Constants.PROJECTS, new StringContent("")))
             .Should().ThrowAsync<HttpRequestException>();
    }

    [Test]
    public void CreatePayload_Should_Return_StringContent()
    {
        //Arrange       
        var project = new AutoFaker<Project>().Generate();

        //Act
        var result = AbstractRestClient.CreatePayload(project);

        //Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<StringContent>();
        }
    }

    [Test]
    public void CreateStreamPayload_Should_Return_HttpContent()
    {
        //Arrange 
        var source = System.Text.Encoding.ASCII.GetBytes("Acme Unit Testing");
        var stream = new MemoryStream(source);

        //Act
        var result = AbstractRestClient.CreateStreamPayload(stream);

        //Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<HttpContent>();
        }
    }
}
