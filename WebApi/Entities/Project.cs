namespace WebApi.Entities;

public class Project
{
    public int Id { get; set; }
    public ProjectStatus Status { get; set; }
    public string Title { get; set; }
    public string Responsible { get; set; }
    public List<UserStory> UserStories { get; set; }
}