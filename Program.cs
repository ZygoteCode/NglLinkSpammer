using FUDChromeDriver;
using OpenQA.Selenium;
using System.Text.RegularExpressions;

public class Program
{
    public static void Main()
    {
        string username = "", message = "";
        Console.Title = "NglLinkSpammer | Made by https://github.com/ZygoteCode/";

        while (username.Trim() == "")
        {
            Console.Write("Please, insert the URI of ngl.link or the username here > ");
            username = Console.ReadLine().Trim();
            string pattern = @"^(https?:\/\/)?ngl\.link\/[a-zA-Z0-9]+$";

            if (Regex.IsMatch(username, pattern))
            {
                string value = Regex.Match(username, pattern).Value;
                string[] splitted = value.Split('/');
                username = splitted[splitted.Length - 1];
            }
        }

        while (message.Trim() == "")
        {
            Console.Write("Please, insert the message here > ");
            message = EscapeCharacters(Console.ReadLine());
        }

        while (true)
        {
            UndetectedChromeDriver driver = UndetectedChromeDriver.Create(
                driverExecutablePath: $"{Environment.CurrentDirectory}\\chromedriver.exe",
                browserExecutablePath: "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
                headless: true
            );

            driver.GoToUrl($"https://ngl.link/{username}");

            while (!driver.IsPageContentLoaded())
            {
                Thread.Sleep(1);
            }

            try
            {
                driver.IsElementReady(driver.FindClickableElement(By.CssSelector("textarea[name='question']")));
                driver.ExecuteScript("const textarea = document.querySelector(\"textarea[name='question']\");\r\n" +
                    $"textarea.value = \"{message}\";\r\n" +
                    "textarea.dispatchEvent(new Event(\"input\", { bubbles: true }));\r\n");
                driver.IsElementReady(driver.FindClickableElement(By.CssSelector("button[class='submit']")));
                driver.ExecuteScript("document.querySelector(\"button[class='submit']\").click();");
                driver.IsElementReady(driver.FindClickableElement(By.CssSelector("img[src='/images/sent.png']")));
                Console.WriteLine("Message sent!");
            }
            finally
            {
                try
                {
                    driver.Close();
                }
                catch
                {

                }

                try
                {

                }
                catch
                {
                    driver.Quit();
                }
            }
        }
    }
    
    private static string EscapeCharacters(string str)
    {
        string result = "";

        foreach (char c in str)
        {
            if (c == '\"')
            {
                result += "\\\"";
            }
            else
            {
                result += c;
            }
        }

        return result;
    }
}