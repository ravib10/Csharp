
// using AventStack.ExtentReports;
// using AventStack.ExtentReports.Reporter;
// using AventStack.ExtentReports.Gherkin.Model;

// using Reqnroll;
// using System;
// using System.Collections.Concurrent;
// using System.IO;
// using System.Threading;
// using AventStack.ExtentReports.Gherkin;

// using log4net;
// using log4net.Config;
// using AventStack.ExtentReports.MarkupUtils;

// [Binding]
// public class ExtentReportHooks
// {
//     private static readonly object _lock = new object();
//     private static ExtentReports _extent;
//     private static ExtentSparkReporter _htmlReporter;
//     public string customisedMessage;

//     // Stores one feature node per feature title
//     private static ConcurrentDictionary<string, ExtentTest> _featureNodes = new ConcurrentDictionary<string, ExtentTest>();

//     // Each thread gets its own scenario node
//     private static ThreadLocal<ExtentTest> _scenarioNode = new ThreadLocal<ExtentTest>();
   
//     private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

//     private readonly ScenarioContext _scenarioContext;
//     private readonly FeatureContext _featureContext;
//     public void message(string message){
//         customisedMessage=message;
//     }

//     public ExtentReportHooks(ScenarioContext scenarioContext, FeatureContext featureContext)
//     {
//         _scenarioContext = scenarioContext;
//         _featureContext = featureContext;
//     }

//     [BeforeTestRun]
//     public static void BeforeTestRun()
//     {
//         string reportPath = "./../../ExtentReports/ExtentReport.html";
//         _htmlReporter = new ExtentSparkReporter(reportPath);
//         _htmlReporter.Config.DocumentTitle = "Automation Report";
//         _htmlReporter.Config.ReportName = "Reqnroll Parallel Execution";
      
//         _extent = new ExtentReports();
//         _extent.AttachReporter(_htmlReporter);
//     }
//          string featureTitle ;
//         string scenarioTitle;

//     [BeforeScenario]
//     public void BeforeScenario()
//     {
//         var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
//         var v = new FileInfo(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Support/log4net.config");
//         XmlConfigurator.Configure(logRepository, new FileInfo(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Support/log4net.config"));

//         featureTitle = _featureContext.FeatureInfo.Title;
//         scenarioTitle = _scenarioContext.ScenarioInfo.Title;

//         ExtentTest featureNode;

//         // Lock to ensure only one thread creates the feature node
//         lock (_lock)
//         {
//             if (!_featureNodes.ContainsKey(featureTitle))
//             {
//                 featureNode = _extent.CreateTest<Feature>(featureTitle);
//                 _featureNodes.TryAdd(featureTitle, featureNode);
//             }
//             else
//             {
//                 featureNode = _featureNodes[featureTitle];
//             }
//         }

//         // Each scenario node is a child of its feature node
//         _scenarioNode.Value = featureNode.CreateNode<Scenario>(scenarioTitle);
//          var __ = _scenarioNode.Value;
        
      
       
//     }

//     [AfterStep]
//     public void AfterStep()
//     {
//                 var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
//                 var stepText = _scenarioContext.StepContext.StepInfo.Text;
//                 Boolean val;
//                 if (_scenarioNode.Value == null)
//                 {
//                      val = false;
//         }
//           log.Info("Executing feature : " + featureTitle +".   Executing Scenario :"+ scenarioTitle+".                        Exectuting step" +stepText);
//                 if (_scenarioContext.TestError == null)

//                 {//if you want to pass any additional message to your step, use following ravi
//                  // _scenarioNode.Value.CreateNode(new GherkinKeyword(stepType), stepText).Pass("You can pass your customised message or leave it blank");
            
//                     //if you don't want to pass a message to your step, then use following
//                     _scenarioNode.Value.CreateNode(new GherkinKeyword(stepType), stepText);

//                     //following will add the details in the log file.
//                     log.Info("step pass " + stepText);


//                 }
//                 else if (_scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.StepDefinitionPending || _scenarioContext.TestError is PendingStepException)
//                 {
//                     _scenarioNode.Value
//                         .CreateNode(new GherkinKeyword(stepType), stepText)
//                         .Fail("Step definition is pending or not implemented.");

//                     // log.Error("Pending step failed: " + stepText);

//                     // Forcefully fail the test
//                     throw new Exception("Pending step encountered: " + stepText);
//                 }
//                 else
//                 {
//                     // var mediaEntity = CaptureScreenshotAndReturnModel(_scenarioContext.ScenarioInfo.Title);
//                     // _scenarioNode.Value
//                     //     .CreateNode(new GherkinKeyword(stepType), stepText)
//                     //     .Fail(_scenarioContext.TestError.Message, mediaEntity);

//                     _scenarioNode.Value
//                                      .CreateNode(new GherkinKeyword(stepType), _scenarioContext.StepContext.StepInfo.Text)
//                                      .Fail(_scenarioContext.TestError.Message);


//                 }




//         // var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
//         // var stepText = _scenarioContext.StepContext.StepInfo.Text;
//         // var featureTitle = _featureContext.FeatureInfo.Title;
//         // var scenarioTitle = _scenarioContext.ScenarioInfo.Title;

//         // log.Info($"Executing Feature: {featureTitle} | Scenario: {scenarioTitle} | Step: {stepText}");

//         // // Defensive check
//         // if (_scenarioNode?.Value == null)
//         // {
//         //     log.Warn("WARNING: _scenarioNode.Value is null in AfterStep(). Skipping Extent logging for this step.");
//         //     return;
//         // }

//         // try
//         // {
//         //     if (_scenarioContext.TestError == null)
//         //     {
//         //         _scenarioNode.Value
//         //             .CreateNode(new GherkinKeyword(stepType), stepText)
//         //             .Pass("Step passed");

//         //         log.Info("Step passed: " + stepText);
//         //     }
//         //     else if (_scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.StepDefinitionPending ||
//         //              _scenarioContext.TestError is PendingStepException)
//         //     {
//         //         _scenarioNode.Value
//         //             .CreateNode(new GherkinKeyword(stepType), stepText)
//         //             .Skip("Step definition is pending or not implemented.");

//         //         log.Warn("Pending step skipped: " + stepText);

//         //         // Optional: throw to make test fail
//         //         // throw new Exception("Pending step encountered: " + stepText);
//         //     }
//         //     else
//         //     {
//         //         _scenarioNode.Value
//         //             .CreateNode(new GherkinKeyword(stepType), stepText)
//         //             .Fail(_scenarioContext.TestError.Message);

//         //         log.Error("Step failed: " + stepText + " | Error: " + _scenarioContext.TestError.Message);
//         //     }
//         // }
//         // catch (Exception ex)
//         // {
//         //     log.Error("Error during AfterStep Extent logging: " + ex.Message);
//         // }
    
//     }

//     [AfterTestRun]
//     public static void AfterTestRun()
//     {
//         _extent.Flush();
//     }



//     [AfterScenario]
//     public void AfterScenario()
//     {
//       var scenarioStatus = _scenarioContext.ScenarioExecutionStatus;

//     if (scenarioStatus == ScenarioExecutionStatus.StepDefinitionPending||_scenarioContext.TestError is PendingStepException)
//     {
//         string message = $"❌ Scenario '{scenarioTitle}' failed due to missing/unimplemented step definitions.";
//         _scenarioNode.Value.Fail(message);
//         log.Error(message);
//         throw new Exception(message);
//     }
//     else if (_scenarioContext.TestError != null)
//     {
//         string errorMessage = $"❌ Scenario '{scenarioTitle}' failed with error: {_scenarioContext.TestError.Message}";
//         string stackTrace = _scenarioContext.TestError.StackTrace;

//         _scenarioNode.Value.Fail(errorMessage);
//         _scenarioNode.Value.Info($"📌 Stack Trace:<pre>{stackTrace}</pre>");
//         log.Error(errorMessage);
//     }
//     else
//     {
//         string passMessage = $"✅ Scenario '{scenarioTitle}' executed successfully.";
//         _scenarioNode.Value.Pass(passMessage);
//         _scenarioNode.Value.Info("ℹ️ This is an extra note in the report for passed scenario.");
//         log.Info(passMessage);
//     }
//         // Optional: cleanup or flush scenario-specific logs
//     }
// }











using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Gherkin.Model;
using Reqnroll;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using AventStack.ExtentReports.MarkupUtils;
using log4net;
using log4net.Config;
using AventStack.ExtentReports.Gherkin;

[Binding]
public class ExtentReportHooks
{
    private static readonly object _lock = new object();
  private static ExtentReports _extent = null!;
private static ExtentSparkReporter _htmlReporter = null!;

    private static ConcurrentDictionary<string, ExtentTest> _featureNodes = new ConcurrentDictionary<string, ExtentTest>();

private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType ?? typeof(object));


    private readonly ScenarioContext _scenarioContext;
    private readonly FeatureContext _featureContext;

    private string featureTitle;
    private string scenarioTitle;
    private string customisedMessage;

    private ExtentTest ScenarioNode
    {
        get => _scenarioContext.ContainsKey("ScenarioNode") ? (ExtentTest)_scenarioContext["ScenarioNode"] : null;
        set => _scenarioContext["ScenarioNode"] = value;
    }

    public ExtentReportHooks(ScenarioContext scenarioContext, FeatureContext featureContext)
    {
        _scenarioContext = scenarioContext;
        _featureContext = featureContext;
    }

    public void Message(string message)
    {
        customisedMessage = message;
    }

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        string currentDir = Directory.GetCurrentDirectory();
        string projectFolderPath = Directory.GetParent(currentDir)?.Parent?.Parent?.FullName;

        string reportPath = projectFolderPath+"/ExtentReports/ExtentReport.html";
        Directory.CreateDirectory(Path.GetDirectoryName(reportPath));

        _htmlReporter = new ExtentSparkReporter(reportPath);
        _htmlReporter.Config.DocumentTitle = "Automation Report";
        _htmlReporter.Config.ReportName = "Reqnroll Parallel Execution";

        _extent = new ExtentReports();
        _extent.AttachReporter(_htmlReporter);
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        var logConfigPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Support", "log4net.config");
        XmlConfigurator.Configure(logRepository, new FileInfo(logConfigPath));

        featureTitle = _featureContext.FeatureInfo.Title;
        scenarioTitle = _scenarioContext.ScenarioInfo.Title;

        ExtentTest featureNode;

        lock (_lock)
        {
            if (!_featureNodes.ContainsKey(featureTitle))
            {
                featureNode = _extent.CreateTest<Feature>(featureTitle);
                _featureNodes.TryAdd(featureTitle, featureNode);
            }
            else
            {
                featureNode = _featureNodes[featureTitle];
            }
        }

        ScenarioNode = featureNode.CreateNode<Scenario>(scenarioTitle);
    }

    [AfterStep]
    public void AfterStep()
    {
       var stepInfo = _scenarioContext.StepContext?.StepInfo;
if (stepInfo == null)
{
    log.Warn("StepInfo is null. Skipping step logging.");
    return;
}

var stepType = stepInfo.StepDefinitionType.ToString();
var stepText = stepInfo.Text;

        log.Info($"Executing Feature: {featureTitle} | Scenario: {scenarioTitle} | Step: {stepText}");

        if (ScenarioNode == null)
        {
            log.Warn("ScenarioNode is null. Skipping Extent logging for this step.");
            return;
        }

        try
        {
            if (_scenarioContext.TestError == null)
            {
                ScenarioNode.CreateNode(new GherkinKeyword(stepType), stepText).Pass(customisedMessage ?? "Step passed");
                log.Info("Step passed: " + stepText);
            }
            else if (_scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.StepDefinitionPending ||
                     _scenarioContext.TestError is PendingStepException)
            {
                ScenarioNode.CreateNode(new GherkinKeyword(stepType), stepText)
                            .Skip("Step definition is pending or not implemented.");
                log.Warn("Pending step skipped: " + stepText);
            }
            else
            {
                ScenarioNode.CreateNode(new GherkinKeyword(stepType), stepText)
                            .Fail(_scenarioContext.TestError.Message);
                log.Error("Step failed: " + stepText + " | Error: " + _scenarioContext.TestError.Message);
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in AfterStep Extent logging: " + ex.Message);
        }
    }

    [AfterScenario]
    public void AfterScenario()
    {
        var scenarioStatus = _scenarioContext.ScenarioExecutionStatus;

        if (ScenarioNode == null)
        {
            log.Warn("ScenarioNode is null in AfterScenario. Cannot log scenario result.");
            return;
        }

        if (scenarioStatus == ScenarioExecutionStatus.StepDefinitionPending || _scenarioContext.TestError is PendingStepException)
        {
            string message = $"❌ Scenario '{scenarioTitle}' failed due to missing/unimplemented step definitions.";
            ScenarioNode.Fail(message);
            log.Error(message);
            throw new Exception(message);
        }
        else if (_scenarioContext.TestError != null)
        {
            string errorMessage = $"❌ Scenario '{scenarioTitle}' failed with error: {_scenarioContext.TestError.Message}";
            string stackTrace = _scenarioContext.TestError.StackTrace;

            ScenarioNode.Fail(errorMessage);
            ScenarioNode.Info($"📌 Stack Trace:<pre>{stackTrace}</pre>");
            log.Error(errorMessage);
        }
        else
        {
            string passMessage = $"✅ Scenario '{scenarioTitle}' executed successfully.";
            ScenarioNode.Pass(passMessage);
            ScenarioNode.Info("ℹ️ This is an extra note in the report for passed scenario.");
            log.Info(passMessage);
        }
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        _extent.Flush();
    }
}
