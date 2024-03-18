using System;
using Azure;
using Azure.Data.Tables;

namespace BlazorApp.Shared.Models.ShortenedUrl;

public class ShortUrlModel : ITableEntity
{
    public ShortUrlModel()
    {
    }
    
    public ShortUrlModel(string originUrl, string shortUrl)
    {
        this.OriginUrl = originUrl;
        this.ShortUrl = shortUrl;
        this.PartitionKey = shortUrl;
        this.RowKey = shortUrl;
    }
    
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public string OriginUrl { get; set; }
    public string ShortUrl { get; set; }
}