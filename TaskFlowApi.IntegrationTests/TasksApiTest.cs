using System.Net;
using TaskFlowApi.Domain.Enum;
using TaskFlowApi.DTOs;
using TaskStatus = TaskFlowApi.Domain.Enum.TaskStatus;

namespace TaskFlowApi.IntegrationTests;

using System.Net.Http.Json;

public class TasksApiTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetTasks_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("api/tasks");
        Assert.True(response.IsSuccessStatusCode);
        var tasks = await response.Content.ReadFromJsonAsync<List<TaskResponse>>();
        Assert.NotNull(tasks);
    }

    [Fact]
    public async Task CreateTask_WithValidPayload_ReturnsCreatedAndTaskDetails()
    {
        // Arrange
        var createdProject =
            await CreateTestProjectAsync("Test Project", "A valid description for testing project creation.");

        var request = new CreateTaskRequest
        {
            Title = "Test Task",
            Description = "A valid description for testing Task creation.",
            Priority = TaskPriority.Low,
            ProjectId = createdProject.Id,
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<TaskResponse>();
        Assert.NotNull(content);
        Assert.Equal(request.Title, content.Title);
        Assert.Equal(request.Description, content.Description);
        Assert.Equal(request.Priority, content.Priority);
        Assert.Equal(request.ProjectId, content.ProjectId);
        Assert.True(content.Id != Guid.Empty);
    }

    [Fact]
    public async Task GetTaskById_WhenTaskExists_ReturnsOkWithTaskDetails()
    {
        // Arrange
        var createdProject =
            await CreateTestProjectAsync("Test Project", "A valid description for testing project creation.");
        var createdTask = await CreateTestTaskAsync("Test Task", "A valid description for testing Task Creation.",
            TaskPriority.Low, createdProject.Id);

        // Act
        var getResponse = await _client.GetAsync($"api/tasks/{createdTask.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var fetchedTask = await getResponse.Content.ReadFromJsonAsync<TaskResponse>();
        Assert.NotNull(fetchedTask);
        Assert.Equal(createdTask.Id, fetchedTask.Id);
        Assert.Equal(createdTask.Title, fetchedTask.Title);
        Assert.Equal(createdTask.Description, fetchedTask.Description);
        Assert.Equal(createdTask.Priority, fetchedTask.Priority);
        Assert.Equal(createdTask.ProjectId, fetchedTask.ProjectId);
    }

    [Fact]
    public async Task GetTaskById_WhenTaskDoesNotExists_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var getResponse = await _client.GetAsync($"api/tasks/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);

    }
    
    [Fact]
    public async Task UpdateTaskById_WhenTaskExists_ReturnsOkWithUpdatedTaskDetails()
    {
        // Arrange
        var createdProject =
            await CreateTestProjectAsync("Test Project", "A valid description for testing project creation.");
        var createdTask = await CreateTestTaskAsync("Test Task", "A valid description for testing Task Creation.",
            TaskPriority.Low, createdProject.Id);


        var updateRequest = new UpdateTaskRequest()
        {
            Title = "Updated task Name",
            Description = "A valid updated description for testing.",
            Priority = TaskPriority.Low,
            ProjectId = createdTask.ProjectId,
        };

        // Act
        var updateResponse = await _client.PutAsJsonAsync($"api/tasks/{createdTask.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updatedTask = await updateResponse.Content.ReadFromJsonAsync<TaskResponse>();
        Assert.NotNull(updatedTask);
        Assert.Equal(createdTask.Id, updatedTask.Id);
        Assert.Equal(updateRequest.Title, updatedTask.Title);
        Assert.Equal(updateRequest.Description, updatedTask.Description);
        Assert.Equal(updateRequest.Priority, updatedTask.Priority);
        Assert.Equal(updateRequest.ProjectId, updatedTask.ProjectId);
    }

    [Fact]
    public async Task UpdateTaskById_WhenTaskStatusInValid_ReturnsBadRequest()
    {
        // Arrange
        var createdProject =
            await CreateTestProjectAsync("Test Project", "A valid description for testing project creation.");
        var createdTask = await CreateTestTaskAsync("Test Task", "A valid description for testing Task Creation.",
            TaskPriority.Low, createdProject.Id);


        var updateRequest = new UpdateTaskRequest()
        {
            Title = "Updated task Name",
            Description = "A valid updated description for testing.",
            Priority = TaskPriority.Low,
            Status = TaskStatus.Done,
            ProjectId = createdTask.ProjectId,
        };

        // Act
        var updateResponse = await _client.PutAsJsonAsync($"api/tasks/{createdTask.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);
    }
    [Fact]
    public async Task DeleteTaskById_WhenTaskExists_ReturnsNoContent()
    {
        // Arrange
        var createdProject =
            await CreateTestProjectAsync("Test Project", "A valid description for testing project creation.");
        var createdTask = await CreateTestTaskAsync("Test Task", "A valid description for testing Task Creation.",
            TaskPriority.Low, createdProject.Id);

        // Act
        var getResponse = await _client.DeleteAsync($"api/tasks/{createdTask.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, getResponse.StatusCode);
        var response = await _client.GetAsync($"api/tasks/{createdTask.Id}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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

    private async Task<TaskResponse> CreateTestTaskAsync(string title, string description, TaskPriority priority,
        Guid projectId)
    {
        var request = new CreateTaskRequest()
            {Title = title, Description = description, Priority = priority, ProjectId = projectId};
        var response = await _client.PostAsJsonAsync("api/tasks", request);
        response.EnsureSuccessStatusCode();

        var task = await response.Content.ReadFromJsonAsync<TaskResponse>();
        return task ?? throw new InvalidOperationException("Failed to deserialize created task.");
    }

    #endregion
}