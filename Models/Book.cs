using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookstoreDemo.Models;

public class Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id{ get; set; }

    [BsonElement("Title")]
    public string Title {get; set; } = null!;
    public string Genre { get; set; } = null!;
    public string Author {get; set; } = null!;
    public decimal Publication_Year {get; set; }
    public string ISBN {get; set;} = null!;
}