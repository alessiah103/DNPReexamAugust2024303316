using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Service;

namespace WebApi.Controllers;

[ApiController]
    [Route("[controller]")]
public class DataController : ControllerBase
{
    private readonly IDataService _dataService;

    public DataController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject([FromBody] Project project)
    {
        var createdProject = await _dataService.AddProject(project);
        return Ok(createdProject);
    }

    [HttpPost("{projectId}/userStories")]
    public async Task<ActionResult<Project>> AddUserStoryToProject(int projectId, [FromBody] UserStory userStory)
    {
        try
        {
            var updatedProfile = await _dataService.AddUserStoryToProject(userStory, projectId);
            return Ok(updatedProfile);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Project>>> GetManyProjects([FromQuery] ProjectStatus? status, [FromQuery] String? responsible)
    {
        var profiles = await _dataService.GetManyProjects(status, responsible);
        return Ok(profiles);
    }
    
    [HttpGet("{deckId}/cards")]
    public async Task<ActionResult<List<UserStory>>> GetSingle(int projectId)
    {
        try
        {
            var userStory = await _dataService.GetSingle(projectId);
            return Ok(userStory);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
        
    }
}