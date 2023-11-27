using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartFeedback.Scripts.Entities
{
    public class Project
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }

        public Project()
        {
        }

        public Project(string name)
        {
            Name = name;
        }
    }
}