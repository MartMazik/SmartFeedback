using Newtonsoft.Json;
using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.ExternalApi;

public static class TextApi
{
    private static readonly HttpClient? Client = ApiClient.GetClient();

    public static async Task<TextObject> PreprocessOne(TextObject textObject)
    {
        var response = await Client.PostAsync("preprocessing/one", new StringContent(textObject.Content));

        var textObjectProcessedContend = JsonConvert
            .DeserializeObject<TextObject>(await response.Content.ReadAsStringAsync())?.ProcessedContent;
        if (textObjectProcessedContend !=
            null)
            textObject.ProcessedContent = textObjectProcessedContend;

        return textObject;
    }

    public static async Task<List<TextObject>> PreprocessFew(List<TextObject> textObjects)
    {
        var response = await Client.PostAsync("preprocessing/few",
            new StringContent(JsonConvert.SerializeObject(textObjects)));

        var textObjectsProcessedContend = JsonConvert
            .DeserializeObject<List<TextObject>>(await response.Content.ReadAsStringAsync());
        if (textObjectsProcessedContend !=
            null)
            textObjects = textObjectsProcessedContend;

        return textObjects;
    }

}