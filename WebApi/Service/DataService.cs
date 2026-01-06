using WebApi.Entities;

namespace WebApi.Service;

public class DataService : IDataService
{
    private List<Project> _projects;

    public DataService()
    {
        _projects = new List<Project>();

        CreateDummyData();
    }

    public Task<Project> AddProject(Project project)
    {
        project.Id = _projects.Any()
            ? _projects.Max(p => p.Id)
              + 1
            : 1;
        _projects.Add(project);
        return Task.FromResult(project);
    }

    public Task<Project> AddUserStoryToProject(UserStory userStory, int projectId)
    {
        Project? project = _projects.FirstOrDefault(p => p.Id == projectId);

        if (project == null)
        {
            throw new Exception($"Project with id {projectId} not found.");
        }

        project.UserStories ??= new List<UserStory>(); //prevents crash

        int newUserStoriesId = _projects.SelectMany(p => p.UserStories)
            .Any()
            ? _projects.SelectMany(p => p.UserStories)
                .Max(u => u.Id) + 1
            : 1;

        userStory.Id = newUserStoriesId;

        project.UserStories.Add(userStory);

        return Task.FromResult(project);
    }

    public Task<Project> GetSingle(int projectId)
    {
        Project? project = _projects.FirstOrDefault(p => p.Id == projectId);

        if (project == null)
        {
            throw new Exception($"Project with id {projectId} not found");
        }

        return Task.FromResult(project);
    }

    public Task<List<Project>> GetManyProjects(ProjectStatus? status, string? responsible)
    {
        IEnumerable<Project> projects = _projects;

        // filter by status if provided
        if (status.HasValue)
        {
            projects = projects.Where(p => p.Status == status.Value);
        }

        // filter by responsible if provided
        if (!string.IsNullOrWhiteSpace(responsible))
        {
            projects = projects.Where(p =>
                p.Responsible.Contains(responsible, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(projects.ToList());
    }

    private void CreateDummyData()
    {
        Project project1 = new Project()
        {
            Id = 1,
            Title = "Lola",
            Status = ProjectStatus.Completed,
            Responsible = "Resp1",
            UserStories =
            [
                new UserStory()
                {

                    Id = 1,
                    Description = "Description1",
                    Estimate = 2.1
                },
                new UserStory()
                {

                    Id = 2,
                    Description = "Description2",
                    Estimate = 2.3
                },
                new UserStory()
                {

                    Id = 3,
                    Description = "Description3",
                    Estimate = 2.0
                },
            ]
        };
        Project project2 = new Project()
        {
            Id = 2,
            Title = "Project2",
            Status = ProjectStatus.InProgress,
            Responsible = "Resp2",
            UserStories =
            [
                new UserStory()
                {
                    Id = 4,
                    Description = "Description4",
                    Estimate = 2.1
                },
                new UserStory()
                {
                    Id = 5,
                    Description = "Description5",
                    Estimate = 2.3
                },
                new UserStory()
                {
                    Id = 6,
                    Description = "Description6",
                    Estimate = 2.0
                },
            ]
        };
        _projects.AddRange(new List<Project>()
        {
            project2, project1
        });
    }
}
    
    