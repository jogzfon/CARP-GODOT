using Godot;
using OpenAI_API.Completions;
using OpenAI_API;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class ai_system : Control
{
    [Export] private VBoxContainer _chatContainer;
    [Export] private TextEdit _request;
    [Export] private Button _sendRequestButton;
    [Export] private TextureButton _backButton;

    private string apiKey = "sk-SyE75X15zKdupnh8v9O-DtOKzOsnPCMp0pwCdoidW0T3BlbkFJGfbtjheSvAP-XvOmUxqhlkyCqGT0m3u1fh0PR4jdIA";  // Replace with your actual API key

    // State variables
    private List<string> conversationHistory = new List<string>();
    private Dictionary<string, string> sessionData = new Dictionary<string, string>();

    public override void _Ready()
    {
        _sendRequestButton.Connect("pressed", new Callable(this, nameof(OnSendRequestPressed)));
        _backButton.Connect("pressed", new Callable(this, nameof(OnBackPressed)));
    }

    public override void _Process(double delta) { }

    private void OnBackPressed()
    {
        this.Visible = false;
    }
    private async void OnSendRequestPressed()
    {
        string userMessage = _request.Text;
        DisplayUserMessage(userMessage);
        conversationHistory.Add(userMessage);
        await GenerateCodeAsync(userMessage);
        _request.Clear();
    }

    private void DisplayUserMessage(string message)
    {
        var label = new Label
        {
            Text = message,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        label.Set("theme_override_fonts/font", "res://Fonts/Inter-Regular.ttf");
        _chatContainer.AddChild(label);
    }

    private async Task GenerateCodeAsync(string userPrompt)
    {
        var apiAuthentication = new APIAuthentication(apiKey);
        var openAiApi = new OpenAIAPI(apiAuthentication);

        try
        {
            // Including conversation history in the prompt to make the session stateful
            string history = string.Join("\n", conversationHistory);
            string message = $@" Your name is CARPVisor and you are provided with details about the system architecture, a code example, and the user's request.
                Information on System Architecture:
                {aiMessageDoc.information}

                Code Example:
                {aiMessageDoc.format}

                Conversation History:
                {history}

                User's Request:
                {userPrompt}

                Your response should:
                1. If the user's request is a greeting or a question about the system architecture:
                   - Respond appropriately (e.g., 'Hello!', 'Hi there!', 'Good day!')
                   - Answer questions based on the provided system architecture information
                   - Offer to assist with programming tasks if the request is unclear

                2. If the user's request involves a programming task:
                   - Generate code that fulfills the request
                   - Base the response solely on the provided system architecture information
                   - Use only the instruction set codes defined for creating programs
                   - Indicate if the request cannot be fulfilled with the provided information and format

                Please provide your response below:";

            var completionRequest = new CompletionRequest
            {
                Prompt = message,
                Model = "gpt-3.5-turbo-instruct",
                MaxTokens = 1500,
            };

            var loadingLabel = new Label { Text = "Generating code...", HorizontalAlignment = HorizontalAlignment.Center };
            _chatContainer.AddChild(loadingLabel);

            var completionResult = await openAiApi.Completions.CreateCompletionAsync(completionRequest);

            _chatContainer.RemoveChild(loadingLabel);

            string generatedText = completionResult.Completions[0].Text;
            conversationHistory.Add(generatedText);
            DisplayGeneratedCode(generatedText);
        }
        catch (Exception ex)
        {
            Debug.Print($"Error: {ex.Message}");
            DisplayErrorMessage(ex.Message);
        }
    }

    private void DisplayGeneratedCode(string generatedText)
    {
        var generatedCodeBox = new TextEdit
        {
            Text = generatedText,
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill,
            WrapMode = TextEdit.LineWrappingMode.Boundary,
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            ScrollFitContentHeight = true
        };

        generatedCodeBox.Set("theme_override_fonts/font", "res://Fonts/Inter-Regular.ttf");

        if (_chatContainer.GetChildCount() > 0 && _chatContainer.GetChild(_chatContainer.GetChildCount() - 1) is TextEdit existingTextEdit)
        {
            _chatContainer.RemoveChild(existingTextEdit);
            existingTextEdit.QueueFree();
        }

        _chatContainer.AddChild(generatedCodeBox);
    }

    private void DisplayErrorMessage(string errorMessage)
    {
        var errorLabel = new Label
        {
            Text = $"Error: {errorMessage}",
            HorizontalAlignment = HorizontalAlignment.Center,
            Modulate = new Color(1, 0, 0) // Red color to indicate error
        };
        _chatContainer.AddChild(errorLabel);
    }
}
