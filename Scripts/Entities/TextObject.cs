using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartFeedback.Scripts.Entities
{
    public class TextObject
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public bool IsDeleted { get; set; }

        public string Content { get; set; } = "";

        public string TechnicalText { get; set; } = "";

        [BsonElement("Project")]
        public Project Project { get; set; }

        public int AnalogCount { get; set; }

        public int UserRatingCount { get; set; }

        public int RatingSum { get; set; }

        public TextObject()
        {
        }

        public TextObject(string content, Project project)
        {
            Content = content;
            Project = project;
        }
    }
}