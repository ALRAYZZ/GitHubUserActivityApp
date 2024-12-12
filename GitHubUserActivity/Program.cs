
using GitHubUserActivity;
using System.Text.Json;

Console.WriteLine("Welcome to a GitHub app to check for events made from a specific user");
Console.WriteLine("Type 'exit' to stop the program");



while (true)
{
	Console.Write("Enter GitHub Username: ");
	string username = Console.ReadLine();

	if (username == "exit")
	{
		break;
	}
	else if (!string.IsNullOrEmpty(username))
	{
		await GitHubController.GetGitHubUserActivity(username);
	}
	else
	{
		Console.WriteLine("Username cannot be empty");
	}
}




