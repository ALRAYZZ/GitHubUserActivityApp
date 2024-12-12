using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GitHubUserActivity;

public static class GitHubController
{
	public static async Task GetGitHubUserActivity(string username)
	{
		using (HttpClient client = new HttpClient())
		{
			client.DefaultRequestHeaders.UserAgent.ParseAdd("C# App");

			string url = $"https://api.github.com/users/{username}/events";
			HttpResponseMessage response = await client.GetAsync(url);


			if (response.IsSuccessStatusCode)
			{
				string responseBody = await response.Content.ReadAsStringAsync();
				if (!string.IsNullOrEmpty(responseBody))
				{
					using (JsonDocument doc = JsonDocument.Parse(responseBody))
					{
						if (doc.RootElement.GetArrayLength() == 0)
						{
							Console.WriteLine("No events found for this user");
							return;
						}
						Dictionary<string, EventModel> eventCounts = new Dictionary<string, EventModel>();
						foreach (JsonElement element in doc.RootElement.EnumerateArray())
						{
							string type = element.TryGetProperty("type", out JsonElement typeElement) ? typeElement.GetString() : "N/A";
							if (eventCounts.ContainsKey(type))
							{
								eventCounts[type].Count++;
							}
							else
							{
								eventCounts[type] = new EventModel { Type = type, Count = 1 };
							}
						}
						foreach (var eventCount in eventCounts.Values)
						{
							Console.WriteLine($"{username} did: {eventCount.Count} - {eventCount.Type}");
						}
					}
				}
			}
			else
			{
				Console.WriteLine($"Error: {response.StatusCode}");
			}
		}
	}
}
