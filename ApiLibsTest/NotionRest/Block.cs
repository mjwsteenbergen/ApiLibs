using NUnit.Framework;
using ApiLibs.NotionRest;
using Newtonsoft.Json;
using System;

namespace ApiLibsTest.NotionRest;

/// <summary>
/// Tests for <see cref="NotionFileConverter"/>
/// </summary>
[TestFixture]
class NotionRestTest
{
    [Test]
    public void SerializeHeading()
    {
        Console.WriteLine(new NotionImage().Type);

        Assert.AreEqual("""{"type":"heading_1","heading_1":{}}""", JsonConvert.SerializeObject(new NotionBlockWrapper
        {
            Value = new Heading1()
        }));
    }

    [Test]
    public void DeserializeHeading()
    {
        var res = JsonConvert.DeserializeObject<NotionBlockWrapper>("""{"type":"heading_1","heading_1":{}}""");

        var expected = new NotionBlockWrapper
        {
            Value = new Heading1()
        };

        Assert.AreEqual(res.Value.Type, expected.Value.Type);
    }

    [Test]
    public void SerializeImage()
    {
        var serializable = new NotionBlockWrapper
        {
            Value = new NotionImage
            {
                Value = new ExternalNotionFile()
                {
                    Url = new Uri("https://website.domain/images/image.png")
                }
            }
        };

        // Example of https://developers.notion.com/reference/block#image
        Assert.AreEqual("""{"type":"image","image":{"type":"external","external":{"url":"https://website.domain/images/image.png"}}}""", JsonConvert.SerializeObject(serializable));
    }

    [Test]
    public void DeserializeImage()
    {
        var res = JsonConvert.DeserializeObject<NotionBlockWrapper>("""{"type":"heading_1","heading_1":{}}""");

        var expected = new NotionBlockWrapper
        {
            Value = new Heading1()
        };

        Assert.AreEqual(res.Value.Type, expected.Value.Type);
    }

    [Test]
    public void SerializeFile()
    {
        Assert.AreEqual("""{"type":"external","external":{}}""", JsonConvert.SerializeObject(new NotionFileWrapper
        {
            Value = new ExternalNotionFile(),
        }));
    }

    [Test]
    public void DeserializeFile()
    {

        var res = JsonConvert.DeserializeObject<NotionFileWrapper>("""{"type":"external","external":{}}""");

        var expected = new NotionFileWrapper
        {
            Value = new ExternalNotionFile(),   
        };

        Assert.AreEqual(res.Value.Type, expected.Value.Type);
    }
}
