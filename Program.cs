using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Chat;


var apiKey =Environment.GetEnvironmentVariable("OPENAI_API_KEY")
            ?? throw new InvalidOperationException("Set OPENAI_API_KEY");

var agent = new OpenAIClient(apiKey).GetChatClient("gpt-4o-mini").CreateAIAgent(
    name: "Assistant",
    instructions: """
            You are a friendly and knowledgeable AI assistant.
            Be concise but thorough. If you don't know something, admit it.
            Always respond in the user's language.
            """);

Console.WriteLine("Agent ready! Type 'exit' to quit.\n");

var thread = agent.GetNewThread();

while (true)
{
    Console.Write("You > ");
    var input = Console.ReadLine();

    if (string.IsNullOrEmpty(input)) continue;
    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

    Console.Write("Agent > ");
    await foreach (var update in agent.RunStreamingAsync(input, thread)) 
    {
        Console.Write(update.ToString());
    }

    Console.WriteLine("\n");
}