using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.DataAnalysis;

public static class TfidfAlgorithm
{
    private static double CalculateTf(TextObject document, string term)
    {
        var termFrequency =
            document.ProcessedContent.Count(word => word.Equals(term, StringComparison.OrdinalIgnoreCase));
        var totalWords = document.ProcessedContent.Length;
        return (double)termFrequency / totalWords;
    }

    private static double CalculateIdf(IReadOnlyCollection<TextObject> documents, string term)
    {
        var documentCount =
            documents.Count(doc => doc.ProcessedContent.Contains(term, StringComparer.OrdinalIgnoreCase));
        return Math.Log((double)documents.Count / (1 + documentCount));
    }

    private static double CalculateTfidf(TextObject document, List<TextObject> documents, string term)
    {
        var tf = CalculateTf(document, term);
        var idf = CalculateIdf(documents, term);
        return tf * idf;
    }

    private static double CalculateCosineSimilarity(double[] vector1, double[] vector2)
    {
        var dotProduct = vector1.Zip(vector2, (a, b) => a * b).Sum();
        var magnitude1 = Math.Sqrt(vector1.Sum(x => x * x));
        var magnitude2 = Math.Sqrt(vector2.Sum(y => y * y));
        return dotProduct / (magnitude1 * magnitude2);
    }

    private static double CompareTexts(TextObject text1, TextObject text2, List<TextObject> allDocuments)
    {
        var allTerms = text1.ProcessedContent.Union(text2.ProcessedContent).ToArray();

        var vector1 = allTerms.Select(term => CalculateTfidf(text1, allDocuments, term)).ToArray();
        var vector2 = allTerms.Select(term => CalculateTfidf(text2, allDocuments, term)).ToArray();

        // Вычисление косинусово расстояния
        var cosineSimilarity = CalculateCosineSimilarity(vector1, vector2);

        // Преобразование косинусово расстояния в процент схожести
        var similarityPercentage = (cosineSimilarity + 1) * 50;

        return similarityPercentage;
    }
}