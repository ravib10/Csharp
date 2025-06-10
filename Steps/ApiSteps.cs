using NUnit.Framework;
using Reqnroll;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using AventStack.ExtentReports;
using testPRoject.Utils;
using System;

[Binding]
public class ApiSteps
{
    // private HttpClient _client;
    //     private HttpResponseMessage _response;
    //     private string _userId;
    //     private JObject _responseBody;

    //     private ExtentTest _test;
    
    private HttpClient _client = null!;
private HttpResponseMessage _response = null!;
private string _userId = null!;
private JObject _responseBody = null!;
private ExtentTest _test = null!;

    [BeforeScenario]
    public void Setup()

    {
    
        _client = new HttpClient();
        // Create a new ExtentTest instance per scenario using AsyncLocal-based manager
        _test = ExtentReportManager.CreateTest(TestContext.CurrentContext.Test.Name);
       
    }
    

    [Given("I have user ID (.*)")]
    public void GivenIHaveUserId(string userId)
    {
        _userId = userId;
        //this is how you pass the data in report.
        _test.Info($"User ID set to {_userId}");
    }

    [When("I request the user details")]
    public async Task WhenIRequestTheUserDetails()
    {
        var url = $"https://jsonplaceholder.typicode.com/users/{_userId}";
        _response = await _client.GetAsync(url);
        var jsonString = await _response.Content.ReadAsStringAsync();
        _responseBody = JObject.Parse(jsonString);

        //this is how you pass the data in report.
        _test.Info($"Requested URL: {url}");
        _test.Info($"Response: {jsonString}");
    }

    [Then(@"the user name should be ""(.*)""")]
    public void ThenTheUserNameShouldBe(string expectedName)
    {
        var actualName = _responseBody["name"].ToString();
        _test.Info($"Asserting user name. Expected: {expectedName}, Actual: {actualName}");
   try
    {
        Assert.That(actualName, Is.EqualTo("expectedName"), "User name did not match");
        _test.Pass("User name matched.");
    }
    catch (Exception ex)
    {
        _test.Fail($"Assertion failed: {ex.Message}");
        throw;  // important: rethrow exception to mark test as failed in test runner
    }
        Console.WriteLine("response is  "+_responseBody.ToString()); 
    }

    [AfterScenario]
    public void TearDown()
    {
        _client.Dispose();
        // Do NOT flush here if tests run in parallel, flush once globally after all tests
        //  ExtentReportManager.FlushReport();
    }
}
