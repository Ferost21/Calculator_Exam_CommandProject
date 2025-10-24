using System;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[Binding]
public class BookingApiSteps
{
    private readonly RestClient _client = new RestClient("https://restful-booker.herokuapp.com/");
    private RestResponse _response;
    private int _bookingId;
    private JObject _payload;
    private JObject _createdBooking;

    private string GetAuthToken()
    {
        var req = new RestRequest("auth", Method.Post);
        var auth = new JObject { ["username"] = "admin", ["password"] = "password123" };
        req.AddStringBody(auth.ToString(), "application/json");
        var resp = _client.Execute(req);
        Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode, "Auth request failed");
        var token = JObject.Parse(resp.Content)["token"]?.ToString();
        Assert.IsFalse(string.IsNullOrEmpty(token), "Token is empty");
        return token;
    }

    [AfterScenario]
    public void CleanupCreatedBooking()
    {
        if (_bookingId > 0)
        {
            try
            {
                var token = GetAuthToken();
                var delReq = new RestRequest($"booking/{_bookingId}", Method.Delete);
                delReq.AddHeader("Cookie", $"token={token}");
                var delResp = _client.Execute(delReq);
                Console.WriteLine($"Cleanup: delete booking {_bookingId} status {delResp.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cleanup failed: " + ex.Message);
            }
        }
    }

    [Given(@"I have a valid booking payload")]
    public void GivenIHaveAValidBookingPayload()
    {
        _payload = new JObject
        {
            ["firstname"] = "John",
            ["lastname"] = "Doe",
            ["totalprice"] = 123,
            ["depositpaid"] = true,
            ["bookingdates"] = new JObject
            {
                ["checkin"] = "2024-01-01",
                ["checkout"] = "2024-01-10"
            },
            ["additionalneeds"] = "Breakfast"
        };
    }

    [When(@"I create a new booking")]
    public void WhenICreateANewBooking()
    {
        var request = new RestRequest("booking", Method.Post);
        request.AddStringBody(_payload.ToString(), "application/json");
        _response = _client.Execute(request);
        Assert.AreEqual(HttpStatusCode.OK, _response.StatusCode, "Create booking failed");
        var json = JObject.Parse(_response.Content);
        _bookingId = (int)json["bookingid"];
        _createdBooking = (JObject)json["booking"];
        Assert.IsTrue(_bookingId > 0, "BookingId not returned");
    }

    [Then(@"the booking is created successfully")]
    public void ThenTheBookingIsCreatedSuccessfully()
    {
        Assert.IsTrue(_bookingId > 0);
    }

    [When(@"I get the created booking")]
    public void WhenIGetTheCreatedBooking()
    {
        var request = new RestRequest($"booking/{_bookingId}", Method.Get);
        _response = _client.Execute(request);
        Assert.AreEqual(HttpStatusCode.OK, _response.StatusCode, "Get booking failed");
    }

    [Then(@"the booking data matches the payload")]
    public void ThenTheBookingDataMatchesThePayload()
    {
        var json = JObject.Parse(_response.Content);
        Assert.AreEqual((string)_payload["firstname"], (string)json["firstname"]);
        Assert.AreEqual((string)_payload["lastname"], (string)json["lastname"]);
        Assert.AreEqual((int)_payload["totalprice"], (int)json["totalprice"]);
    }

    [When(@"I update the booking")]
    public void WhenIUpdateTheBooking()
    {
        var updated = new JObject(_payload);
        updated["firstname"] = "Jane";

        var token = GetAuthToken();
        var request = new RestRequest($"booking/{_bookingId}", Method.Put);
        request.AddHeader("Cookie", $"token={token}");
        request.AddStringBody(updated.ToString(), "application/json");
        _response = _client.Execute(request);
    }

    [Then(@"the booking is updated successfully")]
    public void ThenTheBookingIsUpdatedSuccessfully()
    {
        Assert.AreEqual(HttpStatusCode.OK, _response.StatusCode, "Update did not return 200");
        // додаткова перевірка: GET і перевірка зміни
        var getReq = new RestRequest($"booking/{_bookingId}", Method.Get);
        var getResp = _client.Execute(getReq);
        Assert.AreEqual(HttpStatusCode.OK, getResp.StatusCode, "Get after update failed");
        var json = JObject.Parse(getResp.Content);
        Assert.AreEqual("Jane", (string)json["firstname"], "Firstname was not updated");
    }

    [When(@"I delete the booking")]
    public void WhenIDeleteTheBooking()
    {
        var token = GetAuthToken();
        var request = new RestRequest($"booking/{_bookingId}", Method.Delete);
        request.AddHeader("Cookie", $"token={token}");
        _response = _client.Execute(request);
    }

    [Then(@"the booking is deleted successfully")]
    public void ThenTheBookingIsDeletedSuccessfully()
    {
        Assert.AreEqual(HttpStatusCode.Created, _response.StatusCode, "Delete did not return 201");
        // перевірка — GET має повертати 404 або 400
        var getReq = new RestRequest($"booking/{_bookingId}", Method.Get);
        var getResp = _client.Execute(getReq);
        Assert.IsTrue(getResp.StatusCode == HttpStatusCode.NotFound || getResp.StatusCode == HttpStatusCode.BadRequest,
            $"Expected booking to be removed, get returned {getResp.StatusCode}");
        // відмічаємо, що ресурс видалено — щоб cleanup не намагався видаляти ще раз
        _bookingId = 0;
    }
}