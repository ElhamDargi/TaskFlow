using System.Net;
using System.Net.Http.Json;
using TaskFlowApi.DTOs;

namespace TaskFlowApi.IntegrationTests;

public class ProjectsApiTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetProjects_WhenProjectsExist_ReturnsOkWithProjectList()
    {
        // Act
        var response = await _client.GetAsync("api/projects");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var projects = await response.Content.ReadFromJsonAsync<List<ProjectResponse>>();
        Assert.NotNull(projects);
        Assert.NotEmpty(projects);
    }

    [Fact]
    public async Task CreateProject_WithValidPayload_ReturnsCreatedAndProjectDetails()
    {
        // Arrange
        var request = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "A valid description for testing project creation."
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/projects", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(content);
        Assert.Equal(request.Name, content.Name);
        Assert.Equal(request.Description, content.Description);
        Assert.True(content.Id != Guid.Empty);
    }

    [Fact]
    public async Task CreateProject_WithInValidPayload_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateProjectRequest
        {
            Name = "",
            Description = "A valid description for testing project creation."
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/projects", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task GetProjectById_WhenProjectExists_ReturnsOkWithProjectDetails()
    {
        // Arrange
        var createdProject =
            await CreateTestProjectAsync("Test Project", "A valid description for testing project creation.");

        // Act
        var getResponse = await _client.GetAsync($"api/projects/{createdProject.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var fetchedProject = await getResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(fetchedProject);
        Assert.Equal(createdProject.Id, fetchedProject.Id);
        Assert.Equal(createdProject.Name, fetchedProject.Name);
        Assert.Equal(createdProject.Description, fetchedProject.Description);
    }

    [Fact]
    public async Task GetProjectById_WhenProjectDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"api/projects/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProjectById_WhenProjectExists_ReturnsOkWithUpdatedProjectDetails()
    {
        // Arrange
        var createdProject =
            await CreateTestProjectAsync("Test Project", "A valid description for testing project creation.");

        var updateRequest = new UpdateProjectRequest
        {
            Name = "Updated Project Name",
            Description = "A valid updated description for testing."
        };

        // Act
        var updateResponse = await _client.PutAsJsonAsync($"api/projects/{createdProject.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updatedProject = await updateResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(updatedProject);
        Assert.Equal(createdProject.Id, updatedProject.Id);
        Assert.Equal(updateRequest.Name, updatedProject.Name);
        Assert.Equal(updateRequest.Description, updatedProject.Description);
    }

    [Fact]
    public async Task UpdateProjectById_WhenProjectDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        var updateRequest = new UpdateProjectRequest
        {
            Name = "Updated Project Name",
            Description = "A valid updated description for testing."
        };

        // Act
        var updateResponse = await _client.PutAsJsonAsync($"api/projects/{nonExistentId}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, updateResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteProjectById_WhenProjectExists_ReturnsNoContent()
    {
        // Arrange
        var createdProject =
            await CreateTestProjectAsync("Test Project", "A valid description for testing project creation.");

        // Act
        var getResponse = await _client.DeleteAsync($"api/projects/{createdProject.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, getResponse.StatusCode);
        var response = await _client.GetAsync($"api/projects/{createdProject.Id}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProjectById_WhenProjectDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var getResponse = await _client.DeleteAsync($"api/projects/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    #region Helper Methods

    private async Task<ProjectResponse> CreateTestProjectAsync(string name, string description)
    {
        var request = new CreateProjectRequest {Name = name, Description = description};
        var response = await _client.PostAsJsonAsync("api/projects", request);
        response.EnsureSuccessStatusCode();

        var project = await response.Content.ReadFromJsonAsync<ProjectResponse>();
        return project ?? throw new InvalidOperationException("Failed to deserialize created project.");
    }

    #endregion
}