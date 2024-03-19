using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.ExternalApi;

namespace SmartFeedback.Scripts.DataAnalysis;

public static class Preprocessing
{
    public static async Task<TextObject> Preprocess(TextObject textObject)
    {
        // textObject.ProcessedContend = await TextApi.PreprocessOne(textObject).Split(" ");
        return textObject;
    }
}