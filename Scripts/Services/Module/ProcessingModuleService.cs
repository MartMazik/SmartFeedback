using System.Diagnostics;
using System.Text;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;
using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services.Module;

public class ProcessingModuleService : IProcessingModuleService
{
    private readonly IMongoCollection<TextObject> _texts;
    private readonly IMongoCollection<Project> _projects;
    private readonly IMongoCollection<TextGroup> _textGroups;
    private readonly IHttpClientFactory _httpClientFactory;

    public ProcessingModuleService(IMongoDatabase database, IHttpClientFactory httpClientFactory)
    {
        _texts = database.GetCollection<TextObject>("text_object");
        _projects = database.GetCollection<Project>("project");
        _textGroups = database.GetCollection<TextGroup>("text_group");
        _httpClientFactory = httpClientFactory;
    }

    public async Task<TextObject?> PreprocessingOne(TextObject text)
    {
        Logger.Log("PreprocessingModuleService.PreprocessingOne", "Start-preprocessing one text");

        try
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var httpClient = _httpClientFactory.CreateClient("PythonModuleClient");
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
            var project = await _projects.Find(p => p.Id == text.ProjectId && !p.IsDeleted).FirstOrDefaultAsync();
            if (project == null) return null;
            var projectModel = new ProjectModel(project);
            var textModel = new TextObjectModel(text);

            var requestData = new
            {
                text_object_model = textModel,
                project_model = projectModel
            };

            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/preprocessing/one")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json")
            }, cancellationTokenSource.Token);

            var processedTextModel = await response.Content.ReadFromJsonAsync<TextObjectModel>();
            if (processedTextModel == null) return null;

            text.ProcessedContent = processedTextModel.ProcessedContent;

            Logger.Log("PreprocessingModuleService.PreprocessingOne", "End-preprocessing one text");

            return text;
        }
        catch (Exception ex)
        {
            Logger.Log("PreprocessingModuleService.PreprocessingOne", $"Error: {ex.Message}");
            return null;
        }
    }

    public async Task<List<TextObject>> PreprocessingFew(List<TextObject> texts)
    {
        Logger.Log("PreprocessingModuleService.PreprocessingFew", "Start-preprocessing few texts");

        try
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var httpClient = _httpClientFactory.CreateClient("PythonModuleClient");
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
            var project = await _projects.Find(p => p.Id == texts[0].ProjectId && !p.IsDeleted)
                .FirstOrDefaultAsync();
            if (project == null) return [];
            var projectModel = new ProjectModel(project);

            var textModels = texts.Select(t => new TextObjectModel(t)).ToList();

            var requestData = new
            {
                text_object_models = textModels,
                project_model = projectModel
            };

            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/preprocessing/few")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8,
                    "application/json")
            }, cancellationTokenSource.Token);

            var processedTextModels = await response.Content.ReadFromJsonAsync<List<TextObjectModel>>();
            if (processedTextModels == null) return [];

            for (var i = 0; i < texts.Count; i++)
            {
                texts[i].ProcessedContent = processedTextModels[i].ProcessedContent;
            }

            Logger.Log("PreprocessingModuleService.PreprocessingFew", "End-preprocessing few texts");
            return texts;
        }
        catch (Exception ex)
        {
            Logger.Log("PreprocessingModuleService.PreprocessingFew", $"Error: {ex.Message}");
            return [];
        }
    }

    public async Task<bool> ComparisonOne(TextObject text)
    {
        Logger.Log("PreprocessingModuleService.ComparisonOne", "Start-comparison one text");

        try
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var httpClient = _httpClientFactory.CreateClient("PythonModuleClient");
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
            var project = await _projects.Find(p => p.Id == text.ProjectId && !p.IsDeleted).FirstOrDefaultAsync();
            if (project == null) return false;
            var projectModel = new ProjectModel(project);

            var textModel = new TextObjectModel(text);
            var textModels = new List<TextObjectModel> { textModel };

            var textGroups = await _textGroups.Find(tg => tg.ProjectId == text.ProjectId).ToListAsync();
            var textGroupModels = await Task.WhenAll(textGroups.Select(async tg =>
            {
                var centralText = await _texts.Find(t => t.Id == tg.CenterTextId).FirstOrDefaultAsync();
                var analogTexts = await _texts.Find(t => tg.TextIds.Contains(t.Id)).ToListAsync();
                return new TextGroupModel(tg, centralText, analogTexts);
            }));

            var requestData = new
            {
                group_models = textGroupModels,
                text_object_models = textModels,
                project_model = projectModel
            };

            Logger.Log("PreprocessingModuleService.ComparisonFew", $"Start-send request to comparison");

            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/comparison")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8,
                    "application/json")
            }, cancellationTokenSource.Token);
            response.EnsureSuccessStatusCode();

            var changedTextGroupModels = await response.Content.ReadFromJsonAsync<List<TextGroupModel>>();
            if (changedTextGroupModels == null) return false;

            Logger.Log("PreprocessingModuleService.ComparisonOne", "End-send request to comparison");
            Logger.Log("PreprocessingModuleService.ComparisonOne",
                $"Start-Save changed groups: {changedTextGroupModels.Count}");

            foreach (var changedTextGroupModel in changedTextGroupModels)
            {
                var textGroup = textGroups.Find(tg => tg.Id.ToString() == changedTextGroupModel.Id);
                if (textGroup == null)
                {
                    var newGroup = new TextGroup
                    {
                        ProjectId = ObjectId.Parse(changedTextGroupModel.ProjectId),
                        AnalogCount = changedTextGroupModel.AnalogCount,
                        CoreText = changedTextGroupModel.CoreText,
                        CenterTextId = ObjectId.Parse(changedTextGroupModel.CentralText.Id),
                        TextIds = changedTextGroupModel.AnalogTexts.Select(at => ObjectId.Parse(at.Id)).ToArray()
                    };
                    await _textGroups.InsertOneAsync(newGroup);
                }
                else
                {
                    textGroup.AnalogCount = changedTextGroupModel.AnalogCount;
                    textGroup.CenterTextId = ObjectId.Parse(changedTextGroupModel.CentralText.Id);
                    textGroup.TextIds = changedTextGroupModel.AnalogTexts.Select(at => ObjectId.Parse(at.Id))
                        .ToArray();
                    await _textGroups.ReplaceOneAsync(tg => tg.Id == textGroup.Id, textGroup);
                }
            }

            Logger.Log("PreprocessingModuleService.ComparisonOne", "End-Save changed groups");
            Logger.Log("PreprocessingModuleService.ComparisonOne", "End-comparison one text");

            return true;
        }
        catch (Exception ex)
        {
            Logger.Log("PreprocessingModuleService.ComparisonOne", $"Error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ComparisonFew(List<TextObject> texts)
    {
        Logger.Log("PreprocessingModuleService.ComparisonFew", "Start-comparison few texts");

        try
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var httpClient = _httpClientFactory.CreateClient("PythonModuleClient");
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
            var project = await _projects.Find(p => p.Id == texts[0].ProjectId && !p.IsDeleted)
                .FirstOrDefaultAsync();
            if (project == null) return false;
            var projectModel = new ProjectModel(project);

            var textModels = texts.Select(t => new TextObjectModel(t)).ToList();

            var textGroups = await _textGroups.Find(tg => tg.ProjectId == texts[0].ProjectId)
                .ToListAsync();
            var textGroupModels = await Task.WhenAll(textGroups.Select(async tg =>
            {
                var centralText = await _texts.Find(t => t.Id == tg.CenterTextId)
                    .FirstOrDefaultAsync();
                var analogTexts = await _texts.Find(t => tg.TextIds.Contains(t.Id)).ToListAsync();
                return new TextGroupModel(tg, centralText, analogTexts);
            }));

            var requestData = new
            {
                group_models = textGroupModels,
                text_object_models = textModels,
                project_model = projectModel
            };

            Logger.Log("PreprocessingModuleService.ComparisonFew", $"Start-send request to comparison");

            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/comparison")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8,
                    "application/json")
            }, cancellationTokenSource.Token);
            response.EnsureSuccessStatusCode();

            var changedTextGroupModels = await response.Content.ReadFromJsonAsync<List<TextGroupModel>>();
            Logger.Log("PreprocessingModuleService.ComparisonFew", "End-send request to comparison");
            Logger.Log("PreprocessingModuleService.ComparisonFew", $"Start-Save changed groups");

            Debug.Assert(changedTextGroupModels != null, nameof(changedTextGroupModels) + " != null");
            foreach (var changedTextGroupModel in changedTextGroupModels)
            {
                var textGroup = textGroups.FirstOrDefault(tg => tg.Id.ToString() == changedTextGroupModel.Id);
                if (textGroup == null)
                {
                    var newGroup = new TextGroup
                    {
                        ProjectId = ObjectId.Parse(changedTextGroupModel.ProjectId),
                        AnalogCount = changedTextGroupModel.AnalogCount,
                        CoreText = changedTextGroupModel.CoreText,
                        CenterTextId = ObjectId.Parse(changedTextGroupModel.CentralText.Id),
                        TextIds = changedTextGroupModel.AnalogTexts.Select(at => ObjectId.Parse(at.Id)).ToArray()
                    };
                    await _textGroups.InsertOneAsync(newGroup);
                }
                else
                {
                    textGroup.AnalogCount = changedTextGroupModel.AnalogCount;
                    textGroup.CenterTextId = ObjectId.Parse(changedTextGroupModel.CentralText.Id);
                    textGroup.TextIds = changedTextGroupModel.AnalogTexts.Select(at => ObjectId.Parse(at.Id))
                        .ToArray();
                    await _textGroups.ReplaceOneAsync(tg => tg.Id == textGroup.Id, textGroup);
                }
            }

            Logger.Log("PreprocessingModuleService.ComparisonFew", "End-Save changed groups");
            Logger.Log("PreprocessingModuleService.ComparisonFew", "End-comparison few texts");
            return true;
        }
        catch (Exception ex)
        {
            Logger.Log("PreprocessingModuleService.ComparisonFew", $"Error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ReComparisonInProject(string projectId)
    {
        Logger.Log("PreprocessingModuleService.ReComparisonInProject", "Start-re-comparison in project");

        try
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var httpClient = _httpClientFactory.CreateClient("PythonModuleClient");
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
            var project = await _projects.Find(p => p.Id == ObjectId.Parse(projectId) && !p.IsDeleted).FirstOrDefaultAsync();
            if (project == null) return false;

            var texts = await _texts.Find(t => t.ProjectId == project.Id && !t.IsDeleted).ToListAsync();
            var textModels = texts.Select(t => new TextObjectModel(t)).ToList();

            var requestData = new
            {
                group_models = new List<TextGroupModel>(),
                text_object_models = textModels,
                project_model = new ProjectModel(project)
            };

            Logger.Log("PreprocessingModuleService.ReComparisonInProject", "Start-send request to comparison");

            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/comparison")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json")
            }, cancellationTokenSource.Token);
            response.EnsureSuccessStatusCode();

            var changedTextGroupModels = await response.Content.ReadFromJsonAsync<List<TextGroupModel>>();
            if (changedTextGroupModels == null) return false;

            Logger.Log("PreprocessingModuleService.ReComparisonInProject", "End-send request to comparison");
            Logger.Log("PreprocessingModuleService.ReComparisonInProject", $"Start-Save changed groups: {changedTextGroupModels.Count}");

            await _textGroups.DeleteManyAsync(tg => tg.ProjectId == project.Id);

            foreach (var changedTextGroupModel in changedTextGroupModels)
            {
                var newGroup = new TextGroup
                {
                    ProjectId = ObjectId.Parse(changedTextGroupModel.ProjectId),
                    AnalogCount = changedTextGroupModel.AnalogCount,
                    CoreText = changedTextGroupModel.CoreText,
                    CenterTextId = ObjectId.Parse(changedTextGroupModel.CentralText.Id),
                    TextIds = changedTextGroupModel.AnalogTexts.Select(at => ObjectId.Parse(at.Id)).ToArray()
                };
                await _textGroups.InsertOneAsync(newGroup);
            }

            Logger.Log("PreprocessingModuleService.ReComparisonInProject", "End-Save changed groups");
            Logger.Log("PreprocessingModuleService.ReComparisonInProject", "End-re-comparison in project");

            return true;
        }
        catch (Exception ex)
        {
            Logger.Log("PreprocessingModuleService.ReComparisonInProject", $"Error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ComparisonFew(Project project)
    {
        Logger.Log("PreprocessingModuleService.ComparisonFew", "Start-comparison few texts");

        try
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var httpClient = _httpClientFactory.CreateClient("PythonModuleClient");
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
            var texts = await _texts.Find(t => t.ProjectId == project.Id).ToListAsync();
            var textModels = texts.Select(t => new TextObjectModel(t)).ToList();
            if (textModels.Count == 0) return false;

            var textGroups =
                await _textGroups.Find(tg => tg.ProjectId == project.Id).ToListAsync();
            var textGroupModels = await Task.WhenAll(textGroups.Select(async tg =>
            {
                var centralText = await _texts.Find(t => t.Id == tg.CenterTextId).FirstOrDefaultAsync();
                var analogTexts = await _texts.Find(t => tg.TextIds.Contains(t.Id)).ToListAsync();
                return new TextGroupModel(tg, centralText, analogTexts);
            }));
            if (textGroupModels.Length == 0) return false;

            var requestData = new
            {
                group_models = textGroupModels,
                text_object_models = textModels,
                project_model = new ProjectModel(project)
            };

            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/comparison")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json")
            }, cancellationTokenSource.Token);
            response.EnsureSuccessStatusCode();

            var changedTextGroupModels = await response.Content.ReadFromJsonAsync<List<TextGroupModel>>();
            if (changedTextGroupModels == null) return false;

            foreach (var changedTextGroupModel in changedTextGroupModels)
            {
                var savedTextGroup = await _textGroups.Find(tg => tg.Id.ToString() == changedTextGroupModel.Id).FirstOrDefaultAsync();
                if (savedTextGroup == null)
                {
                    var newGroup = new TextGroup
                    {
                        ProjectId = ObjectId.Parse(changedTextGroupModel.ProjectId),
                        AnalogCount = changedTextGroupModel.AnalogCount,
                        CoreText = changedTextGroupModel.CoreText,
                        CenterTextId = ObjectId.Parse(changedTextGroupModel.CentralText.Id),
                        TextIds = changedTextGroupModel.AnalogTexts.Select(at => ObjectId.Parse(at.Id)).ToArray()
                    };
                    await _textGroups.InsertOneAsync(newGroup);
                }
                else
                {
                    savedTextGroup.AnalogCount = changedTextGroupModel.AnalogCount;
                    savedTextGroup.CenterTextId = ObjectId.Parse(changedTextGroupModel.CentralText.Id);
                    savedTextGroup.TextIds = changedTextGroupModel.AnalogTexts.Select(at => ObjectId.Parse(at.Id)).ToArray();
                    await _textGroups.ReplaceOneAsync(tg => tg.Id == savedTextGroup.Id, savedTextGroup);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Logger.Log("PreprocessingModuleService.ComparisonFew", $"Error: {ex.Message}");
            return false;
        }
    }

    public async Task<List<TextGroupModel>> SearchText(string searchText, string projectId)
    {
        Logger.Log("PreprocessingModuleService.SearchText", "Start-search text");

        try
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var httpClient = _httpClientFactory.CreateClient("PythonModuleClient");
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
            var project = await _projects.Find(p => p.Id == ObjectId.Parse(projectId) && !p.IsDeleted).FirstOrDefaultAsync();

            var textGroups = await _textGroups.Find(tg => tg.ProjectId == project.Id).ToListAsync();
            var textGroupModels = await Task.WhenAll(textGroups.Select(async tg =>
            {
                var centralText = await _texts.Find(t => t.Id == tg.CenterTextId).FirstOrDefaultAsync();
                return new TextGroupModel(tg, centralText);
            }));
            var projectModel = new ProjectModel(project);
                
            var requestDate = new
            {
                text_group_models = textGroupModels,
                project_model = projectModel
            };

            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"/search-in-text?search_line={searchText}")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestDate), Encoding.UTF8, "application/json")
            }, cancellationTokenSource.Token);

            var foundTextGroups = await response.Content.ReadFromJsonAsync<List<TextGroupModel>>();

            Logger.Log("PreprocessingModuleService.SearchText", "End-search text");

            return foundTextGroups ?? [];
        }
        catch (Exception ex)
        {
            Logger.Log("PreprocessingModuleService.SearchText", $"Error: {ex.Message}");
            return [];
        }
    }

    public async Task<List<Project>> SearchProject(string searchText)
    {
        Logger.Log("PreprocessingModuleService.SearchProject", "Start-search project");

        try
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var httpClient = _httpClientFactory.CreateClient("PythonModuleClient");
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
            var projects = await _projects.Find(p => !p.IsDeleted && !p.IsDeleted).ToListAsync();
            var projectModels = projects.Select(p => new ProjectModel(p)).ToList();

            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"/search-in-project?search_line={searchText}")
            {
                Content = new StringContent(JsonSerializer.Serialize(projectModels), Encoding.UTF8, "application/json")
            }, cancellationTokenSource.Token);

            var foundProjects = await response.Content.ReadFromJsonAsync<List<ProjectModel>>();
            if (foundProjects == null) return [];

            var projectsList = new List<Project>();
            foreach (var projectModel in foundProjects)
            {
                var project = await _projects.Find(p => p.Id == ObjectId.Parse(projectModel.Id)).FirstOrDefaultAsync();
                if (project != null) projectsList.Add(project);
            }

            Logger.Log("PreprocessingModuleService.SearchProject", "End-search project");
            return projectsList;
        }
        catch (Exception ex)
        {
            Logger.Log("PreprocessingModuleService.SearchProject", $"Error: {ex.Message}");
            return [];
        }
    }
}