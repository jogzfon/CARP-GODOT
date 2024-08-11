using Godot;
using OpenAI_API.Completions;
using OpenAI_API;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

public partial class ai_system : Control
{
    [Export] private VBoxContainer _chatContainer;
    [Export] private TextEdit _request;
    [Export] private Button _sendRequestButton;
    [Export] private TextureButton _backButton;

    private string apiKey = "sk-Is_d8Ut7qdJkcnGqqogQw35GCGNwpGYtszZ8yJyRdcT3BlbkFJy1_hKpYEAl-c6am6m_TmsbeiPVNRxgCMWEhKkIy90A";  // Replace with your actual API key

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
        DisplayUserMessage(_request.Text);
        await GenerateCode(_request.Text);
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

    private async Task GenerateCode(string userPrompt)
    {
        var apiAuthentication = new APIAuthentication(apiKey);
        var openAiApi = new OpenAIAPI(apiAuthentication);

        try
        {
            string message = $"You are provided with system architecture details, a code example, and a user's request. " +
                             $"Based on this information, generate a program that fulfills the user's request." +
                             $"\n\n Information on System Architecture:\n{aiMessageDoc.information}" +
                             $"\n\n Code Example:\n{aiMessageDoc.format}" +
                             $"\n\n User's Request:\n{userPrompt}" +
                             $"\n\n Your response should:" +
                             $"\n- Be based solely on the provided system architecture information." +
                             $"\n- Use only the instruction set codes defined." +
                             $"\n- Clearly state if the request cannot be fulfilled with the provided information and format." +
                             $"\n\nGenerated Code:";

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

            //GD.Print($"Generated Text: {completionResult.Completions[0].Text}");

            DisplayGeneratedCode(completionResult.Completions[0].Text);
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
        
        // Add or update the `TextEdit` widget
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
