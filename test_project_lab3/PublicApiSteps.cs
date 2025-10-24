using System;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[Binding]
public class PublicApiSteps
{
    private readonly RestClient _client = new RestClient("https://api.publicapis.org/");
    private RestResponse _response;

    [When(@"I request entries for category ""(.*)""")]
    public void WhenIRequestEntriesForCategory(string category)
    {
        var req = new RestRequest("entries", Method.Get);
        req.AddParameter("category", category);
        _response = _client.Execute(req);
    }

    [Then(@"response status should be 200")]
    public void ThenResponseStatusShouldBe200()
    {
        Assert.AreEqual(HttpStatusCode.OK, _response.StatusCode, "Unexpected HTTP status");
    }

    [Then(@"response should contain at least one entry")]
    public void ThenResponseShouldContainAtLeastOneEntry()
    {
        var json = JObject.Parse(_response.Content);
        var entries = (JArray)json["entries"];
        Assert.IsTrue(entries != null && entries.Count > 0, "No entries returned");
    }

    [Then(@"each entry should contain fields ""(.*)"" and ""(.*)""")]
    public void ThenEachEntryShouldContainFields(string field1, string field2)
    {
        var json = JObject.Parse(_response.Content);
        var entries = (JArray)json["entries"];
        foreach (JObject e in entries)
        {
            Assert.IsTrue(e.ContainsKey(field1), $"Entry missing field {field1}");
            Assert.IsTrue(e.ContainsKey(field2), $"Entry missing field {field2}");
        }
    }
}