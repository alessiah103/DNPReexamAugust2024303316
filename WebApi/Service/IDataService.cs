using WebApi.Entities;

namespace WebApi.Service;

public interface IDataService
{
    public Task<Project> AddProject(Project project);
    public Task<Project> AddUserStoryToProject(UserStory userStory, int projectId);
    public Task<Project> GetSingle(int projectIdId);
    public Task<List<Project>> GetManyProjects(ProjectStatus? status, string? responsible);
    
}