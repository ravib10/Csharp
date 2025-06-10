using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.IO;
using System.Threading;

namespace testPRoject.Utils
{
    public static class ExtentReportManager
    {
        private static ExtentReports _extent;
        private static ExtentSparkReporter _sparkReporter;
        private static readonly object _lock = new object();

        // Use AsyncLocal to store ExtentTest per async/thread context
        private static AsyncLocal<ExtentTest> _currentTest = new AsyncLocal<ExtentTest>();

        public static ExtentReports GetInstance()
        {
            if (_extent == null)
            {
                lock (_lock)
                {
                    if (_extent == null)
                    {
                        string reportDir;// = Path.Combine(AppContext.BaseDirectory, "TestResults");
                        reportDir = "/Users/ravinder.budhawan/Desktop/Octopus/TestFolder/testPRoject/TestResults/";
                     
                        Directory.CreateDirectory(reportDir);  // Ensure folder exists
                           Console.WriteLine("Report directory1: " + reportDir);

                        string reportPath = Path.Combine(reportDir, "ExtentReport.html");
                        Console.WriteLine("Report reportPath1 : " + reportPath);
                        _sparkReporter = new ExtentSparkReporter(reportPath);

                        _extent = new ExtentReports();
                        _extent.AttachReporter(_sparkReporter);
                    }
                }
            }
            return _extent;
        }

        // Create a new test and store it in AsyncLocal
        public static ExtentTest CreateTest(string testName)
        {
            var test = GetInstance().CreateTest(testName);
            _currentTest.Value = test;
            return test;
        }

        // Get the current test instance for this async context
        public static ExtentTest GetTest()
        {
            return _currentTest.Value;
        }

        // Flush the report (call this once after all tests finish)
        public static void FlushReport()
        {
            GetInstance().Flush();

        }
        
        
    }
}
