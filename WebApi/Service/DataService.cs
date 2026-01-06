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
        
        if(project == null)
        {
            throw new Exception($"Project with id {projectId} not found");
        }

        return Task.FromResult(project);
    }

    public Task<List<Project>> GetManyProjects(ProjectStatus? status, string? responsible)
    {
        IEnumerable<Project> projects = _projects;

        if (status.HasValue)
        {
            projects = projects.Where(p => p.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(responsible))
        {
            projects = projects.Where(p => p.Responsible.Contains(responsible, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(projects.ToList());
    }
    
    private void CreateDummyData()
    {
        Project project = new Project()
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
                    IsCreature = false,
                    ManaCost = 2.2
                },
                new Card()
                {
                    CardName = "Card2",
                    Id = 2,
                    IsCreature = true,
                    ManaCost = 2.7
                },
                new Card()
                {
                    CardName = "Card3",
                    Id = 3,
                    IsCreature = false,
                    ManaCost = 3.5
                }
            ]
        };
    
}