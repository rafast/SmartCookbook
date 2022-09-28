﻿using FluentAssertions;
using SmartCookbook.Exceptions;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TestsUtility.Requests;
using WebApi.Test.User.V1;
using Xunit;

namespace WebApi.Test.User.Register;

public class UserRegisterTest : ControllerBase
{
    private const string METHOD = "user";
    public UserRegisterTest(SmartCookbookWebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task Validate_Success()
    {
        var request = RegisterUserRequestBuilder.Build();

        var response = await PostRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Validate_Error_Empty_Name()
    {
        var request = RegisterUserRequestBuilder.Build();
        request.Name = string.Empty;

        var response = await PostRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("messages").EnumerateArray();
        errors.Should().ContainSingle().And.Contain(c => c.GetString().Equals(ResourceErrorMessages.EMPTY_USER_NAME));
    }
}
