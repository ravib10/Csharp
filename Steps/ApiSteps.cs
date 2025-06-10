using NUnit.Framework;
using Reqnroll;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using AventStack.ExtentReports;
using testPRoject.Utils;
using System;
using log4net;
using System.IO;
using log4net.Config;



[Binding]
public class ApiSteps
{
    // private HttpClient _client;
    //     private HttpResponseMessage _response;
    //     private string _userId;
    //     private JObject _responseBody;

    //     private ExtentTest _test;
    
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private HttpClient _client = null!;
private HttpResponseMessage _response = null!;
private string _userId = null!;
private JObject _responseBody = null!;
    private ExtentTest _test = null!;
private readonly ScenarioContext _scenarioContext;
 public ApiSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

    [BeforeScenario]
    public void Setup()

    {
    
            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            var v = new FileInfo(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Support/log4net.config");
            XmlConfigurator.Configure(logRepository, new FileInfo(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Support/log4net.config"));
            log.Info("yuppie");

            
        _client = new HttpClient();
        // Create a new ExtentTest instance per scenario using AsyncLocal-based manager
        _test = ExtentReportManager.CreateTest(TestContext.CurrentContext.Test.Name);
       
    }

[BeforeStep]
        public void BeforeEachStep()
        {
            // Access the step text from ScenarioContext
            // Note: CurrentStepInfo.StepDefinition.Text might be null if called too early or in certain async contexts
            // If Text is null, fallback to StepDefinition.MethodInfo.Name
        var stepText = _scenarioContext.StepContext?.StepInfo?.Text ??
               _scenarioContext.StepContext?.StepInfo?.BindingMatch?.StepBinding?.Method?.Name ??
               "Unknown Step";
                           

            // Log to the Serilog logger instance that was initialized in Program.cs
        log.Info($"Entering Step: '{stepText}'");
        }

        [AfterStep]
        public void AfterEachStep()
        {
            // Access the step text from ScenarioContext
            // Note: CurrentStepInfo.StepDefinition.Text might be null if called too early or in certain async contexts
            // If Text is null, fallback to StepDefinition.MethodInfo.Name
        var stepText = _scenarioContext.StepContext?.StepInfo?.Text ??
               _scenarioContext.StepContext?.StepInfo?.BindingMatch?.StepBinding?.Method?.Name ??
               "Unknown Step";
                           

            // Log to the Serilog logger instance that was initialized in Program.cs
        log.Info($"Exiting Step: '{stepText}'");
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
        Assert.That(actualName, Is.EqualTo(expectedName), "User name did not match");
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
